using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // ← Text を使うために必要

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
    public Text remainingBlocksText; // ← TMP から Text に変更

    private Dictionary<Vector2, (GameObject obj, bool canDelete, bool isInitialObject)> placedObjects
        = new Dictionary<Vector2, (GameObject, bool, bool)>();

    private enum Mode { None, Place, Delete }
    private Mode currentMode = Mode.None;

    private Vector2? lastProcessedGridPos = null;

    public int MaxBlocks => maxBlocks;

    [Header("プレイヤー周囲の配置制限")]
    public Transform playerTransform;
    public int playerNoPlacementRadius = 1;

    [Header("識別子")]
    public string placerId = "Placer_1";

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
                    placedObjects.Add(gridPos, (obj, allowDeletingInitialObjects, true));
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
            placedObjects.Add(gridPos, (obj, allowDeletingInitialObjects, true));
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
        if (Input.GetKeyDown(KeyCode.C))
        {
            CollectAllPlacedObjects();
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
        var (obj, canDelete, _) = placedObjects[gridPos];
        if (canDelete)
        {
            Destroy(obj);
            placedObjects.Remove(gridPos);

            Debug.Log($"オブジェクトを削除しました at {gridPos}");
            UpdateUI();
        }
        else
        {
            Debug.Log("このオブジェクトは削除できません");
        }
    }

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

            placedObjects.Add(gridPos, (placedObject, true, false));

            Debug.Log($"オブジェクトを配置しました at {gridPos}");
            UpdateUI();
        }
        else
        {
            Debug.Log("配置できる場所ではありません（他ギミックの領域？）");
        }
    }

    private bool IsNearPlayer(Vector2 gridPos)
    {
        if (playerTransform == null) return false;

        Vector2 playerGrid = ToGridPosition(playerTransform.position);

        float dx = Mathf.Abs(gridPos.x - playerGrid.x);
        float dy = Mathf.Abs(gridPos.y - playerGrid.y);

        return dx <= playerNoPlacementRadius && dy <= playerNoPlacementRadius;
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

    private Vector2 ToGridPosition(Vector3 worldPos)
    {
        return new Vector2(Mathf.Floor(worldPos.x) + 0.5f, Mathf.Floor(worldPos.y) + 0.5f);
    }

    private void UpdateUI()
    {
        if (remainingBlocksText != null)
            remainingBlocksText.text = $"配置可能ブロック： {maxBlocks - CurrentUsedBlockCount}";
    }

    public int CurrentUsedBlockCount => CountDeletableObjects();

    private void CollectAllPlacedObjects()
    {
        List<Vector2> keysToRemove = new List<Vector2>();
        int collectedCount = 0;

        foreach (var kvp in placedObjects)
        {
            var (obj, canDelete, isInitial) = kvp.Value;
            if (!isInitial)
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
}