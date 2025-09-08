using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{
    public Transform player;

    public Vector3 offset = new Vector3(0f, 0f, -10f);

    [Range(0f, 1f)]
    public float smoothSpeed = 0.125f;

    private Camera cam;

    public float zoomSize = 7f;

    // �ǉ�: �Y�[���̍ő�E�ŏ��l
    public float minZoomSize = 3f;
    public float maxZoomSize = 12f;

    // �ǉ�: �}�E�X�z�C�[�����x
    public float zoomSpeed = 2f;

    void Start()
    {
        cam = GetComponent<Camera>();

        if (cam == null)
        {
            Debug.LogError("Camera �R���|�[�l���g��������܂���B");
        }

        if (player == null)
        {
            Debug.LogWarning("Player ���ݒ肳��Ă��܂���B�V�[�����̃I�u�W�F�N�g���C���X�y�N�^����ݒ肵�Ă��������B");
        }
    }

    void LateUpdate()
    {
        if (player == null) return;

        // �}�E�X�z�C�[�����͂ŃY�[���T�C�Y�ύX
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0f)
        {
            zoomSize -= scrollInput * zoomSpeed;
            zoomSize = Mathf.Clamp(zoomSize, minZoomSize, maxZoomSize);
        }

        Vector3 targetPosition = player.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
        transform.position = smoothedPosition;

        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, zoomSize, smoothSpeed);
    }
}