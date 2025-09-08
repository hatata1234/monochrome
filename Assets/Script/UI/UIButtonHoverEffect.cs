using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Vector3 hoverScale = new Vector3(1.1f, 1.1f, 1f);  // 拡大サイズ
    public Color hoverTextColor = Color.red;                  // ホバー時のテキスト色

    private Vector3 originalScale;
    private Color originalTextColor;

    private Text buttonText;

    void Start()
    {
        // 元のサイズを保存
        originalScale = transform.localScale;

        // 子のTextコンポーネントを取得（ボタンにTextが1つだけある想定）
        buttonText = GetComponentInChildren<Text>();

        if (buttonText != null)
        {
            originalTextColor = buttonText.color;
        }
        else
        {
            Debug.LogWarning("UIButtonHoverEffect: 子オブジェクトにTextが見つかりません。");
        }
    }

    // カーソルがボタンに入ったとき
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = hoverScale;
        if (buttonText != null)
        {
            buttonText.color = hoverTextColor;
        }
    }

    // カーソルがボタンから離れたとき
    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = originalScale;
        if (buttonText != null)
        {
            buttonText.color = originalTextColor;
        }
    }
}