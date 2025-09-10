using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float movePower = 10f;
    public float jumpPower = 20f; // Rigidbody2DのGravity Scaleは5推奨

    private Rigidbody2D rb;
    private Animator anim;
    private int direction = 1;

    private bool isJumping = false;
    private bool alive = true;

    // 地面判定用（Raycast）
    private bool isGrounded = false;
    public Transform groundCheckPoint;    // 足元の判定ポイント
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    public Transform respawnPoint; // リスポーン地点
    private bool isRespawning = false;

    [Header("Retry UI")]
    public GameObject retryPanel;         // UI パネルをインスペクターで設定

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // --- ポーズ中は一切の入力処理をスキップ ---
        if (PauseManager.IsPaused) return;

        Restart();

        if (alive)
        {
            Die();
            Run();
            Jump();
        }

        // 地面に接地しているか判定（Raycastを使用）
        isGrounded = Physics2D.Raycast(groundCheckPoint.position, Vector2.down, groundCheckRadius, groundLayer);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        anim.SetBool("isJump", false);
    }

    void Run()
    {
        Vector3 moveVelocity = Vector3.zero;
        anim.SetBool("isRun", false);

        float h = Input.GetAxisRaw("Horizontal");

        if (h < 0)
        {
            direction = -1;
            moveVelocity = Vector3.left;
            transform.localScale = new Vector3(direction, 1, 1);
            if (!anim.GetBool("isJump"))
                anim.SetBool("isRun", true);
        }
        else if (h > 0)
        {
            direction = 1;
            moveVelocity = Vector3.right;
            transform.localScale = new Vector3(direction, 1, 1);
            if (!anim.GetBool("isJump") && isGrounded)
                anim.SetBool("isRun", true);
        }

        transform.position += moveVelocity * movePower * Time.deltaTime;
    }

    void Jump()
    {
        if ((Input.GetButtonDown("Jump") || Input.GetAxisRaw("Vertical") > 0) && isGrounded)
        {
            isJumping = true;
            anim.SetBool("isJump", true);
        }

        if (!isJumping)
            return;

        rb.velocity = Vector2.zero;
        rb.AddForce(new Vector2(0, jumpPower), ForceMode2D.Impulse);
        isJumping = false;
    }

    void Die()
    {
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            anim.SetTrigger("die");
            alive = false;
        }
    }

    void Restart()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            anim.SetTrigger("idle");
            alive = true;
        }
    }

    public void DieAndShowRetryUI()
    {
        if (isRespawning) return;

        isRespawning = true;
        alive = false;
        anim.SetTrigger("die");

        StartCoroutine(WaitAndShowRetryUI());
    }

    private IEnumerator WaitAndShowRetryUI()
    {
        yield return new WaitForSeconds(1f); // 死亡アニメに合わせて
        RespawnPlayer();
    }

    public void RespawnPlayer()
    {
        //transform.position = respawnPoint.position;
        //anim.SetTrigger("idle");
        //alive = true;
        //isRespawning = false;
        retryPanel.SetActive(true);
    }

    // デバッグ用：SceneビューでRayを表示
    void OnDrawGizmosSelected()
    {
        if (groundCheckPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(groundCheckPoint.position, groundCheckPoint.position + Vector3.down * groundCheckRadius);
        }
    }
}