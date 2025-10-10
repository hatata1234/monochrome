using UnityEngine;

public class Bullet : MonoBehaviour
{
    public LayerMask destroyOnLayers;  // 消える対象のレイヤー

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // collision.gameObjectのレイヤーがdestroyOnLayersに含まれているか判定
        if (((1 << collision.gameObject.layer) & destroyOnLayers) != 0)
        {
            Destroy(gameObject);
        }
    }
}
