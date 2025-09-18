using UnityEngine;

public class MainMenuStageUnlocker : MonoBehaviour
{
    [System.Serializable]
    public struct StageUnlockPanel
    {
        public string stageId;
        public GameObject panel;  // アクティブにしたいUIパネル
    }

    public StageUnlockPanel[] stagePanels;

    void Start()
    {
        foreach (var entry in stagePanels)
        {
            bool cleared = GameManager.Instance.IsStageCleared(entry.stageId);
            entry.panel.SetActive(cleared);  // クリアしていればON、してなければOFF
        }
    }
}