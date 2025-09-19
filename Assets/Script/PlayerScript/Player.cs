using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float movePower = 10f;
    public float jumpPower = 20f;

    private Rigidbody2D rb;
    private Animator anim;
    private int direction = 1;

    private bool isJumping = false;
    private bool alive = true;

    // 地面判定（Raycast）
    private bool isGrounded = false;
    public Transform groundCheckPoint;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    public Transform respawnPoint;
    private bool isRespawning = false;

    [Header("Retry UI")]
    public GameObject retryPanel;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (PauseManager.IsPaused || Time.timeScale == 0) return;

        // 1. 接地判定は最初に行う
        isGrounded = Physics2D.Raycast(groundCheckPoint.position, Vector2.down, groundCheckRadius, groundLayer);

        // 2. アニメーションフラグ更新
        anim.SetBool("isJump", !isGrounded);

        // 3. 各処理
        Restart();

        if (alive)
        {
            Die();
            Run();
            Jump();
        }
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
            if (isGrounded)
                anim.SetBool("isRun", true);
        }
        else if (h > 0)
        {
            direction = 1;
            moveVelocity = Vector3.right;
            transform.localScale = new Vector3(direction, 1, 1);
            if (isGrounded)
                anim.SetBool("isRun", true);
        }

        transform.position += moveVelocity * movePower * Time.deltaTime;
    }

    void Jump()
    {
        if ((Input.GetButtonDown("Jump") || Input.GetAxisRaw("Vertical") > 0) && isGrounded)
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(new Vector2(0, jumpPower), ForceMode2D.Impulse);
        }
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
        yield return new WaitForSeconds(1f);
        Time.timeScale = 0f;
        RespawnPlayer();
    }

    public void RespawnPlayer()
    {
        retryPanel.SetActive(true);
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheckPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(groundCheckPoint.position, groundCheckPoint.position + Vector3.down * groundCheckRadius);
        }
    }
}