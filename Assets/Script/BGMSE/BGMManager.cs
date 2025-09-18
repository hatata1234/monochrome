using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BGMManager : MonoBehaviour
{
    [Header("BGM")]
    public Slider bgmSlider; // Inspector �ŃA�T�C��
    private AudioSource bgmAudioSource;

    private void Start()
    {
        // AudioSource ���擾
        bgmAudioSource = GetComponent<AudioSource>();

        // �����l�𔽉f
        bgmAudioSource.volume = bgmSlider.value;

        // �X���C�_�[����ŉ��ʕύX
        bgmSlider.onValueChanged.AddListener(value =>
        {
            bgmAudioSource.volume = value;
        });
    }
}