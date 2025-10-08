using UnityEngine;
using System;

public class WorldFlipManager : MonoBehaviour
{
    public static WorldFlipManager Instance { get; private set; }

    public bool IsBlackWorld { get; private set; } = false; // �����͔��̐��E�ifalse�j

    // �C�x���g�F���E�����]�����Ƃ��ɒʒm�i���X�N���v�g�������ł���悤�Ɂj
    public event Action<bool> OnWorldFlipped;

    private Camera mainCamera;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        mainCamera = Camera.main;
    }
    private void Update()
    {
        // �u1�v�L�[���������� FlipWorld() �����s
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            FlipWorld();
        }
    }

    public void FlipWorld()
    {
        IsBlackWorld = !IsBlackWorld;

        // �w�i�F�؂�ւ��i��j
        if (mainCamera != null)
        {
            mainCamera.backgroundColor = IsBlackWorld ? Color.black : Color.white;
        }

        Debug.Log($"���E�� {(IsBlackWorld ? "��" : "��")} �ɔ��]���܂���");

        // ���]�C�x���g��ʒm�i�v���C���[��M�~�b�N�͂�����󂯂ĐF�␫����ύX�j
        OnWorldFlipped?.Invoke(IsBlackWorld);
    }
}
