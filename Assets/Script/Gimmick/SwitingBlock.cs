using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitingBlock : MonoBehaviour
{
    [Header("�͈͎w��")]
    [SerializeField] private Transform leftPoint;    // ���[
    [SerializeField] private Transform rightPoint;   // �E�[
    [SerializeField] private GameObject blockPrefab; // �o������u���b�N

    [Header("����ݒ�")]
    [SerializeField] private float moveInterval = 0.5f; // �_�ŊԊu�i�b�j
    [SerializeField] private int stepCount = 5;         // �������i���}�X�ɂ��邩�j

    private GameObject[] blocks;   // �z�u�����u���b�N�Q
    private int currentIndex = 0;  // ���A�N�e�B�u�Ȉʒu
    private int direction = 1;     // �i�s�����i1:�E / -1:���j
    private float timer = 0f;

    private void Start()
    {
        // �͈͂� stepCount �ɕ������ău���b�N�𐶐�
        blocks = new GameObject[stepCount];
        for (int i = 0; i < stepCount; i++)
        {
            Vector3 pos = Vector3.Lerp(leftPoint.position, rightPoint.position, (float)i / (stepCount - 1));
            blocks[i] = Instantiate(blockPrefab, pos, Quaternion.identity, transform);
            blocks[i].SetActive(false); // �ŏ��͔�\��
        }
        blocks[currentIndex].SetActive(true);
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= moveInterval)
        {
            timer = 0f;

            // ���̃u���b�N��OFF
            blocks[currentIndex].SetActive(false);

            // ���̈ʒu��
            currentIndex += direction;

            // �[�ɒ������甽�]
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

            // ���̃u���b�N��ON
            blocks[currentIndex].SetActive(true);
        }
    }
}