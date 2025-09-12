using UnityEngine;

public class TargetObject_SetActive : MonoBehaviour
{
    public GameObject[] gameObjects; // 対象オブジェクト
    private bool[] originalStates;   // 各オブジェクトの初期状態を記録

    [Tooltip("このタグを持つオブジェクトがトリガーに入ったときに処理されます")]
    public string targetTag = "Code";

    [Header("押すと非表示になるキー")]
    public KeyCode keyToPress = KeyCode.E;

    // ← 接触中かどうかをフラグで管理
    private bool isPlayerInTrigger = false;

    private void Start()
    {
        originalStates = new bool[gameObjects.Length];
        for (int i = 0; i < gameObjects.Length; i++)
        {
            if (gameObjects[i] != null)
                originalStates[i] = gameObjects[i].activeSelf;
        }
    }

    private void Update()
    {
        // 接触中かつキーが押されたら、自分を非アクティブにする
        if (/*isPlayerInTrigger &&*/ Input.GetKeyDown(keyToPress))
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(targetTag))
        {
            isPlayerInTrigger = true;

            for (int i = 0; i < gameObjects.Length; i++)
            {
                if (gameObjects[i] != null)
                {
                    gameObjects[i].SetActive(!gameObjects[i].activeSelf);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(targetTag))
        {
            isPlayerInTrigger = false;

            for (int i = 0; i < gameObjects.Length; i++)
            {
                if (gameObjects[i] != null)
                {
                    gameObjects[i].SetActive(originalStates[i]);
                }
            }
        }
    }
}