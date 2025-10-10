using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableBlock : MonoBehaviour
{

    [Header("設定")]
    [SerializeField] private float breakTime = 2f;   // 乗ってから壊れるまでの時間
    [SerializeField] private float respawnTime = 3f; // 再出現までの時間
    [SerializeField] private float blinkInterval = 0.2f; // 点滅の間隔（秒）

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
            isBreaking = true;
            StartCoroutine(BreakSequence());
        }
    }

    private IEnumerator BreakSequence()
    {
        float elapsed = 0f;
        bool visible = true;

        // 壊れるまでの間、点滅を繰り返す
        while (elapsed < breakTime)
        {
            visible = !visible;
            sr.color = new Color(1f, 1f, 1f, visible ? 1f : 0.3f); // α値を変えて点滅
            yield return new WaitForSeconds(blinkInterval);
            elapsed += blinkInterval;
        }

        // 壊れる
        col.enabled = false;
        sr.enabled = false;

        // 復活を待つ
        yield return new WaitForSeconds(respawnTime);

        // 復活
        col.enabled = true;
        sr.enabled = true;
        sr.color = new Color(1f, 1f, 1f, 1f);
        isBreaking = false;
    }
}

