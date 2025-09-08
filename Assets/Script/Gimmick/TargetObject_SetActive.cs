using UnityEngine;

public class TargetObject_SetActive : MonoBehaviour
{
    public GameObject game_object; // �ڐG���ɕ\���ؑւ���I�u�W�F�N�g
    public bool isActive = true;                   // ������ԁitrue: �\��, false: ��\���j

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Code"))
        {
            game_object.SetActive(!isActive);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Code"))
        {
            game_object.SetActive(isActive);
        }
    }
}