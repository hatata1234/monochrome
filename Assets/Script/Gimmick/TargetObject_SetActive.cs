using UnityEngine;

public class TargetObject_SetActive : MonoBehaviour
{
    public GameObject game_object; // 接触時に表示切替するオブジェクト
    public bool isActive = true;                   // 初期状態（true: 表示, false: 非表示）

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Code"))
        {
            game_object.SetActive(!isActive);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Code"))
        {
            game_object.SetActive(isActive);
        }
    }
}