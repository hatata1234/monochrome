using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolObject : MonoBehaviour
{
    [Header("パトロールポイント（座標）")]
    public Vector2 pointA;
    public Vector2 pointB;

    [Header("移動速度")]
    public float speed = 2f;

    private Vector2 currentTarget;
    private ObjectPlacer2D placer;

    private int bounceCount = 0; // 跳ね返り回数カウント

    void Start()
    {
        currentTarget = pointB;
        placer = ObjectPlacer2D.FindPlacerById("Placer_1");

        // 初期位置をpointAにセット
        transform.position = pointA;

        // 初期の向きはpointB方向に合わせて設定
        SetFacingDirection(currentTarget - (Vector2)transform.position);
    }

    void Update()
    {
        if (placer == null) return;

        Vector2 nextGridPos = GetNextGridPos();

        // 進行方向にブロックがあったら方向反転
        if (placer.IsBlockAtGrid(nextGridPos))
        {
            Debug.Log($"ブロック検出: {nextGridPos}。方向を反転します。");

            currentTarget = (currentTarget == pointB) ? pointA : pointB;
            SetFacingDirection(currentTarget - (Vector2)transform.position);

            bounceCount++;  // 跳ね返り回数を増やす

            if (bounceCount >= 2)
            {
                Debug.Log("2回跳ね返ったためオブジェクトを非表示にします。");
                gameObject.SetActive(false); // オブジェクト非表示
                return; // 以降処理を中断
            }
        }

        // 目的地へ移動
        transform.position = Vector3.MoveTowards(transform.position, currentTarget, speed * Time.deltaTime);

        // 目的地に到達したらターゲットを切り替える（これが無いと往復しない）
        if ((Vector2)transform.position == currentTarget)
        {
            currentTarget = (currentTarget == pointB) ? pointA : pointB;
            SetFacingDirection(currentTarget - (Vector2)transform.position);
        }
    }


    // 次に進むグリッド位置を計算（0.5刻みでグリッドに合わせる）
    Vector2 GetNextGridPos()
    {
        Vector2 direction = (currentTarget - (Vector2)transform.position).normalized;
        Vector2 nextPos = (Vector2)transform.position + direction * 0.5f;
        return new Vector2(Mathf.Floor(nextPos.x) + 0.5f, Mathf.Floor(nextPos.y) + 0.5f);
    }

    // 見た目の向きをローテーションで切り替え
    void SetFacingDirection(Vector2 direction)
    {
        if (direction.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);  // 右向き（デフォルト）
        }
        else if (direction.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0); // 左向き（Y軸180度回転で反転）
        }
    }
}
