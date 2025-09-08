using UnityEngine;

public class TargetObject_SetActive : MonoBehaviour
{
    public GameObject objectToToggleOnGoalContact; // Goal�ڐG���ɕ\���ؑւ���I�u�W�F�N�g�i�g���Ȃ�j
    public GameObject Damege_Floor;                // ��\��/�ĕ\������I�u�W�F�N�g
    public bool isActive = true;                   // ������ԁitrue: �\��, false: ��\���j

    private void Start()
    {
        // ������Ԃɐݒ�
        if (Damege_Floor != null)
        {
            Damege_Floor.SetActive(isActive);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Code"))
        {
            if (Damege_Floor != null)
            {
                Damege_Floor.SetActive(false);
                Debug.Log("Damege_Floor ��\��");
            }

            if (objectToToggleOnGoalContact != null)
            {
                objectToToggleOnGoalContact.SetActive(!isActive);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Code"))
        {
            if (Damege_Floor != null)
            {
                Damege_Floor.SetActive(true);
                Debug.Log("Damege_Floor �\��");
            }

            if (objectToToggleOnGoalContact != null)
            {
                objectToToggleOnGoalContact.SetActive(isActive);
            }
        }
    }
}