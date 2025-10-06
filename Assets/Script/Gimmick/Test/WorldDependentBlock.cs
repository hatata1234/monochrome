//using UnityEngine;

//[RequireComponent(typeof(SpriteRenderer))]
//[RequireComponent(typeof(Collider2D))]
//public class WorldDependentBlock : MonoBehaviour
//{
//    private SpriteRenderer spriteRenderer;
//    private Collider2D col;

//    [Header("見た目の設定")]
//    public Color colorInWhiteWorld = Color.black;
//    public Color colorInBlackWorld = Color.white;

//    private void Awake()
//    {
//        spriteRenderer = GetComponent<SpriteRenderer>();
//        col = GetComponent<Collider2D>();
//    }

//    private void Start()
//    {
//        // 初期状態に合わせて切り替え
//        UpdateBlockState(WorldFlipManager.Instance.IsBlackWorld);

//        // 世界反転イベントに登録
//        WorldFlipManager.Instance.OnWorldFlipped += UpdateBlockState;
//    }

//    private void OnDestroy()
//    {
//        // イベント解除
//        if (WorldFlipManager.Instance != null)
//        {
//            WorldFlipManager.Instance.OnWorldFlipped -= UpdateBlockState;
//        }
//    }

//    private void UpdateBlockState(bool isBlackWorld)
//    {
//        if (isBlackWorld)
//        {
//            // 黒の世界：白くなる、当たり判定なし
//            spriteRenderer.color = colorInBlackWorld;
//            col.enabled = false;
//        }
//        else
//        {
//            // 白の世界：黒くなる、当たり判定あり
//            spriteRenderer.color = colorInWhiteWorld;
//            col.enabled = true;
//        }
//    }
//}
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
[RequireComponent(typeof(TilemapRenderer))]
[RequireComponent(typeof(TilemapCollider2D))]
public class WorldDependentBlock : MonoBehaviour
{
    private TilemapRenderer tilemapRenderer;
    private TilemapCollider2D tilemapCollider;

    [Header("色設定")]
    public Color colorInWhiteWorld = Color.black;
    public Color colorInBlackWorld = Color.white;

    private void Awake()
    {
        tilemapRenderer = GetComponent<TilemapRenderer>();
        tilemapCollider = GetComponent<TilemapCollider2D>();
    }

    private void Start()
    {
        // 初期状態反映
        UpdateTilemapState(WorldFlipManager.Instance.IsBlackWorld);

        // イベント登録
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
        if (isBlackWorld)
        {
            tilemapRenderer.material.color = colorInBlackWorld;
            tilemapCollider.enabled = false;
        }
        else
        {
            tilemapRenderer.material.color = colorInWhiteWorld;
            tilemapCollider.enabled = true;
        }
    }
}