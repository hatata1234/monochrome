using UnityEngine;

public class CustomCursorController : MonoBehaviour
{
    public Texture2D cursorTexture; // �\���������摜
    public Vector2 offset;          // �摜�̈ʒu�����p�I�t�Z�b�g

    private Vector2 cursorPos;

    void Start()
    {
        // OS�̃J�[�\����\��
        Cursor.visible = false;
    }

    void Update()
    {
        // �}�E�X�̃X�N���[�����W���擾
        cursorPos = Input.mousePosition;

        // �摜�̈ʒu�����炵�����ꍇ�� offset ���g��
        cursorPos.x += offset.x;
        cursorPos.y += offset.y;
    }

    void OnGUI()
    {
        // GUI���W�͍��オ(0,0)�A�}�E�X���W�͍�����(0,0)�Ȃ̂�y�𔽓]����
        float drawX = cursorPos.x;
        float drawY = Screen.height - cursorPos.y;

        // �摜�̑傫�����擾
        float width = cursorTexture.width;
        float height = cursorTexture.height;

        // �`��ʒu������i�摜�̒��S�ɍ��킹�����Ȃ�offset�������Ăˁj
        Rect rect = new Rect(drawX, drawY, width, height);

        // �摜��`��
        GUI.DrawTexture(rect, cursorTexture);
    }
}