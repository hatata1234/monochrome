using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_SetActive : MonoBehaviour
{
    public GameObject[] gameObjects; // 表示するUIなど
    private bool[] originalStates;
    public float reactCooldown = 2f; // 再反応までの待ち時間
    private bool canReact = true;

    private void Start()
    {
        originalStates = new bool[gameObjects.Length];
        for (int i = 0; i < gameObjects.Length; i++)
        {
            if (gameObjects[i] != null)
                originalStates[i] = gameObjects[i].activeSelf;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canReact && collision.CompareTag("Player"))
        {
            ToggleUI(true); // UIを表示
            canReact = false;
            StartCoroutine(ReactCooldown());
        }
    }

    // UIを開いたり閉じたりする
    public void ToggleUI(bool isOpen)
    {
        for (int i = 0; i < gameObjects.Length; i++)
        {
            if (gameObjects[i] != null)
                gameObjects[i].SetActive(isOpen);
        }

        // UIを開いてる間は時間停止
        Time.timeScale = isOpen ? 0f : 1f;
    }

    // UIの閉じるボタンから呼ばれる
    public void OnCloseButton()
    {
        ToggleUI(false);
    }

    // 一定時間経つまで再反応できないようにする
    private IEnumerator ReactCooldown()
    {
        yield return new WaitForSecondsRealtime(reactCooldown); // TimeScaleの影響を受けない
        canReact = true;
    }
}
