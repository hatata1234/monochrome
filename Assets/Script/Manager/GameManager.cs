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

            // シーン切り替え時にタイマー起動をチェック
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ★ タイマーをシーン開始時に起動（ステージシーンのみなど条件付きでOK）
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 必要なら条件を追加（例：scene.name.StartsWith("Stage") など）
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