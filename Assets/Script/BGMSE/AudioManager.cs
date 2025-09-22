using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioMixer audioMixer; // AudioMixer参照

    [SerializeField] private GameObject seSourcePrefab;

    private void Awake()
    {
        // シングルトン化
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // 起動時に音量を反映
        ApplyVolumes();
    }
    public void PlaySE(AudioClip clip)
    {
        if (clip == null) return;

        // 一時的にSE再生用のAudioSourceを生成
        var obj = Instantiate(seSourcePrefab, transform);
        var source = obj.GetComponent<AudioSource>();
        source.PlayOneShot(clip);

        // 再生が終わったら破棄
        Destroy(obj, clip.length);
    }

    // 値を反映する（シーン遷移後に呼び出す用）
    public void ApplyVolumes()
    {
        SetBgmVolume(SettingVariable.Instance.Data.BGMVolume);
        SetSeVolume(SettingVariable.Instance.Data.SEVolume);
    }

    // ==== BGM ====
    public void SetBgmVolume(float value)
    {
        SettingVariable.Instance.Data.BGMVolume = value;
        audioMixer.SetFloat("BGMVolume", Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20);
    }

    // ==== SE ====
    public void SetSeVolume(float value)
    {
        SettingVariable.Instance.Data.SEVolume = value;
        audioMixer.SetFloat("SEVolume", Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20);
    }
}
