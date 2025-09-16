using UnityEngine;

public class CreateMode : MonoBehaviour
{
    public GameObject object_Create_Mode_Area;

    // 複数の GameObject をアサインできるように List に変更
    public GameObject[] Hide_Objects;

    private bool isActive = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            isActive = !isActive;

            // 1. モード切り替え用オブジェクトの表示
            object_Create_Mode_Area.SetActive(isActive);

            // 2. 複数オブジェクトの表示切り替え
            foreach (GameObject obj in Hide_Objects)
            {
                ToggleVisibility(obj, !isActive);
            }
        }
    }

    void ToggleVisibility(GameObject obj, bool visible)
    {
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in renderers)
        {
            r.enabled = visible;
        }
    }
}
