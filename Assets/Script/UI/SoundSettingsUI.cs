using UnityEngine;
using UnityEngine.UI;

public class SoundSettingsUI : MonoBehaviour
{
    [SerializeField] private Slider bgmSlider; // BGM�p�X���C�_�[
    [SerializeField] private Slider seSlider;  // SE�p�X���C�_�[

    private void Start()
    {
        // === �X���C�_�[�̏����� ===
        bgmSlider.value = SettingVariable.Instance.Data.BGMVolume;
        seSlider.value = SettingVariable.Instance.Data.SEVolume;

        // === �X���C�_�[�ύX���̏��� ===
        bgmSlider.onValueChanged.AddListener(v => AudioManager.Instance.SetBgmVolume(v));
        seSlider.onValueChanged.AddListener(v => AudioManager.Instance.SetSeVolume(v));
    }
}
