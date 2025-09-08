using UnityEngine;

public class TargetObject_SetActive : MonoBehaviour
{
    public GameObject objectToToggleOnGoalContact; // Goal接触時に表示切替するオブジェクト（使うなら）
    public GameObject Damege_Floor;                // 非表示/再表示するオブジェクト
    public bool isActive = true;                   // 初期状態（true: 表示, false: 非表示）

    private void Start()
    {
        // 初期状態に設定
        if (Damege_Floor != null)
        {
            Damege_Floor.SetActive(isActive);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Code"))
        {
            if (Damege_Floor != null)
            {
                Damege_Floor.SetActive(false);
                Debug.Log("Damege_Floor 非表示");
            }

            if (objectToToggleOnGoalContact != null)
            {
                objectToToggleOnGoalContact.SetActive(!isActive);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Code"))
        {
            if (Damege_Floor != null)
            {
                Damege_Floor.SetActive(true);
                Debug.Log("Damege_Floor 表示");
            }

            if (objectToToggleOnGoalContact != null)
            {
                objectToToggleOnGoalContact.SetActive(isActive);
            }
        }
    }
}