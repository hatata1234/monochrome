using UnityEngine;

public class Bullet : MonoBehaviour
{
    public LayerMask destroyOnLayers;  // ������Ώۂ̃��C���[

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // collision.gameObject�̃��C���[��destroyOnLayers�Ɋ܂܂�Ă��邩����
        if (((1 << collision.gameObject.layer) & destroyOnLayers) != 0)
        {
            Destroy(gameObject);
        }
    }
}
