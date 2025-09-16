using UnityEngine;

public class ColliderChecker : MonoBehaviour
{
    public string targetTag = "Player"; // �`�F�b�N����^�O
    public bool isTouching = false;     // ���G��Ă��邩�ǂ���

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag(targetTag))
        {
            isTouching = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(targetTag))
        {
            isTouching = false;
        }
    }
}