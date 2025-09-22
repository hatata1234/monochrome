using UnityEngine;

public class ShowUIOnClick : MonoBehaviour
{
    public GameObject targetUI; // �\��������UI�i��FPanel�j
    public GameObject PauseUI; // �\��������UI�i��FPanel�j
    public GameObject TutorialUI; // �\��������UI�i��FPanel�j
    public GameObject TutorialUI2; // �\��������UI�i��FPanel�j
    public GameObject OperateUI; // �\��������UI�i��FPanel�j
    public GameObject SoundUI; // �\��������UI�i��FPanel�j

    //�|�[�Y��ʂ̃`���[�g���A���ꗗ
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
    public void ShowTutorial2()
    {
        TutorialUI2.SetActive(true);

    }
    public void CloseTutorial2()
    {
        TutorialUI2.SetActive(false);
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
