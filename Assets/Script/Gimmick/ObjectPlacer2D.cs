using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectPlacer2D : MonoBehaviour
{
    [Header("設定")]
    public GameObject prefabToPlace;
    public LayerMask placementLayerMask;
    public int maxBlocks = 10;

    [Header("オブジェクト設定")]
    public bool setColliderAsTrigger = false;

    [Header("ルール設定")]
    public bool requirePowerPathForPlacement = true;

    [Header("初期オブジェクトの削除")]
    public bool allowDeletingInitialObjects = true;

    [Header("UI参照")]
    public Text remainingBlocksText;
    public Text totalPlacedText;

    [Header("プレイヤー周囲の配置制限")]
    public Transform playerTransform;
    public int noPlacementWidth = 3;
    public int noPlacementHeight = 2;

    [Header("識別子")]
    public string placerId = "Placer_1";

    [Header("クールダウン設定")]
    public float collectCooldown = 1f;
    private float lastCollectTime = -Mathf.Infinity;

    [Header("回収制限時間")]
    public float autoCollectBlockTimeout = 10f;

    private int totalPlacedCount = 0;
    public int TotalPlacedCount => totalPlacedCount;

    private enum Mode { None, Place, Delete }
    private Mode currentMode = Mode.None;

    private Vector2? lastProcessedGridPos = null;

    // 状態情報付きのデータ構造に変更
    private Dictionary<Vector2, (GameObject obj, bool canDelete, bool isInitialObject, float placedTime)>
        placedObjects = new Dictionary<Vector2, (GameObject, bool, bool, float)>();

    public int MaxBlocks => maxBlocks;

    public static ObjectPlacer2D FindPlacerById(string id)
    {
        ObjectPlacer2D[] placers = FindObjectsOfType<ObjectPlacer2D>();
        foreach (var placer in placers)
        {
            if (placer.placerId == id)
                return placer;
        }

        Debug.LogWarning($"Placer ID '{id}' が見つかりませんでした");
        return null;
    }

    void Start()
    {
        ElectricGimmick[] gimmicks = GetComponentsInChildren<ElectricGimmick>(true);
        foreach (var gimmick in gimmicks)
        {
            GameObject obj = gimmick.gameObject;

            if (obj.name.StartsWith(prefabToPlace.name) || obj.name.StartsWith("Insulator"))
            {
                obj.transform.SetParent(transform, worldPositionStays: true);

                Vector2 gridPos = ToGridPosition(obj.transform.position);
                if (!placedObjects.ContainsKey(gridPos))
                {
                    placedObjects.Add(gridPos, (obj, allowDeletingInitialObjects, true, -1f));
                }
                else
                {
                    Debug.LogWarning($"同じ位置に既にオブジェクトがあります: {gridPos}");
                }
            }
        }

        UpdateUI();
    }

    public void RegisterPreplacedObject(GameObject obj)
    {
        obj.transform.SetParent(transform, worldPositionStays: true);
        Vector2 gridPos = ToGridPosition(obj.transform.position);

        if (!placedObjects.ContainsKey(gridPos))
        {
            placedObjects.Add(gridPos, (obj, allowDeletingInitialObjects, true, -1f));
            Debug.Log($"AutoCreateGimmickで登録: {gridPos}");
        }
        else
        {
            Debug.LogWarning($"登録失敗：既に存在しています: {gridPos}");
        }

        UpdateUI();
    }

    void Update()
    {
        if (PauseManager.IsPaused || Time.timeScale == 0) return;

        if (Input.GetKeyDown(KeyCode.C))
        {
            if (Time.time - lastCollectTime >= collectCooldown)
            {
                CollectAllPlacedObjects();
                lastCollectTime = Time.time;
            }
            else
            {
                Debug.Log("回収はクールタイム中です");
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0f;
            Vector2 gridPosition = ToGridPosition(mouseWorldPos);

            currentMode = placedObjects.ContainsKey(gridPosition) ? Mode.Delete : Mode.Place;
            ProcessAt(gridPosition, mouseWorldPos);
            lastProcessedGridPos = gridPosition;
        }
        else if (Input.GetMouseButton(0))
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0f;
            Vector2 gridPosition = ToGridPosition(mouseWorldPos);

            if (lastProcessedGridPos != gridPosition)
            {
                ProcessAt(gridPosition, mouseWorldPos);
                lastProcessedGridPos = gridPosition;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            currentMode = Mode.None;
            lastProcessedGridPos = null;
        }
    }

    private void ProcessAt(Vector2 gridPos, Vector3 mouseWorldPos)
    {
        if (currentMode == Mode.Delete && placedObjects.ContainsKey(gridPos))
        {
            TryDeleteAt(gridPos);
        }
        else if (currentMode == Mode.Place && !placedObjects.ContainsKey(gridPos))
        {
            TryPlaceAt(gridPos, mouseWorldPos);
        }
    }

    private void TryDeleteAt(Vector2 gridPos)
    {
        var (obj, canDelete, isInitial, _) = placedObjects[gridPos];
        // 左クリック回収は常に可能（canDeleteは使わない）
        Destroy(obj);
        placedObjects.Remove(gridPos);

        Debug.Log($"オブジェクトを削除しました at {gridPos}");
        UpdateUI();
    }

    //private void TryPlaceAt(Vector2 gridPos, Vector3 mouseWorldPos)
    //{
    //    if (IsNearPlayer(gridPos))
    //    {
    //        Debug.Log("プレイヤーの近くには配置できません");
    //        return;
    //    }

    //    int currentPlaceableCount = CountDeletableObjects();
    //    if (currentPlaceableCount >= maxBlocks)
    //    {
    //        Debug.Log("これ以上ブロックを配置できません（上限）");
    //        return;
    //    }

    //    RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero, 0f, placementLayerMask);
    //    if (hit.collider != null && hit.collider.transform.IsChildOf(transform))
    //    {
    //        GameObject placedObject = Instantiate(prefabToPlace, gridPos, Quaternion.identity, transform);

    //        Collider2D collider = placedObject.GetComponent<Collider2D>();
    //        if (collider != null)
    //        {
    //            collider.isTrigger = setColliderAsTrigger;
    //        }

    //        float currentTime = Time.time;
    //        placedObjects.Add(gridPos, (placedObject, true, false, currentTime));

    //        totalPlacedCount++;
    //        Debug.Log($"オブジェクトを配置しました at {gridPos}（累積: {totalPlacedCount}）");

    //        UpdateUI();
    //    }
    //    else
    //    {
    //        Debug.Log("配置できる場所ではありません（他ギミックの領域？）");
    //    }
    //}
    private void TryPlaceAt(Vector2 gridPos, Vector3 mouseWorldPos)
    {
        if (IsNearPlayer(gridPos))
        {
            Debug.Log("プレイヤーの近くには配置できません");
            return;
        }

        int currentPlaceableCount = CountDeletableObjects();
        if (currentPlaceableCount >= maxBlocks)
        {
            Debug.Log("これ以上ブロックを配置できません（上限）");
            return;
        }

        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero, 0f, placementLayerMask);
        if (hit.collider != null && hit.collider.transform.IsChildOf(transform))
        {
            GameObject placedObject = Instantiate(prefabToPlace, gridPos, Quaternion.identity, transform);

            Collider2D collider = placedObject.GetComponent<Collider2D>();
            if (collider != null)
            {
                collider.isTrigger = setColliderAsTrigger;
            }

            float currentTime = Time.time;
            placedObjects.Add(gridPos, (placedObject, true, false, currentTime));

            //グリッド可視化中なら色変更
            CreateMode createMode = FindObjectOfType<CreateMode>();
            if (createMode != null && createMode.IsActive)
            {
                SpriteRenderer sr = placedObject.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    sr.color = new Color(1f, 1f, 1f, 0.5f);
                }
            }

            totalPlacedCount++;
            Debug.Log($"オブジェクトを配置しました at {gridPos}（累積: {totalPlacedCount}）");

            UpdateUI();
        }
        else
        {
            Debug.Log("配置できる場所ではありません（他ギミックの領域？）");
        }
    }

    private void CollectAllPlacedObjects()
    {
        List<Vector2> keysToRemove = new List<Vector2>();
        int collectedCount = 0;
        float currentTime = Time.time;

        foreach (var kvp in placedObjects)
        {
            var (obj, canDelete, isInitial, placedTime) = kvp.Value;

            bool isOlderThanTimeout = placedTime > 0f && (currentTime - placedTime >= autoCollectBlockTimeout);

            // isInitial → 回収不可、10秒以上 → 回収不可
            if (!isInitial && !isOlderThanTimeout)
            {
                Destroy(obj);
                keysToRemove.Add(kvp.Key);
                collectedCount++;
            }
        }

        foreach (var key in keysToRemove)
        {
            placedObjects.Remove(key);
        }

        if (collectedCount > 0)
        {
            Debug.Log($"Cキーで{collectedCount}個のオブジェクトを回収しました");
        }
        else
        {
            Debug.Log("回収可能なオブジェクトがありません");
        }

        UpdateUI();
    }

    private bool IsNearPlayer(Vector2 gridPos)
    {
        if (playerTransform == null) return false;

        Collider2D playerCollider = playerTransform.GetComponent<Collider2D>();
        if (playerCollider == null) return false;

        Vector2 center = ToGridPosition(playerCollider.bounds.center);

        float dx = Mathf.Abs(gridPos.x - center.x);
        float dy = Mathf.Abs(gridPos.y - center.y);

        return dx <= noPlacementWidth * 0.5f && dy <= noPlacementHeight * 0.5f;
    }

    private void OnDrawGizmosSelected()
    {
        if (playerTransform == null) return;

        Collider2D playerCollider = playerTransform.GetComponent<Collider2D>();
        if (playerCollider == null) return;

        Vector2 center = ToGridPosition(playerCollider.bounds.center);

        Gizmos.color = new Color(1f, 0f, 0f, 0.25f);
        Gizmos.DrawCube(center, new Vector3(noPlacementWidth, noPlacementHeight, 0f));
    }

    private Vector2 ToGridPosition(Vector3 worldPos)
    {
        return new Vector2(Mathf.Floor(worldPos.x) + 0.5f, Mathf.Floor(worldPos.y) + 0.5f);
    }

    private int CountDeletableObjects()
    {
        int count = 0;
        foreach (var entry in placedObjects.Values)
        {
            if (entry.canDelete)
                count++;
        }
        return count;
    }

    private void UpdateUI()
    {
        if (remainingBlocksText != null)
            remainingBlocksText.text = $"BLOCK:{maxBlocks - CurrentUsedBlockCount}";

        if (totalPlacedText != null)
            totalPlacedText.text = $"累積配置数： {totalPlacedCount}";
    }

    public int CurrentUsedBlockCount => CountDeletableObjects();

    public void AddBlockCapacity(int amount)
    {
        maxBlocks += amount;
        Debug.Log($"MaxBlocksが {amount} 増加されました。現在の上限: {maxBlocks}");
        UpdateUI();
    }

    public void SubtractBlockCapacity(int amount)
    {
        maxBlocks = Mathf.Max(0, maxBlocks - amount);
        Debug.Log($"MaxBlocksが {amount} 減少されました。現在の上限: {maxBlocks}");
        UpdateUI();
    }

    public void SetPlacedObjectsColor(Color color)
    {
        foreach (var kvp in placedObjects)
        {
            var (obj, _, _, _) = kvp.Value;

            SpriteRenderer renderer = obj.GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                renderer.color = color;
            }
        }
    }
}