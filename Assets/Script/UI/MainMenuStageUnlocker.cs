using UnityEngine;

public class MainMenuStageUnlocker : MonoBehaviour
{
    [System.Serializable]
    public struct StageUnlockPanel
    {
        public string stageId;
        public GameObject panel;  // �A�N�e�B�u�ɂ�����UI�p�l��
    }

    public StageUnlockPanel[] stagePanels;

    void Start()
    {
        foreach (var entry in stagePanels)
        {
            bool cleared = GameManager.Instance.IsStageCleared(entry.stageId);
            entry.panel.SetActive(cleared);  // �N���A���Ă����ON�A���ĂȂ����OFF
        }
    }
}