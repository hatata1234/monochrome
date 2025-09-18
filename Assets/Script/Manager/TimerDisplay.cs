using UnityEngine;
using UnityEngine.UI;

public class TimerDisplay : MonoBehaviour
{
    public Text timerText; // ← Text コンポーネントへの参照

    void Update()
    {
        if (Timer.Instance != null)
        {
            string formatted = Timer.Instance.GetFormattedTime();
            timerText.text = $"タイム: {formatted}";
        }
    }
}