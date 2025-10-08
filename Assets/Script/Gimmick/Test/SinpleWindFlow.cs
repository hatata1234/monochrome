using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SimpleWindFlow : MonoBehaviour
{
    [Header("���̐ݒ�")]
    public GameObject windPrefab;                 // ���̃Z��Prefab�i�G�t�F�N�g���j
    public float flowInterval = 0.5f;             // ���������Ԋu�i�b�j
    public Vector2 startWorldPos = Vector2.zero;  // ���̊J�n�n�_�i���[���h���W�j
    public int maxHeight = 10;                    // ���̍�������i�O���b�h��Y���W�j

    [Header("�u���b�N�p�̃^�C���}�b�v��ݒu��")]
    public Tilemap[] collisionTilemaps;           // �����̃u���b�N�pTilemap

    [Header("�J�n�x��")]
    public float startDelay = 2f;                 // �Q�[���J�n���畗���o������܂ł̒x���b��

    private Dictionary<Vector2Int, GameObject> windCells = new Dictionary<Vector2Int, GameObject>();
    private float timer = 0f;

    private float delayTimer = 0f;
    private bool windStarted = false;

    private ObjectPlacer2D objectPlacer;

    void Start()
    {
        objectPlacer = FindObjectOfType<ObjectPlacer2D>();

        // �������Z���̐����� startDelay �o�ߌ�ɍs��
    }

    void Update()
    {
        // �܂������J�n���Ă��Ȃ� �� �x����
        if (!windStarted)
        {
            delayTimer += Time.deltaTime;
            if (delayTimer >= startDelay)
            {
                Vector2Int startGridPos = WorldToGridPos(startWorldPos);
                if (!IsBlocked(startGridPos))
                {
                    SpawnWindCell(startGridPos);
                }

                windStarted = true;
                timer = 0f;
            }
            return;
        }

        // �ʏ�̕��̗��ꏈ��
        timer += Time.deltaTime;
        if (timer >= flowInterval)
        {
            FlowWind();
            timer = 0f;
        }
    }

    void FlowWind()
    {
        Vector2Int startGridPos = WorldToGridPos(startWorldPos);

        // �n�_���u���b�N����Ă�����S�č폜
        if (IsBlocked(startGridPos))
        {
            ClearAllWind();
            return;
        }

        // 1. �n�_���瓞�B�\�ȕ��Z����T���iBFS�j
        HashSet<Vector2Int> connectedWindCells = new HashSet<Vector2Int>();
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        queue.Enqueue(startGridPos);
        connectedWindCells.Add(startGridPos);

        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();

            foreach (var dir in GetFlowDirectionsPriority())
            {
                Vector2Int neighbor = current + dir;

                if (IsBlocked(neighbor))
                    continue;

                if (windCells.ContainsKey(neighbor) && !connectedWindCells.Contains(neighbor))
                {
                    connectedWindCells.Add(neighbor);
                    queue.Enqueue(neighbor);
                }
            }
        }

        // 2. �Ǘ��������Z�����폜
        List<Vector2Int> disconnectedCells = new List<Vector2Int>();
        foreach (var pos in windCells.Keys)
        {
            if (!connectedWindCells.Contains(pos))
            {
                disconnectedCells.Add(pos);
            }
        }
        foreach (var pos in disconnectedCells)
        {
            Destroy(windCells[pos]);
            windCells.Remove(pos);
        }

        // 3. ������ւ̊g�U�i������������j
        List<Vector2Int> currentPositions = new List<Vector2Int>(connectedWindCells);

        foreach (var pos in currentPositions)
        {
            Vector2Int upPos = pos + Vector2Int.up;

            if (upPos.y > maxHeight)
                continue;

            if (!IsBlocked(upPos) && !windCells.ContainsKey(upPos))
            {
                SpawnWindCell(upPos);
            }
        }
    }

    // ��������͏�����̂�
    List<Vector2Int> GetFlowDirectionsPriority()
    {
        return new List<Vector2Int> { Vector2Int.up };
    }

    // �u���b�N����
    bool IsBlocked(Vector2Int gridPos)
    {
        if (objectPlacer != null && objectPlacer.IsBlockAtGrid(ToVector2(gridPos)))
            return true;

        if (collisionTilemaps != null && collisionTilemaps.Length > 0 && IsBlockedByTilemaps(gridPos))
            return true;

        return false;
    }

    bool IsBlockedByTilemaps(Vector2Int gridPos)
    {
        Vector3Int tilePos = new Vector3Int(gridPos.x, gridPos.y, 0);
        foreach (var tilemap in collisionTilemaps)
        {
            if (tilemap == null) continue;

            TileBase tile = tilemap.GetTile(tilePos);
            if (tile != null)
                return true;
        }
        return false;
    }

    // ���Z���𐶐�
    void SpawnWindCell(Vector2Int gridPos)
    {
        Vector3 worldPos = ToWorldPosition(gridPos);
        GameObject windObj = Instantiate(windPrefab, worldPos, Quaternion.identity, transform);
        windCells.Add(gridPos, windObj);
    }

    // �S�Ă̕��Z�����폜
    void ClearAllWind()
    {
        foreach (var wind in windCells.Values)
        {
            Destroy(wind);
        }
        windCells.Clear();
    }

    // ���W�ϊ��F���[���h���O���b�h
    Vector2Int WorldToGridPos(Vector2 worldPos)
    {
        return new Vector2Int(Mathf.FloorToInt(worldPos.x), Mathf.FloorToInt(worldPos.y));
    }

    // ���W�ϊ��F�O���b�h�����[���h�i���S�j
    Vector3 ToWorldPosition(Vector2Int gridPos)
    {
        return new Vector3(gridPos.x + 0.5f, gridPos.y + 0.5f, 0f);
    }

    // �O���b�h��Vector2�iObjectPlacer2D�p�j
    Vector2 ToVector2(Vector2Int v)
    {
        return new Vector2(v.x + 0.5f, v.y + 0.5f);
    }
}
