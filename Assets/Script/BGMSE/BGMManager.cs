using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AudioSettings : MonoBehaviour
{
    [Header("BGM")]
    public Slider bgmSlider; // Inspector でアサイン
    private AudioSource bgmAudioSource;

    private void Start()
    {
        // AudioSource を取得
        bgmAudioSource = GetComponent<AudioSource>();

        // 初期値を反映
        bgmAudioSource.volume = bgmSlider.value;

        // スライダー操作で音量変更
        bgmSlider.onValueChanged.AddListener(value =>
        {
            bgmAudioSource.volume = value;
        });
    }
}