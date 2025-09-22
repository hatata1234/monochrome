using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingVariable : MonoBehaviour
{
    public static SettingVariable Instance;

    // 音量データをまとめたクラス
    [SerializeField] SettingData m_settingData = new SettingData();
    public SettingData Data => m_settingData;

    private void Awake()
    {
        // シングルトン化（重複したら破棄）
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // シーン遷移しても残す
        DontDestroyOnLoad(gameObject);
    }

    [Serializable]
    public class SettingData
    {
        [SerializeField] float m_bgmVolume = 1f; // BGM音量
        [SerializeField] float m_seVolume = 1f;  // SE音量

        public float BGMVolume { get => m_bgmVolume; set => m_bgmVolume = value; }
        public float SEVolume { get => m_seVolume; set => m_seVolume = value; }
    }
}
