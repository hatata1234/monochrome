using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Timeline;

public class ElectricGimmick : MonoBehaviour
{
    private const string TAG_CONNECTION = "Connection";
    private const string TAG_INSULATOR = "Insulator";
    private const string TAG_DEVICE = "Device";
    private const string TAG_CODE = "Code";

    [Header("通電判定")]
    public bool isPowered = false;

    [Header("隣接判定の範囲（ユニット単位）")]
    public float checkRadius = 1.1f;

    [Header("消滅対象※タグが「Device」のものだけ設定")]
    public GameObject[] devicesToDeactivate; // ← 複数対応に変更

    

    // 更新間隔（秒）
    public float updateInterval = 0.2f;
    private float timer = 0f;
    public bool isActive;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= updateInterval)
        {
            timer = 0f;

            bool connected = IsConnectedToPowerSourceBFS();

            if (connected != isPowered)
            {
                isPowered = connected;

                if (isPowered && tag != "Insulator")
                    ActivateDevice();
                else
                    DeactivateDevice();
            }
        }
    }

    private bool IsConnectedToPowerSourceBFS()
    {
        if (CompareTag(TAG_CONNECTION))
            return true;

        HashSet<GameObject> visited = new HashSet<GameObject>();
        Queue<GameObject> queue = new Queue<GameObject>();

        visited.Add(gameObject);
        queue.Enqueue(gameObject);

        while (queue.Count > 0)
        {
            GameObject current = queue.Dequeue();

            Collider2D[] hits = Physics2D.OverlapCircleAll(current.transform.position, checkRadius);

            foreach (var hit in hits)
            {
                GameObject other = hit.gameObject;
                if (visited.Contains(other)) continue;

                Vector2 dir = other.transform.position - current.transform.position;
                dir = new Vector2(Mathf.Round(dir.x), Mathf.Round(dir.y));
                if (Mathf.Abs(dir.x) + Mathf.Abs(dir.y) != 1) continue; // 隣接のみ

                if (other.CompareTag(TAG_INSULATOR)) continue;

                if (other.CompareTag(TAG_CONNECTION))
                    return true;

                ElectricGimmick otherPowerWire = other.GetComponent<ElectricGimmick>();
                if (otherPowerWire != null)
                {
                    visited.Add(other);
                    queue.Enqueue(other);
                }
            }
        }

        return false;
    }

    private void ActivateDevice()
    {
        if (CompareTag(TAG_DEVICE))
        {
            //GetComponent<SpriteRenderer>().color = new Color32(0, 140, 255, 255);

            if (devicesToDeactivate != null)
            {
                foreach (GameObject obj in devicesToDeactivate)
                {
                    if (obj != null)
                        obj.SetActive(!isActive);
                }
            }

            
        }
        else if (CompareTag(TAG_CONNECTION))
        {
           // GetComponent<SpriteRenderer>().color = new Color32(80, 255, 70, 255);
        }
        else
        {
           // GetComponent<SpriteRenderer>().color = new Color32(255, 255, 60, 255);
        }
    }

    private void DeactivateDevice()
    {
        if (CompareTag(TAG_DEVICE))
        {
          // GetComponent<SpriteRenderer>().color = new Color32(45, 45, 82, 255);

            if (devicesToDeactivate != null)
            {
                foreach (GameObject obj in devicesToDeactivate)
                {
                    if (obj != null)
                        obj.SetActive(isActive);
                }
            }

            
        }
        else if (CompareTag(TAG_CODE))
        {
           // GetComponent<SpriteRenderer>().color = Color.gray;
        }
    }
}


