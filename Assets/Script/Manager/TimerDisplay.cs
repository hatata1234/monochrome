using UnityEngine;
using UnityEngine.UI;

public class TimerDisplay : MonoBehaviour
{
    public Text timerText;

    void Update()
    {
        if (Timer.Instance != null)
        {
            timerText.text = $"ƒ^ƒCƒ€: {Timer.Instance.GetFormattedTime()}";
        }
    }
}