using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("BGM Settings")]
    public AudioSource bgmSource;         // BGM 用 AudioSource
    public Slider bgmSlider;              // BGM 音量用スライダー


    [Header("SE Settings")]
    public GameObject seSourcePrefab;     // SE 再生用プレハブ（AudioSource付き）
    public Slider seSlider;               // SE 音量用スライダー

    private List<AudioSource> seSources = new List<AudioSource>(); // 再生中SEリスト
    private float seVolume = 1f;          // SE音量

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // BGMスライダー設定
        if (bgmSlider != null)
        {
            bgmSlider.onValueChanged.AddListener(SetBGMVolume);
            bgmSlider.value = bgmSource.volume; // 初期値
        }

        // SEスライダー設定
        if (seSlider != null)
        {
            seSlider.onValueChanged.AddListener(SetSEVolume);
            seSlider.value = seVolume; // 初期値
        }

        // スタート時にBGMを流す
        if (bgmSource != null && bgmSource.clip != null)
        {
            bgmSource.loop = true;
            bgmSource.Play();
        }
    }

    // BGM 再生
    public void PlayBGM(AudioClip clip)
    {
        if (clip == null) return;
        bgmSource.clip = clip;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    // SE 再生
    public void PlaySE(AudioClip clip)
    {
        if (clip == null || seSourcePrefab == null) return;

        GameObject seObj = Instantiate(seSourcePrefab, transform);
        AudioSource source = seObj.GetComponent<AudioSource>();
        source.clip = clip;
        source.volume = seVolume;
        source.Play();
        seSources.Add(source);

        Destroy(seObj, clip.length);
    }

    // 🔊 BGM 音量変更
    public void SetBGMVolume(float volume)
    {
        bgmSource.volume = volume;
    }

    // 🔊 SE 音量変更
    public void SetSEVolume(float volume)
    {
        seVolume = volume;
        // 再生中のSEにも反映
        foreach (var src in seSources)
        {
            if (src != null) src.volume = seVolume;
        }
    }
}
