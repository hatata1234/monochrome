using UnityEngine;
using UnityEngine.UI;

public class SoundSettingsUI : MonoBehaviour
{
    [SerializeField] private Slider bgmSlider; // BGM用スライダー
    [SerializeField] private Slider seSlider;  // SE用スライダー

    private void Start()
    {
        // === スライダーの初期化 ===
        bgmSlider.value = SettingVariable.Instance.Data.BGMVolume;
        seSlider.value = SettingVariable.Instance.Data.SEVolume;

        // === スライダー変更時の処理 ===
        bgmSlider.onValueChanged.AddListener(v => AudioManager.Instance.SetBgmVolume(v));
        seSlider.onValueChanged.AddListener(v => AudioManager.Instance.SetSeVolume(v));
    }
}
