using UnityEngine;
using UnityEngine.UI;

public class Button_Teleport : MonoBehaviour
{
    [Header("テレポートさせたい対象（例：Player）")]
    public Transform target;

    [Header("テレポート先の位置")]
    public Transform teleportDestination;

    void Start()
    {
        // ボタンコンポーネントを取得してクリックイベントを登録
        GetComponent<Button>().onClick.AddListener(Teleport);
    }

    void Teleport()
    {
        if (target != null && teleportDestination != null)
        {
            target.position = teleportDestination.position;
        }
        else
        {
            Debug.LogWarning("ターゲットまたはテレポート先が設定されていません！");
        }
    }
}
