using UnityEngine;

public class TeleportBlock : MonoBehaviour
{
    [Header("テレポート先のTransform")]
    public Transform whiteWorldTeleportTarget;  // 白世界のテレポート先
    public Transform blackWorldTeleportTarget;  // 黒世界のテレポート先

    private void OnTriggerEnter2D(Collider2D other)
    {
        // プレイヤーかどうか判定（タグで判定する例）
        if (other.CompareTag("Player"))
        {
            Vector3 targetPos;

            // 現在の世界に応じてテレポート先を決める
            if (WorldFlipManager.Instance.IsBlackWorld)
            {
                targetPos = blackWorldTeleportTarget.position;
            }
            else
            {
                targetPos = whiteWorldTeleportTarget.position;
            }

            // プレイヤーをテレポート先へ移動
            other.transform.position = targetPos;

            Debug.Log($"テレポート！ 現在の世界: {(WorldFlipManager.Instance.IsBlackWorld ? "黒" : "白")}");
        }
    }
}