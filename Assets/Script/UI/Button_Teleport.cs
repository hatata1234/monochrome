using UnityEngine;
using UnityEngine.UI;

public class Button_Teleport : MonoBehaviour
{
    [Header("�e���|�[�g���������Ώہi��FPlayer�j")]
    public Transform target;

    [Header("�e���|�[�g��̈ʒu")]
    public Transform teleportDestination;

    void Start()
    {
        // �{�^���R���|�[�l���g���擾���ăN���b�N�C�x���g��o�^
        GetComponent<Button>().onClick.AddListener(Teleport);
    }

    void Teleport()
    {
        if (target != null && teleportDestination != null)
        {
            target.position = teleportDestination.position;
        }
        else
        {
            Debug.LogWarning("�^�[�Q�b�g�܂��̓e���|�[�g�悪�ݒ肳��Ă��܂���I");
        }
    }
}
