using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonSceneChanger : MonoBehaviour
{
    public void ChangeGameScene()
    {
        SceneManager.LoadScene("TestScene");
    }
    public void ChangeMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}