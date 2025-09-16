using UnityEngine;

public class ShowUIOnClick : MonoBehaviour
{
    public GameObject targetUI; // 表示したいUI（例：Panel）
    public GameObject PauseUI; // 表示したいUI（例：Panel）
    public GameObject TutorialUI; // 表示したいUI（例：Panel）
    public GameObject OperateUI; // 表示したいUI（例：Panel）
    public GameObject SoundUI; // 表示したいUI（例：Panel）

    //ポーズ画面のチュートリアル一覧
    public GameObject TutorialsPanel;
    public GameObject Pause_TutorialUI1;
    public GameObject Pause_TutorialUI2;
    public GameObject Pause_TutorialUI3;

    public void ShowUI()
    {
        if (targetUI != null)
        {
            targetUI.SetActive(true);
        }
    }
    public void CloseUI()
    {
        if (targetUI != null)
        {
            Time.timeScale = 1;
            targetUI.SetActive(false);
        }
    }

    public void ShowPause()
    {
        PauseUI.SetActive(true);
        
    }
    public void ClosePause()
    {
            PauseUI.SetActive(false);
    }

    public void ShowTutorial()
    {
        TutorialUI.SetActive(true);

    }
    public void CloseTutorial()
    {
        TutorialUI.SetActive(false);
    }

    public void ShowOperate()
    {
        OperateUI.SetActive(true);

    }
    public void CloseOperate()
    {
        OperateUI.SetActive(false);
    }

    public void ShowSound()
    {
        SoundUI.SetActive(true);
    }

    public void CloseSound()
    {
        SoundUI.SetActive(false);
    }

    public void ShowTutorials()
    {
        TutorialsPanel.SetActive(true);
    }
    public void CloseTutorials()
    {
        TutorialsPanel.SetActive(false);
    }

    public void Pause_Tutorial1()
    {
        Pause_TutorialUI1.SetActive(true);
    }

    public void Pause_Tutorial2()
    {
        Pause_TutorialUI2.SetActive(true);
    }
    public void Pause_Tutorial3()
    {
        Pause_TutorialUI3.SetActive(true);
    }

    public void Close_Pause_Tutorials()
    {
        Pause_TutorialUI1.SetActive(false);
        Pause_TutorialUI2.SetActive(false);
        Pause_TutorialUI3.SetActive(false);
    }
}
