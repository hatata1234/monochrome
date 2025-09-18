using UnityEngine;
using UnityEngine.UI;

public class MainMenuScoreDisplay : MonoBehaviour
{
    [System.Serializable]
    public struct StageScoreUI
    {
        public string stageId;
        public Text scoreText;
    }

    public StageScoreUI[] stages;

    void Start()
    {
        Time.timeScale = 1f;
        foreach (var stage in stages)
        {
            int score = GameManager.Instance.GetHighestScore(stage.stageId);
            string stars = new string('Åö', score) + new string('Åô', 3 - score);
            stage.scoreText.text = stars;
        }
    }
}