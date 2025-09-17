using UnityEngine;
using UnityEngine.EventSystems;

public class DamageBlockVisibility2 : MonoBehaviour
{
    public float visibilityThreshold = 5f;  // ���̒l�ȏ�Ŕ�\���ɂ���

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player playerScript = other.GetComponent<Player>();
            if (playerScript != null)
            {
                playerScript.DieAndShowRetryUI();
            }
        }
    }
   

}