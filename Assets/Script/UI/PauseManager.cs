using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public static bool IsPaused { get; private set; }

    public GameObject pauseMenuUI;
    public GameObject TutorialUI;
    public GameObject OperateUI;
    public GameObject SoundUI;

    //TUTORIALMENU
    public GameObject TutorialUI1;
    public GameObject TutorialUI2;
    public GameObject TutorialUI3;

    public GameObject Tutorial_ClearUI1;
    public GameObject Tutorial_ClearUI2;
    public GameObject Tutorial_ClearUI3;

    //Retry
    public GameObject RetryUI;

    [SerializeField]private AudioClip pauseSE;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            if (TutorialUI.activeSelf || OperateUI.activeSelf || SoundUI.activeSelf || TutorialUI1.activeSelf || TutorialUI2.activeSelf || TutorialUI3.activeSelf 
                || Tutorial_ClearUI1.activeSelf || Tutorial_ClearUI2.activeSelf || Tutorial_ClearUI3.activeSelf || RetryUI.activeSelf)
            {
                if (TutorialUI.activeSelf) CloseTutorial();
                if (OperateUI.activeSelf) CloseOperate();
                if (SoundUI.activeSelf) CloseSound();
                if (TutorialUI1.activeSelf) CloseTutorials();
                if (TutorialUI2.activeSelf) CloseTutorials();
                if (TutorialUI3.activeSelf) CloseTutorials();
            }
            else
            {
                if (IsPaused)
                    Resume();
                else
                {
                    Pause();
                    //AudioManager.Instance.PlaySE(pauseSE);
                }     
            }
        }
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0;
        IsPaused = true;
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1;
        IsPaused = false;
    }

    public void RetryGame()
    {
        Time.timeScale = 1f;
        IsPaused = false;
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        IsPaused = false;
        SceneManager.LoadScene("MainMenu");
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

    public void CloseTutorials()
    {
        Time.timeScale = 1f;
        TutorialUI1.SetActive(false);
        TutorialUI2.SetActive(false);
        TutorialUI3.SetActive(false);
    }
    public void CloseClear()
    {
        Time.timeScale = 1f;
        Tutorial_ClearUI1.SetActive(false);
        Tutorial_ClearUI2.SetActive(false);
        Tutorial_ClearUI3.SetActive(false);
    }
}