using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolObject : MonoBehaviour
{
    [Header("�p�g���[���|�C���g�i���W�j")]
    public Vector2 pointA;
    public Vector2 pointB;

    [Header("�ړ����x")]
    public float speed = 2f;

    private Vector2 currentTarget;
    private ObjectPlacer2D placer;

    private int bounceCount = 0; // ���˕Ԃ�񐔃J�E���g

    void Start()
    {
        currentTarget = pointB;
        placer = ObjectPlacer2D.FindPlacerById("Placer_1");

        // �����ʒu��pointA�ɃZ�b�g
        transform.position = pointA;

        // �����̌�����pointB�����ɍ��킹�Đݒ�
        SetFacingDirection(currentTarget - (Vector2)transform.position);
    }

    void Update()
    {
        if (placer == null) return;

        Vector2 nextGridPos = GetNextGridPos();

        // �i�s�����Ƀu���b�N����������������]
        if (placer.IsBlockAtGrid(nextGridPos))
        {
            Debug.Log($"�u���b�N���o: {nextGridPos}�B�����𔽓]���܂��B");

            currentTarget = (currentTarget == pointB) ? pointA : pointB;
            SetFacingDirection(currentTarget - (Vector2)transform.position);

            bounceCount++;  // ���˕Ԃ�񐔂𑝂₷

            if (bounceCount >= 2)
            {
                Debug.Log("2�񒵂˕Ԃ������߃I�u�W�F�N�g���\���ɂ��܂��B");
                gameObject.SetActive(false); // �I�u�W�F�N�g��\��
                return; // �ȍ~�����𒆒f
            }
        }

        // �ړI�n�ֈړ�
        transform.position = Vector3.MoveTowards(transform.position, currentTarget, speed * Time.deltaTime);

        // �ړI�n�ɓ��B������^�[�Q�b�g��؂�ւ���i���ꂪ�����Ɖ������Ȃ��j
        if ((Vector2)transform.position == currentTarget)
        {
            currentTarget = (currentTarget == pointB) ? pointA : pointB;
            SetFacingDirection(currentTarget - (Vector2)transform.position);
        }
    }


    // ���ɐi�ރO���b�h�ʒu���v�Z�i0.5���݂ŃO���b�h�ɍ��킹��j
    Vector2 GetNextGridPos()
    {
        Vector2 direction = (currentTarget - (Vector2)transform.position).normalized;
        Vector2 nextPos = (Vector2)transform.position + direction * 0.5f;
        return new Vector2(Mathf.Floor(nextPos.x) + 0.5f, Mathf.Floor(nextPos.y) + 0.5f);
    }

    // �����ڂ̌��������[�e�[�V�����Ő؂�ւ�
    void SetFacingDirection(Vector2 direction)
    {
        if (direction.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);  // �E�����i�f�t�H���g�j
        }
        else if (direction.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0); // �������iY��180�x��]�Ŕ��]�j
        }
    }
}
