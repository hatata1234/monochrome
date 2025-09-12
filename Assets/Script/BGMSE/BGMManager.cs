using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AudioSettings : MonoBehaviour
{
    [Header("BGM")]
    public Slider bgmSlider;
    private AudioSource bgmAudioSource;


    private void Start()
    {
        bgmSlider.onValueChanged.AddListener(value => bgmAudioSource.volume = value);

    }

    private void Update()
    {

    }
}