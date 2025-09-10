using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public static bool IsPaused { get; private set; }

    public GameObject pauseMenuUI;
    public GameObject TutorialUI;
    public GameObject OperateUI;
    public GameObject SoundUI;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            if (TutorialUI.activeSelf || OperateUI.activeSelf || SoundUI.activeSelf )
            {
                if (TutorialUI.activeSelf) CloseTutorial();
                if (OperateUI.activeSelf) CloseOperate();
                if (SoundUI.activeSelf) CloseSound();
            }
            else
            {
                if (IsPaused)
                    Resume();
                else
                    Pause();
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

}