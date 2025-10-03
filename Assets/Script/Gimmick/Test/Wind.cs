using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
    public float floatForce = 5f;           // ���͂̋���
    public float damping = 0.5f;            // ���̒�R
    public float floatHeight = 0.5f;        // �����Ԑ[��
    public float jumpForce = 8f;            // �����W�����v�̗�
    public float waterExitTime = 0.3f;      // �W�����v����̐��e����������

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

        //// ���ʋ߂��ŃW�����v
        //if (Input.GetKeyDown(KeyCode.Space) && heightDiff < 0.3f)
        //{
        //    rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        //    jumpIgnoreTimers[rb] = Time.time + waterExitTime;
        //}

        // ���͏����i�W�����v����̓X�L�b�v�j
        if (!isJumpingOut && playerY < waterSurfaceY)
        {
            // ���͂�������
            float buoyancy = heightDiff * floatForce;
            rb.AddForce(new Vector2(0, buoyancy));

            // ���̒�R
            rb.velocity *= (1f - damping * Time.deltaTime);
        }
    }
}