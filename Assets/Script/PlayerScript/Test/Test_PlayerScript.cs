using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_playerscript : MonoBehaviour
{
    public float movePower = 10f;

    private Rigidbody2D rb;
    private Animator anim;
    private int direction = 1;

    private bool alive = true;

    public Transform respawnPoint;
    private bool isRespawning = false;

    [Header("Retry UI")]
    public GameObject retryPanel;

    // �q�I�u�W�F�N�g�܂ޑSSpriteRenderer��ێ�
    private List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        // �����Ǝq�I�u�W�F�N�g�S�Ă�SpriteRenderer���擾���ĕێ�
        spriteRenderers.AddRange(GetComponentsInChildren<SpriteRenderer>());

        // ���E���]�C�x���g�̓o�^
        if (WorldFlipManager.Instance != null)
        {
            WorldFlipManager.Instance.OnWorldFlipped += OnWorldFlipped;

            // ������Ԃɍ��킹�ĐF���Z�b�g
            OnWorldFlipped(WorldFlipManager.Instance.IsBlackWorld);
        }
    }

    private void OnDestroy()
    {
        // �C�x���g�o�^����
        if (WorldFlipManager.Instance != null)
        {
            WorldFlipManager.Instance.OnWorldFlipped -= OnWorldFlipped;
        }
    }

    void Update()
    {
        if (PauseManager.IsPaused || Time.timeScale == 0) return;

        Restart();

        if (alive)
        {
            Die();
            Move();
        }
    }

    void Move()
    {
        Vector2 moveVelocity = Vector2.zero;
        anim.SetBool("isRun", false);

        float h = Input.GetAxisRaw("Horizontal");  // A/D or ��/��
        float v = Input.GetAxisRaw("Vertical");    // W/S or ��/��

        moveVelocity = new Vector2(h, v).normalized;

        if (moveVelocity.magnitude > 0.1f)
        {
            anim.SetBool("isRun", true);

            // �����ύX�i���E���]�̂݁A��≺�ł͔��]���Ȃ��j
            if (h != 0)
            {
                direction = h > 0 ? 1 : -1;
                transform.localScale = new Vector3(direction, 1, 1);
            }
        }

        transform.position += (Vector3)moveVelocity * movePower * Time.deltaTime;
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

    private void OnWorldFlipped(bool isBlackWorld)
    {
        // ���̐��E�Ȃ甒�A���̐��E�Ȃ獕�ɐF�ύX
        Color targetColor = isBlackWorld ? Color.white : Color.black;

        foreach (var sr in spriteRenderers)
        {
            sr.color = targetColor;
        }
    }
}
