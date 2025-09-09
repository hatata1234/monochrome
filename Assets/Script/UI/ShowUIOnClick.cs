using UnityEngine;

public class ShowUIOnClick : MonoBehaviour
{
    public GameObject targetUI; // 表示したいUI（例：Panel）
    public GameObject PauseUI; // 表示したいUI（例：Panel）
    public GameObject TutorialUI; // 表示したいUI（例：Panel）
    public GameObject OperateUI; // 表示したいUI（例：Panel）
    public GameObject SoundUI; // 表示したいUI（例：Panel）

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
