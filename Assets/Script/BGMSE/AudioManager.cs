using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instans;

    [Header("Audio Sources")]
    public AudioSource BGMSource;
    public AudioSource SESource;

    [Header("Audio Mixer")]
    public AudioMixer mixer;

    [Header("Sliders")]
    public Slider BGMslider;
    public Slider SEslider;

    [Header("Audio clips")]
    public AudioClip[] BGMs;
    public AudioClip[] SEs;

    private void Awake()
    {
        if (Instans == null)
        {
            Instans = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // スライダー初期化
        BGMslider.value = Update_Volume.BGMsliderValue;
        SEslider.value = Update_Volume.SEsliderValue;

        BGMslider.onValueChanged.AddListener(SetBGMVolume);
        SEslider.onValueChanged.AddListener(SetSEVolume);

        // 音量を反映
        SetBGMVolume(BGMslider.value);
        SetSEVolume(SEslider.value);

        // BGM 再生
        if (BGMs.Length > 0)
        {
            BGMSource.clip = BGMs[0];
            BGMSource.loop = true;
            BGMSource.Play();
        }
    }

    // シーン切り替え時にスライダー再リンク
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        var bgmSliderObj = GameObject.Find("BGM_Slider");
        var seSliderObj = GameObject.Find("SE_Slider");

        if (bgmSliderObj != null)
        {
            BGMslider = bgmSliderObj.GetComponent<Slider>();
            BGMslider.value = Update_Volume.BGMsliderValue;
            BGMslider.onValueChanged.RemoveAllListeners();
            BGMslider.onValueChanged.AddListener(SetBGMVolume);
        }

        if (seSliderObj != null)
        {
            SEslider = seSliderObj.GetComponent<Slider>();
            SEslider.value = Update_Volume.SEsliderValue;
            SEslider.onValueChanged.RemoveAllListeners();
            SEslider.onValueChanged.AddListener(SetSEVolume);
        }
    }

    // AudioMixer に反映
    public void SetBGMVolume(float value)
    {
        float dB = (value <= 0.0001f) ? -80f : Mathf.Log10(value) * 20f;
        mixer.SetFloat("BGMVolume", dB);
        Update_Volume.BGMsliderValue = value;
    }

    public void SetSEVolume(float value)
    {
        float dB = (value <= 0.0001f) ? -80f : Mathf.Log10(value) * 20f;
        mixer.SetFloat("SEVolume", dB);
        Update_Volume.SEsliderValue = value;
    }

    public void PlaySE(int index)
    {
        if (index >= 0 && index < SEs.Length)
            SESource.PlayOneShot(SEs[index]);
    }
}


