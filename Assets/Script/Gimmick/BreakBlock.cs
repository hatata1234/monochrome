using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakBlock : MonoBehaviour
{
    [Header("設定")]
    [SerializeField] private float breakTime = 2f;   // 乗ってから壊れるまでの時間
    [SerializeField] private float respawnTime = 3f; // 再出現までの時間

    private Collider2D col;
    private SpriteRenderer sr;
    private bool isBreaking = false; // 壊れるカウント中かどうか

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isBreaking && collision.gameObject.CompareTag("Player"))
        {
            // プレイヤーが乗ったら壊れるカウント開始
            isBreaking = true;
            Invoke(nameof(Break), breakTime);
        }
    }

    private void Break()
    {
        // ブロックを消す
        col.enabled = false;
        sr.enabled = false;

        // respawnTime 秒後に復活
        Invoke(nameof(Respawn), respawnTime);
    }

    private void Respawn()
    {
        // ブロックを復活
        col.enabled = true;
        sr.enabled = true;
        isBreaking = false; // 再度壊れるようにリセット
    }
}
