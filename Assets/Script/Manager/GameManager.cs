using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private Dictionary<string, int> highestScores = new Dictionary<string, int>();

    void Awake()
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

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.StartsWith("Game"))
        {
            Timer.Instance?.StartTimer();
        }
    }

    public void UpdateHighestScore(string stageId, int stars)
    {
        if (!highestScores.ContainsKey(stageId) || stars > highestScores[stageId])
        {
            highestScores[stageId] = stars;
        }
    }

    public int GetHighestScore(string stageId)
    {
        return highestScores.TryGetValue(stageId, out int score) ? score : 0;
    }
}