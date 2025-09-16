using UnityEngine;

public class ColliderChecker : MonoBehaviour
{
    public string targetTag = "Player"; // チェックするタグ
    public bool isTouching = false;     // 今触れているかどうか

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