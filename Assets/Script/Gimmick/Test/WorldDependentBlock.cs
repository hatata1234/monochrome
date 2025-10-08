//using UnityEngine;

//[RequireComponent(typeof(SpriteRenderer))]
//[RequireComponent(typeof(Collider2D))]
//public class WorldDependentBlock : MonoBehaviour
//{
//    private SpriteRenderer spriteRenderer;
//    private Collider2D col;

//    [Header("Œ©‚½–Ú‚Ìİ’è")]
//    public Color colorInWhiteWorld = Color.black;
//    public Color colorInBlackWorld = Color.white;

//    private void Awake()
//    {
//        spriteRenderer = GetComponent<SpriteRenderer>();
//        col = GetComponent<Collider2D>();
//    }

//    private void Start()
//    {
//        // ‰Šúó‘Ô‚É‡‚í‚¹‚ÄØ‚è‘Ö‚¦
//        UpdateBlockState(WorldFlipManager.Instance.IsBlackWorld);

//        // ¢ŠE”½“]ƒCƒxƒ“ƒg‚É“o˜^
//        WorldFlipManager.Instance.OnWorldFlipped += UpdateBlockState;
//    }

//    private void OnDestroy()
//    {
//        // ƒCƒxƒ“ƒg‰ğœ
//        if (WorldFlipManager.Instance != null)
//        {
//            WorldFlipManager.Instance.OnWorldFlipped -= UpdateBlockState;
//        }
//    }

//    private void UpdateBlockState(bool isBlackWorld)
//    {
//        if (isBlackWorld)
//        {
//            // •‚Ì¢ŠEF”’‚­‚È‚éA“–‚½‚è”»’è‚È‚µ
//            spriteRenderer.color = colorInBlackWorld;
//            col.enabled = false;
//        }
//        else
//        {
//            // ”’‚Ì¢ŠEF•‚­‚È‚éA“–‚½‚è”»’è‚ ‚è
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

    [Header("Fİ’è")]
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
        // ‰Šúó‘Ô”½‰f
        UpdateTilemapState(WorldFlipManager.Instance.IsBlackWorld);

        // ƒCƒxƒ“ƒg“o˜^
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