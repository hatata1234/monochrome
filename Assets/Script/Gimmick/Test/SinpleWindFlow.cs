using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SimpleWindFlow : MonoBehaviour
{
    [Header("風の設定")]
    public GameObject windPrefab;                 // 風のセルPrefab（エフェクト等）
    public float flowInterval = 0.5f;             // 風が流れる間隔（秒）
    public Vector2 startWorldPos = Vector2.zero;  // 風の開始地点（ワールド座標）

    [Header("ブロック用のタイルマップや設置物")]
    public Tilemap collisionTilemap;              // ブロック用Tilemap（null可）

    private Dictionary<Vector2Int, GameObject> windCells = new Dictionary<Vector2Int, GameObject>();
    private float timer = 0f;

    private ObjectPlacer2D objectPlacer;

    void Start()
    {
        objectPlacer = FindObjectOfType<ObjectPlacer2D>();

        Vector2Int startGridPos = WorldToGridPos(startWorldPos);
        SpawnWindCell(startGridPos);
    }

    void Update()
    {
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

        // 始点がブロックされていたら全て削除
        if (IsBlocked(startGridPos))
        {
            ClearAllWind();
            return;
        }

        // 1. 始点から到達可能な風セルを探索（BFS）
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

        // 2. 孤立した風セルを削除
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

        // 3. 風の拡散（上方向のみ）
        List<Vector2Int> currentPositions = new List<Vector2Int>(connectedWindCells);

        foreach (var pos in currentPositions)
        {
            Vector2Int upPos = pos + Vector2Int.up;

            // 優先度1: 上に流れる
            if (!IsBlocked(upPos) && !windCells.ContainsKey(upPos))
            {
                SpawnWindCell(upPos);
            }

            // 横や下には流れない
        }
    }

    // 流れ方向は上方向のみ
    List<Vector2Int> GetFlowDirectionsPriority()
    {
        return new List<Vector2Int>
        {
            Vector2Int.up
        };
    }

    // ブロック判定
    bool IsBlocked(Vector2Int gridPos)
    {
        if (objectPlacer != null && objectPlacer.IsBlockAtGrid(ToVector2(gridPos)))
            return true;

        if (collisionTilemap != null && IsBlockedByTilemap(gridPos))
            return true;

        return false;
    }

    bool IsBlockedByTilemap(Vector2Int gridPos)
    {
        Vector3Int tilePos = new Vector3Int(gridPos.x, gridPos.y, 0);
        TileBase tile = collisionTilemap.GetTile(tilePos);
        return tile != null;
    }

    // 風セルを生成
    void SpawnWindCell(Vector2Int gridPos)
    {
        Vector3 worldPos = ToWorldPosition(gridPos);
        GameObject windObj = Instantiate(windPrefab, worldPos, Quaternion.identity, transform);
        windCells.Add(gridPos, windObj);
    }

    // 全ての風セルを削除
    void ClearAllWind()
    {
        foreach (var wind in windCells.Values)
        {
            Destroy(wind);
        }
        windCells.Clear();
    }

    // 座標変換：ワールド→グリッド
    Vector2Int WorldToGridPos(Vector2 worldPos)
    {
        return new Vector2Int(Mathf.FloorToInt(worldPos.x), Mathf.FloorToInt(worldPos.y));
    }

    // 座標変換：グリッド→ワールド（中心）
    Vector3 ToWorldPosition(Vector2Int gridPos)
    {
        return new Vector3(gridPos.x + 0.5f, gridPos.y + 0.5f, 0f);
    }

    // グリッド→Vector2（ObjectPlacer2D用）
    Vector2 ToVector2(Vector2Int v)
    {
        return new Vector2(v.x + 0.5f, v.y + 0.5f);
    }
}
