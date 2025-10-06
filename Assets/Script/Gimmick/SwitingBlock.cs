using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitingBlock : MonoBehaviour
{
    [Header("範囲指定")]
    [SerializeField] private Transform leftPoint;    // 左端
    [SerializeField] private Transform rightPoint;   // 右端
    [SerializeField] private GameObject blockPrefab; // 出現するブロック

    [Header("動作設定")]
    [SerializeField] private float moveInterval = 0.5f; // 点滅間隔（秒）
    [SerializeField] private int stepCount = 5;         // 分割数（何マスにするか）

    private GameObject[] blocks;   // 配置したブロック群
    private int currentIndex = 0;  // 今アクティブな位置
    private int direction = 1;     // 進行方向（1:右 / -1:左）
    private float timer = 0f;

    private void Start()
    {
        // 範囲を stepCount に分割してブロックを生成
        blocks = new GameObject[stepCount];
        for (int i = 0; i < stepCount; i++)
        {
            Vector3 pos = Vector3.Lerp(leftPoint.position, rightPoint.position, (float)i / (stepCount - 1));
            blocks[i] = Instantiate(blockPrefab, pos, Quaternion.identity, transform);
            blocks[i].SetActive(false); // 最初は非表示
        }
        blocks[currentIndex].SetActive(true);
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= moveInterval)
        {
            timer = 0f;

            // 今のブロックをOFF
            blocks[currentIndex].SetActive(false);

            // 次の位置へ
            currentIndex += direction;

            // 端に着いたら反転
            if (currentIndex >= stepCount)
            {
                currentIndex = stepCount - 2;
                direction = -1;
            }
            else if (currentIndex < 0)
            {
                currentIndex = 1;
                direction = 1;
            }

            // 次のブロックをON
            blocks[currentIndex].SetActive(true);
        }
    }
}