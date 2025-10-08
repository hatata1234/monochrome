using UnityEngine;

public class TeleportBlock : MonoBehaviour
{
    [Header("�e���|�[�g���Transform")]
    public Transform TeleportTarget;  // �e���|�[�g��

    private void OnTriggerEnter2D(Collider2D other)
    {
        // �v���C���[���ǂ�������i�^�O�Ŕ��肷���j
        if (other.CompareTag("Player"))
        {
            Vector3 targetPos;

            targetPos = TeleportTarget.position;
        
            // �v���C���[���e���|�[�g��ֈړ�
            other.transform.position = targetPos;

            Debug.Log("�e���|�[�g�I");
        }
    }
}