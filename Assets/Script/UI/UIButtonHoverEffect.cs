using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Vector3 hoverScale = new Vector3(1.1f, 1.1f, 1f);  // �g��T�C�Y
    public Color hoverTextColor = Color.red;                  // �z�o�[���̃e�L�X�g�F

    private Vector3 originalScale;
    private Color originalTextColor;

    private Text buttonText;

    void Start()
    {
        // ���̃T�C�Y��ۑ�
        originalScale = transform.localScale;

        // �q��Text�R���|�[�l���g���擾�i�{�^����Text��1��������z��j
        buttonText = GetComponentInChildren<Text>();

        if (buttonText != null)
        {
            originalTextColor = buttonText.color;
        }
        else
        {
            Debug.LogWarning("UIButtonHoverEffect: �q�I�u�W�F�N�g��Text��������܂���B");
        }
    }

    // �J�[�\�����{�^���ɓ������Ƃ�
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = hoverScale;
        if (buttonText != null)
        {
            buttonText.color = hoverTextColor;
        }
    }

    // �J�[�\�����{�^�����痣�ꂽ�Ƃ�
    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = originalScale;
        if (buttonText != null)
        {
            buttonText.color = originalTextColor;
        }
    }
}