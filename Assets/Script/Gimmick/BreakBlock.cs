using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableBlock : MonoBehaviour
{

    [Header("�ݒ�")]
    [SerializeField] private float breakTime = 2f;   // ����Ă������܂ł̎���
    [SerializeField] private float respawnTime = 3f; // �ďo���܂ł̎���
    [SerializeField] private float blinkInterval = 0.2f; // �_�ł̊Ԋu�i�b�j

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
            isBreaking = true;
            StartCoroutine(BreakSequence());
        }
    }

    private IEnumerator BreakSequence()
    {
        float elapsed = 0f;
        bool visible = true;

        // ����܂ł̊ԁA�_�ł��J��Ԃ�
        while (elapsed < breakTime)
        {
            visible = !visible;
            sr.color = new Color(1f, 1f, 1f, visible ? 1f : 0.3f); // ���l��ς��ē_��
            yield return new WaitForSeconds(blinkInterval);
            elapsed += blinkInterval;
        }

        // ����
        col.enabled = false;
        sr.enabled = false;

        // ������҂�
        yield return new WaitForSeconds(respawnTime);

        // ����
        col.enabled = true;
        sr.enabled = true;
        sr.color = new Color(1f, 1f, 1f, 1f);
        isBreaking = false;
    }
}

