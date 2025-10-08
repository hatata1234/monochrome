using UnityEngine;

public class TeleportBlock : MonoBehaviour
{
    [Header("テレポート先のTransform")]
    public Transform TeleportTarget;  // テレポート先

    private void OnTriggerEnter2D(Collider2D other)
    {
        // プレイヤーかどうか判定（タグで判定する例）
        if (other.CompareTag("Player"))
        {
            Vector3 targetPos;

            targetPos = TeleportTarget.position;
        
            // プレイヤーをテレポート先へ移動
            other.transform.position = targetPos;

            Debug.Log("テレポート！");
        }
    }
}