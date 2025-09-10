using UnityEngine;

public class TargetObject_SetActive : MonoBehaviour
{
    public GameObject[] gameObjects; // 対象オブジェクト
    private bool[] originalStates;   // 各オブジェクトの初期状態を記録

    private void Start()
    {
        // 初期状態の保存
        originalStates = new bool[gameObjects.Length];
        for (int i = 0; i < gameObjects.Length; i++)
        {
            if (gameObjects[i] != null)
                originalStates[i] = gameObjects[i].activeSelf;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Code"))
        {
            for (int i = 0; i < gameObjects.Length; i++)
            {
                if (gameObjects[i] != null)
                {
                    // 状態を反転
                    gameObjects[i].SetActive(!gameObjects[i].activeSelf);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Code"))
        {
            for (int i = 0; i < gameObjects.Length; i++)
            {
                if (gameObjects[i] != null)
                {
                    // 元の状態に戻す
                    gameObjects[i].SetActive(originalStates[i]);
                }
            }
        }
    }
}