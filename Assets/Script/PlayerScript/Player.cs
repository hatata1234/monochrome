using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float movePower = 10f;
    public float KickBoardMovePower = 15f;
    public float jumpPower = 20f; // Rigidbody2D��Gravity Scale��5����

    private Rigidbody2D rb;
    private Animator anim;
    private int direction = 1;

    private bool isJumping = false;
    private bool alive = true;
    private bool isKickboard = false;

    // �n�ʔ���p�iRaycast�j
    private bool isGrounded = false;
    public Transform groundCheckPoint;    // �����̔���|�C���g
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

   
    public Transform respawnPoint; // ���X�|�[���n�_

    private bool isRespawning = false;

    void Start()
    {
       
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        Restart();

        if (alive)
        {
            Hurt();
            Die();
            Attack();
            KickBoard();
            Run();
            Jump();
        }

        // �n�ʂɐڒn���Ă��邩����iRaycast���g�p�j
        isGrounded = Physics2D.Raycast(groundCheckPoint.position, Vector2.down, groundCheckRadius, groundLayer);

        if (isGrounded)
        {
            // �ڒn���Ă���΃W�����v�A�j���[�V�������I�t�i�K�v�ł���΁j
            // anim.SetBool("isJump", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        anim.SetBool("isJump", false);
    }

    void KickBoard()
    {
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            isKickboard = !isKickboard;
            anim.SetBool("isKickBoard", isKickboard);
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

        float speed = isKickboard ? KickBoardMovePower : movePower;
        transform.position += moveVelocity * speed * Time.deltaTime;
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

    void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            anim.SetTrigger("attack");
        }
    }

    void Hurt()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            anim.SetTrigger("hurt");
            float knockbackX = direction == 1 ? -5f : 5f;
            rb.AddForce(new Vector2(knockbackX, 1f), ForceMode2D.Impulse);
        }
    }

    void Die()
    {
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            isKickboard = false;
            anim.SetBool("isKickBoard", false);
            anim.SetTrigger("die");
            alive = false;
        }
    }

    void Restart()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            isKickboard = false;
            anim.SetBool("isKickBoard", false);
            anim.SetTrigger("idle");
            alive = true;
        }
    }

    public void DieAndShowRetryUI()
    {
        if (isRespawning) return;

        isRespawning = true;
        alive = false;
        isKickboard = false;
        anim.SetBool("isKickBoard", false);
        anim.SetTrigger("die");

        StartCoroutine(WaitAndShowRetryUI());
    }

    private IEnumerator WaitAndShowRetryUI()
    {
        yield return new WaitForSeconds(1f); // ���S�A�j���ɍ��킹��
      
    }

    public void RespawnPlayer()
    {
        transform.position = respawnPoint.position;
        anim.SetTrigger("idle");
        alive = true;
        isRespawning = false;
    }

    // �f�o�b�O�p�FScene�r���[��Ray��\��
    void OnDrawGizmosSelected()
    {
        if (groundCheckPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(groundCheckPoint.position, groundCheckPoint.position + Vector3.down * groundCheckRadius);
        }
    }
}