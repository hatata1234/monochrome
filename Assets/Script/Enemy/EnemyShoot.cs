using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    public GameObject bulletPrefab;          // �e��Prefab
    public Transform shootPoint;             // �e�𔭎˂���ʒu
    public float shootInterval = 2f;         // ���ˊԊu
    public float bulletSpeed = 5f;           // �e�̑���
    public float detectionRadius = 5f;       // �v���C���[���m�͈�

    public string playerTag = "Player";      // �v���C���[�̃^�O
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

        // �v���C���[���͈͓����`�F�b�N
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
            shootTimer = shootInterval; // �͈͊O�Ȃ烊�Z�b�g���Ă���
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

    // �f�o�b�O�p�Ɍ��m�͈͂�\��
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
