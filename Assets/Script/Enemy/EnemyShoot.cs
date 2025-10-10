using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    public GameObject bulletPrefab;          // 弾のPrefab
    public Transform shootPoint;             // 弾を発射する位置
    public float shootInterval = 2f;         // 発射間隔
    public float bulletSpeed = 5f;           // 弾の速さ
    public float detectionRadius = 5f;       // プレイヤー検知範囲

    public string playerTag = "Player";      // プレイヤーのタグ
    private GameObject player;
    private float shootTimer;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag(playerTag);
        shootTimer = shootInterval;
    }

    void Update()
    {
        if (player == null) return;

        // プレイヤーが範囲内かチェック
        float distance = Vector2.Distance(transform.position, player.transform.position);
        if (distance <= detectionRadius)
        {
            shootTimer -= Time.deltaTime;
            if (shootTimer <= 0f)
            {
                Shoot();
                shootTimer = shootInterval;
            }
        }
        else
        {
            shootTimer = shootInterval; // 範囲外ならリセットしておく
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
        Vector2 direction = (player.transform.position - shootPoint.position).normalized;

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction * bulletSpeed;
        }
    }

    // デバッグ用に検知範囲を表示
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
