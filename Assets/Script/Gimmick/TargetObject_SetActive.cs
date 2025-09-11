using UnityEngine;

public class TargetObject_SetActive : MonoBehaviour
{
    public GameObject[] gameObjects; // �ΏۃI�u�W�F�N�g
    private bool[] originalStates;   // �e�I�u�W�F�N�g�̏�����Ԃ��L�^

    [Tooltip("���̃^�O�����I�u�W�F�N�g���g���K�[�ɓ������Ƃ��ɏ�������܂�")]
    public string targetTag = "Code"; // ��������Ɏg���^�O���C���X�y�N�^�[�Őݒ�\

    private void Start()
    {
        // ������Ԃ̕ۑ�
        originalStates = new bool[gameObjects.Length];
        for (int i = 0; i < gameObjects.Length; i++)
        {
            if (gameObjects[i] != null)
                originalStates[i] = gameObjects[i].activeSelf;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(targetTag))
        {
            for (int i = 0; i < gameObjects.Length; i++)
            {
                if (gameObjects[i] != null)
                {
                    // ��Ԃ𔽓]
                    gameObjects[i].SetActive(!gameObjects[i].activeSelf);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(targetTag))
        {
            for (int i = 0; i < gameObjects.Length; i++)
            {
                if (gameObjects[i] != null)
                {
                    // ���̏�Ԃɖ߂�
                    gameObjects[i].SetActive(originalStates[i]);
                }
            }
        }
    }
}