using UnityEngine;
using UnityEngine.UI;

public class GoalPanelController : MonoBehaviour
{
    [Header("UI�Q��")]
    public GameObject goalPanel;
    public Text resultPlacedText;
    public Text evaluationText;
    public Text timeText;

    [Header("Placer�̎���ID")]
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
        timeText.text = $"�^�C��: {timeStr}";

        ObjectPlacer2D placer = ObjectPlacer2D.FindPlacerById(placerId);
        if (placer != null)
        {
            int count = placer.TotalPlacedCount;
            resultPlacedText.text = $"�ݐϔz�u��: {count}";

            evaluationText.text = $"�]��: {GetEvaluation(count, elapsedSeconds)}";
        }
        else
        {
            resultPlacedText.text = "�z�u�����Ȃ�";
            evaluationText.text = "�]��: ���]��";
        }

        goalPanel.SetActive(true);
    }

    private string GetEvaluation(int placedCount, float elapsedSeconds)
    {
        int stars = 0;

        // ����1: �S�[�������̂� ��1 (���̃��\�b�h���Ă΂��̂̓N���A���Ȃ̂ŏ�ɃN���A�Ƃ��ā�1)
        stars += 1;

        // ����2: 60�b�ȓ��N���A
        if (elapsedSeconds <= 60f)
        {
            stars += 1;
        }

        // ����3: �ݐϐݒu����10�ȉ�
        if (placedCount <= 10)
        {
            stars += 1;
        }

        // ���𕶎���ŕԂ��i��: �������j
        return new string('��', stars) + new string('��', 3 - stars);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // �S�[���p�l����\��
            FindObjectOfType<GoalPanelController>().ShowGoalPanel();

            // �Q�[�����~����Ȃǂ̏�����������
            Time.timeScale = 0f;
        }
    }
}

