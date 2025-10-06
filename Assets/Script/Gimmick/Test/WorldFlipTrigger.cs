using UnityEngine;

public class WorldFlipTrigger : MonoBehaviour
{
    public float flipCooldown = 2f; // クールタイム(秒)

    private float lastFlipTime = -Mathf.Infinity;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // プレイヤータグ判定
        {
            float currentTime = Time.time;
            if (currentTime - lastFlipTime >= flipCooldown)
            {
                WorldFlipManager.Instance?.FlipWorld();
                lastFlipTime = currentTime;
            }
            else
            {
                Debug.Log("反転はクールタイム中です");
            }
        }
    }
}
