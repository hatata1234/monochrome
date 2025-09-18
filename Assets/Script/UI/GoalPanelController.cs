using UnityEngine;
using UnityEngine.UI;

public class GoalPanelController : MonoBehaviour
{
    public GameObject goalPanel;
    public Text resultPlacedText;
    public Text evaluationText;
    public Text timeText;
    public string placerId = "Placer_1";
    public string stageId = "Stage1";

    public void ShowGoalPanel()
    {
        Timer.Instance.StopTimer();
        float elapsedSeconds = Timer.Instance.GetElapsedTime();
        string formattedTime = Timer.Instance.GetFormattedTime();

        ObjectPlacer2D placer = ObjectPlacer2D.FindPlacerById(placerId);
        int count = placer != null ? placer.TotalPlacedCount : 0;

        // ★/☆ 条件表示付きテキスト
        timeText.text = $"{(elapsedSeconds <= 60f ? "★" : "☆")}";
        //timeText.text = $" {(elapsedSeconds <= 60f ? "★" : "☆")}( タイム: {formattedTime} ";
        resultPlacedText.text = $"{(count <= 10 ? "★" : "☆")}";
        //resultPlacedText.text = $" {(count <= 10 ? "★" : "☆")}( 累積配置数: {count}) ";

        // 総合評価
        string stars = GetEvaluation(count, elapsedSeconds);
        evaluationText.text = $"評価: {stars}";

        // スコア保存
        int starCount = CountStars(stars);
        GameManager.Instance.UpdateHighestScore(stageId, starCount);

        goalPanel.SetActive(true);
    }

    string GetEvaluation(int placedCount, float elapsedSeconds)
    {
        int stars = 1;
        if (elapsedSeconds <= 60f) stars += 1;
        if (placedCount <= 10) stars += 1;
        return new string('★', stars) + new string('☆', 3 - stars);
    }

    int CountStars(string stars)
    {
        int count = 0;
        foreach (char c in stars) if (c == '★') count++;
        return count;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ShowGoalPanel();
            Time.timeScale = 0f;
        }
    }
}