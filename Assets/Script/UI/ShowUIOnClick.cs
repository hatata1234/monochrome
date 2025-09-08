using UnityEngine;

public class ShowUIOnClick : MonoBehaviour
{
    public GameObject targetUI; // 表示したいUI（例：Panel）
    public GameObject PauseUI; // 表示したいUI（例：Panel）

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
    
  
}
