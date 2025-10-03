using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
    public float floatForce = 5f;           // 浮力の強さ
    public float damping = 0.5f;            // 水の抵抗
    public float floatHeight = 0.5f;        // 浮かぶ深さ
    public float jumpForce = 8f;            // 水中ジャンプの力
    public float waterExitTime = 0.3f;      // ジャンプ直後の水影響無視時間

    private Dictionary<Rigidbody2D, float> jumpIgnoreTimers = new Dictionary<Rigidbody2D, float>();

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        if (rb == null) return;

        Collider2D waterCollider = GetComponent<Collider2D>();
        if (waterCollider == null) return;

        float waterSurfaceY = transform.position.y + (waterCollider.bounds.size.y / 2f);
        float playerY = other.transform.position.y;

        float heightDiff = waterSurfaceY - playerY - floatHeight;

        bool isJumpingOut = jumpIgnoreTimers.ContainsKey(rb) && Time.time < jumpIgnoreTimers[rb];

        //// 水面近くでジャンプ
        //if (Input.GetKeyDown(KeyCode.Space) && heightDiff < 0.3f)
        //{
        //    rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        //    jumpIgnoreTimers[rb] = Time.time + waterExitTime;
        //}

        // 浮力処理（ジャンプ直後はスキップ）
        if (!isJumpingOut && playerY < waterSurfaceY)
        {
            // 浮力をかける
            float buoyancy = heightDiff * floatForce;
            rb.AddForce(new Vector2(0, buoyancy));

            // 水の抵抗
            rb.velocity *= (1f - damping * Time.deltaTime);
        }
    }
}