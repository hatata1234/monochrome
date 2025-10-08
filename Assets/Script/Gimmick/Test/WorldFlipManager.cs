using UnityEngine;
using System;

public class WorldFlipManager : MonoBehaviour
{
    public static WorldFlipManager Instance { get; private set; }

    public bool IsBlackWorld { get; private set; } = false; // 初期は白の世界（false）

    // イベント：世界が反転したときに通知（他スクリプトが反応できるように）
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
        // 「1」キーを押したら FlipWorld() を実行
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            FlipWorld();
        }
    }

    public void FlipWorld()
    {
        IsBlackWorld = !IsBlackWorld;

        // 背景色切り替え（例）
        if (mainCamera != null)
        {
            mainCamera.backgroundColor = IsBlackWorld ? Color.black : Color.white;
        }

        Debug.Log($"世界が {(IsBlackWorld ? "黒" : "白")} に反転しました");

        // 反転イベントを通知（プレイヤーやギミックはこれを受けて色や性質を変更）
        OnWorldFlipped?.Invoke(IsBlackWorld);
    }
}
