using UnityEngine;

public class CustomCursorController : MonoBehaviour
{
    public Texture2D cursorTexture; // 表示したい画像
    public Vector2 offset;          // 画像の位置調整用オフセット

    private Vector2 cursorPos;

    void Start()
    {
        // OSのカーソル非表示
        Cursor.visible = false;
    }

    void Update()
    {
        // マウスのスクリーン座標を取得
        cursorPos = Input.mousePosition;

        // 画像の位置をずらしたい場合は offset を使う
        cursorPos.x += offset.x;
        cursorPos.y += offset.y;
    }

    void OnGUI()
    {
        // GUI座標は左上が(0,0)、マウス座標は左下が(0,0)なのでyを反転する
        float drawX = cursorPos.x;
        float drawY = Screen.height - cursorPos.y;

        // 画像の大きさを取得
        float width = cursorTexture.width;
        float height = cursorTexture.height;

        // 描画位置を決定（画像の中心に合わせたいならoffset調整してね）
        Rect rect = new Rect(drawX, drawY, width, height);

        // 画像を描画
        GUI.DrawTexture(rect, cursorTexture);
    }
}