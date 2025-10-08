using UnityEngine;
using UnityEngine.Tilemaps;

public class InverseWorldDependentBlock : MonoBehaviour
{
    private TilemapRenderer tilemapRenderer;
    private SpriteRenderer spriteRenderer;
    private TilemapCollider2D tilemapCollider;
    private Collider2D genericCollider;

    [Header("�F�ݒ�")]
    public Color colorInWhiteWorld = Color.white;
    public Color colorInBlackWorld = Color.black;

    private void Awake()
    {
        tilemapRenderer = GetComponent<TilemapRenderer>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        tilemapCollider = GetComponent<TilemapCollider2D>();
        genericCollider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        UpdateTilemapState(WorldFlipManager.Instance.IsBlackWorld);
        WorldFlipManager.Instance.OnWorldFlipped += UpdateTilemapState;
    }

    private void OnDestroy()
    {
        if (WorldFlipManager.Instance != null)
        {
            WorldFlipManager.Instance.OnWorldFlipped -= UpdateTilemapState;
        }
    }

    private void UpdateTilemapState(bool isBlackWorld)
    {
        Color targetColor = isBlackWorld ? colorInBlackWorld : colorInWhiteWorld;
        bool colliderEnabled = isBlackWorld; // �����E�œ����蔻��I��

        if (tilemapRenderer != null)
        {
            tilemapRenderer.material.color = targetColor;
        }

        if (spriteRenderer != null)
        {
            spriteRenderer.color = targetColor;
        }

        if (tilemapCollider != null)
        {
            tilemapCollider.enabled = colliderEnabled;
        }
        else if (genericCollider != null)
        {
            genericCollider.enabled = colliderEnabled;
        }
    }
}
