using UnityEngine;
using UnityEngine.UI;

public class TimerDisplay : MonoBehaviour
{
    public Text timerText; // �� Text �R���|�[�l���g�ւ̎Q��

    void Update()
    {
        if (Timer.Instance != null)
        {
            string formatted = Timer.Instance.GetFormattedTime();
            timerText.text = $"�^�C��: {formatted}";
        }
    }
}