using UnityEngine;

public class TouchConditionManager : MonoBehaviour
{
    public ColliderChecker[] checkers;

    private bool conditionAchieved = false;
    public GameObject gameObjects; // �ΏۃI�u�W�F�N�g

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
        Debug.Log("�����B���I���ׂẴR���C�_�[�ɐG��Ă��܂��I");
        // �����ɃC�x���g�����i��F�����J����A�p�Y���������o�Ȃǁj
        gameObjects.SetActive(false);
    }
}