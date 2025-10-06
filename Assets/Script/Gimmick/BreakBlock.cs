using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakBlock : MonoBehaviour
{
    [Header("�ݒ�")]
    [SerializeField] private float breakTime = 2f;   // ����Ă������܂ł̎���
    [SerializeField] private float respawnTime = 3f; // �ďo���܂ł̎���

    private Collider2D col;
    private SpriteRenderer sr;
    private bool isBreaking = false; // ����J�E���g�����ǂ���

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isBreaking && collision.gameObject.CompareTag("Player"))
        {
            // �v���C���[������������J�E���g�J�n
            isBreaking = true;
            Invoke(nameof(Break), breakTime);
        }
    }

    private void Break()
    {
        // �u���b�N������
        col.enabled = false;
        sr.enabled = false;

        // respawnTime �b��ɕ���
        Invoke(nameof(Respawn), respawnTime);
    }

    private void Respawn()
    {
        // �u���b�N�𕜊�
        col.enabled = true;
        sr.enabled = true;
        isBreaking = false; // �ēx����悤�Ƀ��Z�b�g
    }
}
