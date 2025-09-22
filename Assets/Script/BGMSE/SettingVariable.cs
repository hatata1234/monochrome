using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingVariable : MonoBehaviour
{
    public static SettingVariable Instance;

    // ���ʃf�[�^���܂Ƃ߂��N���X
    [SerializeField] SettingData m_settingData = new SettingData();
    public SettingData Data => m_settingData;

    private void Awake()
    {
        // �V���O���g�����i�d��������j���j
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // �V�[���J�ڂ��Ă��c��
        DontDestroyOnLoad(gameObject);
    }

    [Serializable]
    public class SettingData
    {
        [SerializeField] float m_bgmVolume = 1f; // BGM����
        [SerializeField] float m_seVolume = 1f;  // SE����

        public float BGMVolume { get => m_bgmVolume; set => m_bgmVolume = value; }
        public float SEVolume { get => m_seVolume; set => m_seVolume = value; }
    }
}
