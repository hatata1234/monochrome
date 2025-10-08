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

    // 子オブジェクト含む全SpriteRendererを保持
    private List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        // 自分と子オブジェクト全てのSpriteRendererを取得して保持
        spriteRenderers.AddRange(GetComponentsInChildren<SpriteRenderer>());

        // 世界反転イベントの登録
        if (WorldFlipManager.Instance != null)
        {
            WorldFlipManager.Instance.OnWorldFlipped += OnWorldFlipped;

            // 初期状態に合わせて色をセット
            OnWorldFlipped(WorldFlipManager.Instance.IsBlackWorld);
        }
    }

    private void OnDestroy()
    {
        // イベント登録解除
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

        float h = Input.GetAxisRaw("Horizontal");  // A/D or ←/→
        float v = Input.GetAxisRaw("Vertical");    // W/S or ↑/↓

        moveVelocity = new Vector2(h, v).normalized;

        if (moveVelocity.magnitude > 0.1f)
        {
            anim.SetBool("isRun", true);

            // 向き変更（左右反転のみ、上や下では反転しない）
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
        // 黒の世界なら白、白の世界なら黒に色変更
        Color targetColor = isBlackWorld ? Color.white : Color.black;

        foreach (var sr in spriteRenderers)
        {
            sr.color = targetColor;
        }
    }
}
