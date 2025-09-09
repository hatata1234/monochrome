using UnityEngine;

public class ShowUIOnClick : MonoBehaviour
{
    public GameObject targetUI; // �\��������UI�i��FPanel�j
    public GameObject PauseUI; // �\��������UI�i��FPanel�j
    public GameObject TutorialUI; // �\��������UI�i��FPanel�j
    public GameObject OperateUI; // �\��������UI�i��FPanel�j
    public GameObject SoundUI; // �\��������UI�i��FPanel�j

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

}
