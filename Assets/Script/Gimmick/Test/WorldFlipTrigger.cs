using UnityEngine;

public class WorldFlipTrigger : MonoBehaviour
{
    public float flipCooldown = 2f; // �N�[���^�C��(�b)

    private float lastFlipTime = -Mathf.Infinity;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // �v���C���[�^�O����
        {
            float currentTime = Time.time;
            if (currentTime - lastFlipTime >= flipCooldown)
            {
                WorldFlipManager.Instance?.FlipWorld();
                lastFlipTime = currentTime;
            }
            else
            {
                Debug.Log("���]�̓N�[���^�C�����ł�");
            }
        }
    }
}
