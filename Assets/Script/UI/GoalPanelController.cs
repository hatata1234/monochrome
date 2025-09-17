using UnityEngine;
using UnityEngine.UI;

public class GoalPanelController : MonoBehaviour
{
    [Header("UI参照")]
    public GameObject goalPanel;
    public Text resultPlacedText;
    public Text evaluationText;
    public Text timeText;

    [Header("Placerの識別ID")]
    public string placerId = "Placer_1";

    private void Start()
    {
        Timer.Instance.StartTimer();
    }

    public void ShowGoalPanel()
    {
        Timer.Instance.StopTimer();
        float elapsedSeconds = Timer.Instance.GetElapsedTime();
        string timeStr = Timer.Instance.GetFormattedTime();
        timeText.text = $"タイム: {timeStr}";

        ObjectPlacer2D placer = ObjectPlacer2D.FindPlacerById(placerId);
        if (placer != null)
        {
            int count = placer.TotalPlacedCount;
            resultPlacedText.text = $"累積配置数: {count}";

            evaluationText.text = $"評価: {GetEvaluation(count, elapsedSeconds)}";
        }
        else
        {
            resultPlacedText.text = "配置数情報なし";
            evaluationText.text = "評価: 未評価";
        }

        goalPanel.SetActive(true);
    }

    private string GetEvaluation(int placedCount, float elapsedSeconds)
    {
        int stars = 0;

        // 条件1: ゴールしたので ★1 (このメソッドが呼ばれるのはクリア時なので常にクリアとして★1)
        stars += 1;

        // 条件2: 60秒以内クリア
        if (elapsedSeconds <= 60f)
        {
            stars += 1;
        }

        // 条件3: 累積設置数が10以下
        if (placedCount <= 10)
        {
            stars += 1;
        }

        // ★を文字列で返す（例: ★★☆）
        return new string('★', stars) + new string('☆', 3 - stars);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // ゴールパネルを表示
            FindObjectOfType<GoalPanelController>().ShowGoalPanel();

            // ゲームを停止するなどの処理もここで
            Time.timeScale = 0f;
        }
    }
}

