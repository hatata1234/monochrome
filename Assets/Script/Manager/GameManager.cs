using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private Dictionary<string, int> highestScores = new Dictionary<string, int>();
    private HashSet<string> clearedStages = new HashSet<string>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // �V�[���؂�ւ����Ƀ^�C�}�[�N�����`�F�b�N
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // �� �^�C�}�[���V�[���J�n���ɋN���i�X�e�[�W�V�[���݂̂ȂǏ����t����OK�j
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // �K�v�Ȃ������ǉ��i��Fscene.name.StartsWith("Stage") �Ȃǁj
        if (Timer.Instance != null)
        {
            Timer.Instance.StartTimer();
        }
    }

    public void UpdateHighestScore(string stageId, int stars)
    {
        if (!highestScores.ContainsKey(stageId) || stars > highestScores[stageId])
        {
            highestScores[stageId] = stars;
        }

        if (!clearedStages.Contains(stageId))
        {
            clearedStages.Add(stageId);
        }
    }

    public int GetHighestScore(string stageId)
    {
        return highestScores.TryGetValue(stageId, out int score) ? score : 0;
    }

    public bool IsStageCleared(string stageId)
    {
        return clearedStages.Contains(stageId);
    }
}