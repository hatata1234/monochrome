using UnityEngine;
using UnityEngine.EventSystems;

public class DamageBlockVisibility : MonoBehaviour
{
    //public float visibilityThreshold = 5f;  // この値以上で非表示にする
    public Transform targetPoint;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player playerScript = other.GetComponent<Player>();
            if (playerScript != null)
            {
                // playerScript.DieAndShowRetryUI();
                toTeleport();
            }
        }
    }
    public void toTeleport()
    {
        GameObject player = GameObject.FindWithTag("Player");

        if (player != null)
        {
            if (targetPoint != null)
            {
                player.transform.position = targetPoint.position;
            }
        }
        //フォーカス解除
        EventSystem.current.SetSelectedGameObject(null);
    }

}