using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectPlacer2D : MonoBehaviour
{
    [Header("�ݒ�")]
    public GameObject prefabToPlace;
    public LayerMask placementLayerMask;
    public int maxBlocks = 10;

    [Header("�I�u�W�F�N�g�ݒ�")]
    public bool setColliderAsTrigger = false;

    [Header("���[���ݒ�")]
    public bool requirePowerPathForPlacement = true;

    [Header("�����I�u�W�F�N�g�̍폜")]
    public bool allowDeletingInitialObjects = true;

    [Header("UI�Q��")]
    public Text remainingBlocksText;
    public Text totalPlacedText;

    [Header("�v���C���[���͂̔z�u����")]
    public Transform playerTransform;
    public int noPlacementWidth = 3;
    public int noPlacementHeight = 2;

    [Header("���ʎq")]
    public string placerId = "Placer_1";

    [Header("�N�[���_�E���ݒ�")]
    public float collectCooldown = 1f;
    private float lastCollectTime = -Mathf.Infinity;

    [Header("�����������")]
    public float autoCollectBlockTimeout = 10f;

    private int totalPlacedCount = 0;
    public int TotalPlacedCount => totalPlacedCount;

    private enum Mode { None, Place, Delete }
    private Mode currentMode = Mode.None;

    private Vector2? lastProcessedGridPos = null;

    // ��ԏ��t���̃f�[�^�\���ɕύX
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

        Debug.LogWarning($"Placer ID '{id}' ��������܂���ł���");
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
                    Debug.LogWarning($"�����ʒu�Ɋ��ɃI�u�W�F�N�g������܂�: {gridPos}");
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
            Debug.Log($"AutoCreateGimmick�œo�^: {gridPos}");
        }
        else
        {
            Debug.LogWarning($"�o�^���s�F���ɑ��݂��Ă��܂�: {gridPos}");
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
                Debug.Log("����̓N�[���^�C�����ł�");
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
        // ���N���b�N����͏�ɉ\�icanDelete�͎g��Ȃ��j
        Destroy(obj);
        placedObjects.Remove(gridPos);

        Debug.Log($"�I�u�W�F�N�g���폜���܂��� at {gridPos}");
        UpdateUI();
    }

    //private void TryPlaceAt(Vector2 gridPos, Vector3 mouseWorldPos)
    //{
    //    if (IsNearPlayer(gridPos))
    //    {
    //        Debug.Log("�v���C���[�̋߂��ɂ͔z�u�ł��܂���");
    //        return;
    //    }

    //    int currentPlaceableCount = CountDeletableObjects();
    //    if (currentPlaceableCount >= maxBlocks)
    //    {
    //        Debug.Log("����ȏ�u���b�N��z�u�ł��܂���i����j");
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
    //        Debug.Log($"�I�u�W�F�N�g��z�u���܂��� at {gridPos}�i�ݐ�: {totalPlacedCount}�j");

    //        UpdateUI();
    //    }
    //    else
    //    {
    //        Debug.Log("�z�u�ł���ꏊ�ł͂���܂���i���M�~�b�N�̗̈�H�j");
    //    }
    //}
    private void TryPlaceAt(Vector2 gridPos, Vector3 mouseWorldPos)
    {
        if (IsNearPlayer(gridPos))
        {
            Debug.Log("�v���C���[�̋߂��ɂ͔z�u�ł��܂���");
            return;
        }

        int currentPlaceableCount = CountDeletableObjects();
        if (currentPlaceableCount >= maxBlocks)
        {
            Debug.Log("����ȏ�u���b�N��z�u�ł��܂���i����j");
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

            // �O���b�h�\�����͏�ɊD�F�������ɂ���
            CreateMode createMode = FindObjectOfType<CreateMode>();
            if (createMode != null && createMode.IsActive)
            {
                SpriteRenderer sr = placedObject.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    sr.color = new Color(0.5f, 0.5f, 0.5f, 0.5f); // �D�F������
                }
            }
            else
            {
                // �O���b�h��\�����͐��E�F�ɍ��킹��
                bool isBlackWorld = WorldFlipManager.Instance != null && WorldFlipManager.Instance.IsBlackWorld;
                SpriteRenderer sr = placedObject.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    sr.color = isBlackWorld ? Color.white : Color.black;
                }
            }

            totalPlacedCount++;
            Debug.Log($"�I�u�W�F�N�g��z�u���܂��� at {gridPos}�i�ݐ�: {totalPlacedCount}�j");

            UpdateUI();
        }
        else
        {
            if (hit.collider != null)
            {
                Debug.Log($"�z�u�ł��܂���: {hit.collider.gameObject.name} �Ƀq�b�g���܂���");
            }
            else
            {
                Debug.Log("�z�u�ł���ꏊ�ł͂���܂���i���ɂ��q�b�g���Ă��Ȃ��j");
            }
        }
    }

    private void OnEnable()
    {
        if (WorldFlipManager.Instance != null)
            WorldFlipManager.Instance.OnWorldFlipped += HandleWorldFlip;
    }

    private void OnDisable()
    {
        if (WorldFlipManager.Instance != null)
            WorldFlipManager.Instance.OnWorldFlipped -= HandleWorldFlip;
    }

    private void HandleWorldFlip(bool isBlackWorld)
    {
        Color color = isBlackWorld ? Color.white : Color.black;
        SetPlacedObjectsColor(color);
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

            // isInitial �� ����s�A10�b�ȏ� �� ����s��
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
            Debug.Log($"C�L�[��{collectedCount}�̃I�u�W�F�N�g��������܂���");
        }
        else
        {
            Debug.Log("����\�ȃI�u�W�F�N�g������܂���");
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
            totalPlacedText.text = $"�ݐϔz�u���F {totalPlacedCount}";
    }

    public int CurrentUsedBlockCount => CountDeletableObjects();

    public void AddBlockCapacity(int amount)
    {
        maxBlocks += amount;
        Debug.Log($"MaxBlocks�� {amount} ��������܂����B���݂̏��: {maxBlocks}");
        UpdateUI();
    }

    public void SubtractBlockCapacity(int amount)
    {
        maxBlocks = Mathf.Max(0, maxBlocks - amount);
        Debug.Log($"MaxBlocks�� {amount} ��������܂����B���݂̏��: {maxBlocks}");
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

    public bool IsBlockAtGrid(Vector2 gridPos)
    {
        return placedObjects.ContainsKey(gridPos);
    }
}