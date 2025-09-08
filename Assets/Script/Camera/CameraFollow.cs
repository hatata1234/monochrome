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

    // 追加: ズームの最大・最小値
    public float minZoomSize = 3f;
    public float maxZoomSize = 12f;

    // 追加: マウスホイール感度
    public float zoomSpeed = 2f;

    void Start()
    {
        cam = GetComponent<Camera>();

        if (cam == null)
        {
            Debug.LogError("Camera コンポーネントが見つかりません。");
        }

        if (player == null)
        {
            Debug.LogWarning("Player が設定されていません。シーン内のオブジェクトをインスペクタから設定してください。");
        }
    }

    void LateUpdate()
    {
        if (player == null) return;

        // マウスホイール入力でズームサイズ変更
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