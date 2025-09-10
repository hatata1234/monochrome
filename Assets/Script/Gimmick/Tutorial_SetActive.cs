using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_SetActive : MonoBehaviour
{
    public GameObject[] gameObjects; // �\������UI�Ȃ�
    private bool[] originalStates;
    public float reactCooldown = 2f; // �Ĕ����܂ł̑҂�����
    private bool canReact = true;

    private void Start()
    {
        originalStates = new bool[gameObjects.Length];
        for (int i = 0; i < gameObjects.Length; i++)
        {
            if (gameObjects[i] != null)
                originalStates[i] = gameObjects[i].activeSelf;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canReact && collision.CompareTag("Player"))
        {
            ToggleUI(true); // UI��\��
            canReact = false;
            StartCoroutine(ReactCooldown());
        }
    }

    // UI���J����������肷��
    public void ToggleUI(bool isOpen)
    {
        for (int i = 0; i < gameObjects.Length; i++)
        {
            if (gameObjects[i] != null)
                gameObjects[i].SetActive(isOpen);
        }

        // UI���J���Ă�Ԃ͎��Ԓ�~
        Time.timeScale = isOpen ? 0f : 1f;
    }

    // UI�̕���{�^������Ă΂��
    public void OnCloseButton()
    {
        ToggleUI(false);
    }

    // ��莞�Ԍo�܂ōĔ����ł��Ȃ��悤�ɂ���
    private IEnumerator ReactCooldown()
    {
        yield return new WaitForSecondsRealtime(reactCooldown); // TimeScale�̉e�����󂯂Ȃ�
        canReact = true;
    }
}
