using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // �� Text ���g�����߂ɕK�v

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
    public Text remainingBlocksText; // �� TMP ���� Text �ɕύX

    private Dictionary<Vector2, (GameObject obj, bool canDelete, bool isInitialObject)> placedObjects
        = new Dictionary<Vector2, (GameObject, bool, bool)>();

    private enum Mode { None, Place, Delete }
    private Mode currentMode = Mode.None;

    private Vector2? lastProcessedGridPos = null;

    public int MaxBlocks => maxBlocks;

    [Header("�v���C���[���͂̔z�u����")]
    public Transform playerTransform;
    public int playerNoPlacementRadius = 1;

    [Header("���ʎq")]
    public string placerId = "Placer_1";

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
                    placedObjects.Add(gridPos, (obj, allowDeletingInitialObjects, true));
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
            placedObjects.Add(gridPos, (obj, allowDeletingInitialObjects, true));
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

            Debug.Log($"�I�u�W�F�N�g���폜���܂��� at {gridPos}");
            UpdateUI();
        }
        else
        {
            Debug.Log("���̃I�u�W�F�N�g�͍폜�ł��܂���");
        }
    }

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

            placedObjects.Add(gridPos, (placedObject, true, false));

            Debug.Log($"�I�u�W�F�N�g��z�u���܂��� at {gridPos}");
            UpdateUI();
        }
        else
        {
            Debug.Log("�z�u�ł���ꏊ�ł͂���܂���i���M�~�b�N�̗̈�H�j");
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
            remainingBlocksText.text = $"�z�u�\�u���b�N�F {maxBlocks - CurrentUsedBlockCount}";
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
            Debug.Log($"C�L�[��{collectedCount}�̃I�u�W�F�N�g��������܂���");
        }
        else
        {
            Debug.Log("����\�ȃI�u�W�F�N�g������܂���");
        }

        UpdateUI();
    }

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
}