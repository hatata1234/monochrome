using UnityEngine;

public class TeleportBlock : MonoBehaviour
{
    [Header("�e���|�[�g���Transform")]
    public Transform whiteWorldTeleportTarget;  // �����E�̃e���|�[�g��
    public Transform blackWorldTeleportTarget;  // �����E�̃e���|�[�g��

    private void OnTriggerEnter2D(Collider2D other)
    {
        // �v���C���[���ǂ�������i�^�O�Ŕ��肷���j
        if (other.CompareTag("Player"))
        {
            Vector3 targetPos;

            // ���݂̐��E�ɉ����ăe���|�[�g������߂�
            if (WorldFlipManager.Instance.IsBlackWorld)
            {
                targetPos = blackWorldTeleportTarget.position;
            }
            else
            {
                targetPos = whiteWorldTeleportTarget.position;
            }

            // �v���C���[���e���|�[�g��ֈړ�
            other.transform.position = targetPos;

            Debug.Log($"�e���|�[�g�I ���݂̐��E: {(WorldFlipManager.Instance.IsBlackWorld ? "��" : "��")}");
        }
    }
}