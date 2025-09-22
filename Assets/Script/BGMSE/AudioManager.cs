using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioMixer audioMixer; // AudioMixer�Q��

    [SerializeField] private GameObject seSourcePrefab; // SE��AudioSource���g���܂킷

    private void Awake()
    {
        // �V���O���g����
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // �N�����ɉ��ʂ𔽉f
        ApplyVolumes();
    }

    private void PlaySE(AudioClip clip)
    {
        if (clip == null) return;

        // �ꎞ�I��SE�Đ��p��AudioSource�𐶐�
        var obj = Instantiate(seSourcePrefab, transform);
        var source = obj.GetComponent<AudioSource>();
        source.PlayOneShot(clip);

        // �Đ����I�������j��
        Destroy(obj, clip.length);
    }

    // �l�𔽉f����i�V�[���J�ڌ�ɌĂяo���p�j
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
