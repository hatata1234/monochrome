using UnityEngine;
using UnityEngine.UI;

public class GoalPanelController : MonoBehaviour
{
    [Header("UI Components")]
    public GameObject goalPanel;
    public Text resultPlacedText;
    public Text evaluationText;
    public Text timeText;

    [Header("IDs")]
    public string placerId = "Placer_1";
    public string stageId = "Stage1";

    [Header("Clear Conditions")]
    public float clearTimeLimit = 60f;        // �N���A�����F���̕b���ȓ�
    public int maxPlacedCount = 5;           // �N���A�����F���̐��ȉ�

    public void ShowGoalPanel()
    {
        Timer.Instance.StopTimer();
        float elapsedSeconds = Timer.Instance.GetElapsedTime();
        string formattedTime = Timer.Instance.GetFormattedTime();

        ObjectPlacer2D placer = ObjectPlacer2D.FindPlacerById(placerId);
        int count = placer != null ? placer.TotalPlacedCount : 0;

        // �����ɉ������\��
        timeText.text = $"{(elapsedSeconds <= clearTimeLimit ? "��" : "��")} �^�C��: {formattedTime}";
        resultPlacedText.text = $"{(count <= maxPlacedCount ? "��" : "��")} �ݐϔz�u��: {count}";

        // �����]��
        string stars = GetEvaluation(count, elapsedSeconds);
        evaluationText.text = $"�]��: {stars}";

        // �X�R�A�ۑ�
        int starCount = CountStars(stars);
        GameManager.Instance.UpdateHighestScore(stageId, starCount);

        goalPanel.SetActive(true);
    }

    string GetEvaluation(int placedCount, float elapsedSeconds)
    {
        int stars = 1; // �x�[�X1��
        if (elapsedSeconds <= clearTimeLimit) stars += 1;
        if (placedCount <= maxPlacedCount) stars += 1;
        return new string('��', stars) + new string('��', 3 - stars);
    }

    int CountStars(string stars)
    {
        int count = 0;
        foreach (char c in stars)
        {
            if (c == '��') count++;
        }
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
