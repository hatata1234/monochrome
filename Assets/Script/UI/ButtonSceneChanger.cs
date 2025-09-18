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

    public void Stage1()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void Stage2()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void Stage3()
    {
        SceneManager.LoadScene("GameScene");
    }
}