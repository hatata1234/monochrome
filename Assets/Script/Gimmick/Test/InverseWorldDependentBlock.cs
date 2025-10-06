using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(TilemapRenderer))]
[RequireComponent(typeof(TilemapCollider2D))]
public class InverseWorldDependentBlock : MonoBehaviour
{
    private TilemapRenderer tilemapRenderer;
    private TilemapCollider2D tilemapCollider;

    public Color colorInWhiteWorld = Color.white;
    public Color colorInBlackWorld = Color.black;

    private void Awake()
    {
        tilemapRenderer = GetComponent<TilemapRenderer>();
        tilemapCollider = GetComponent<TilemapCollider2D>();
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
        if (isBlackWorld)
        {
            // �����E�F���F�\���E�����蔻�肠��
            tilemapRenderer.material.color = colorInBlackWorld;
            tilemapCollider.enabled = true;
        }
        else
        {
            // �����E�F���F�\���E�����蔻��Ȃ�
            tilemapRenderer.material.color = colorInWhiteWorld;
            tilemapCollider.enabled = false;
        }
    }
}