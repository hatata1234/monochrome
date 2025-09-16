using UnityEngine;

public class TouchConditionManager : MonoBehaviour
{
    public ColliderChecker[] checkers;

    private bool conditionAchieved = false;
    public GameObject gameObjects; // 対象オブジェクト

    void Update()
    {
        if (conditionAchieved) return;

        bool allTouching = true;

        foreach (var checker in checkers)
        {
            if (!checker.isTouching)
            {
                allTouching = false;
                break;
            }
        }

        if (allTouching)
        {
            conditionAchieved = true;
            OnConditionAchieved();
        }
    }

    private void OnConditionAchieved()
    {
        Debug.Log("条件達成！すべてのコライダーに触れています！");
        // ここにイベント処理（例：扉を開ける、パズル成功演出など）
        gameObjects.SetActive(false);
    }
}