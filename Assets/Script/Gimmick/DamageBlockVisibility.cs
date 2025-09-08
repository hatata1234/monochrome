using UnityEngine;

public class DamageBlockVisibility : MonoBehaviour
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

        if (other.CompareTag("code"))
        {
           
        }
    }
}