//using UnityEngine;

//[RequireComponent(typeof(SpriteRenderer))]
//[RequireComponent(typeof(Collider2D))]
//public class WorldDependentBlock : MonoBehaviour
//{
//    private SpriteRenderer spriteRenderer;
//    private Collider2D col;

//    [Header("�����ڂ̐ݒ�")]
//    public Color colorInWhiteWorld = Color.black;
//    public Color colorInBlackWorld = Color.white;

//    private void Awake()
//    {
//        spriteRenderer = GetComponent<SpriteRenderer>();
//        col = GetComponent<Collider2D>();
//    }

//    private void Start()
//    {
//        // ������Ԃɍ��킹�Đ؂�ւ�
//        UpdateBlockState(WorldFlipManager.Instance.IsBlackWorld);

//        // ���E���]�C�x���g�ɓo�^
//        WorldFlipManager.Instance.OnWorldFlipped += UpdateBlockState;
//    }

//    private void OnDestroy()
//    {
//        // �C�x���g����
//        if (WorldFlipManager.Instance != null)
//        {
//            WorldFlipManager.Instance.OnWorldFlipped -= UpdateBlockState;
//        }
//    }

//    private void UpdateBlockState(bool isBlackWorld)
//    {
//        if (isBlackWorld)
//        {
//            // ���̐��E�F�����Ȃ�A�����蔻��Ȃ�
//            spriteRenderer.color = colorInBlackWorld;
//            col.enabled = false;
//        }
//        else
//        {
//            // ���̐��E�F�����Ȃ�A�����蔻�肠��
//            spriteRenderer.color = colorInWhiteWorld;
//            col.enabled = true;
//        }
//    }
//}
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldDependentBlock : MonoBehaviour
{
    private TilemapRenderer tilemapRenderer;
    private SpriteRenderer spriteRenderer;
    private TilemapCollider2D tilemapCollider;
    private Collider2D genericCollider;

    [Header("�F�ݒ�")]
    public Color colorInWhiteWorld = Color.black;
    public Color colorInBlackWorld = Color.white;

    private void Awake()
    {
        tilemapRenderer = GetComponent<TilemapRenderer>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        tilemapCollider = GetComponent<TilemapCollider2D>();
        genericCollider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        // ������Ԕ��f
        UpdateTilemapState(WorldFlipManager.Instance.IsBlackWorld);

        // �C�x���g�o�^
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

        if (tilemapRenderer != null)
        {
            tilemapRenderer.material.color = targetColor;
        }

        if (spriteRenderer != null)
        {
            spriteRenderer.color = targetColor;
        }

        bool colliderEnabled = !isBlackWorld;

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