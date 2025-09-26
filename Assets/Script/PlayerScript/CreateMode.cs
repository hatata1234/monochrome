using UnityEngine;

public class CreateMode : MonoBehaviour
{
    public GameObject object_Create_Mode_Area;

    // 複数の GameObject をアサインできるように List に変更
    public GameObject[] Hide_Objects;

    private bool isActive = false;
    public bool IsActive => isActive;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            isActive = !isActive;

            object_Create_Mode_Area.SetActive(isActive);

            foreach (GameObject obj in Hide_Objects)
            {
                ToggleVisibility(obj, !isActive);
            }

            // ObjectPlacer2D を探す（シーンに複数ある場合はIDで絞ってもOK）
            ObjectPlacer2D placer = FindObjectOfType<ObjectPlacer2D>();
            if (placer != null)
            {
                Color targetColor = isActive ? new Color(1f, 1f, 1f, 0.5f) : Color.black;
                placer.SetPlacedObjectsColor(targetColor);
            }
        }
    }

    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.E))
    //    {
    //        isActive = !isActive;

    //        // 1. モード切り替え用オブジェクトの表示
    //        object_Create_Mode_Area.SetActive(isActive);

    //        // 2. 複数オブジェクトの表示切り替え
    //        foreach (GameObject obj in Hide_Objects)
    //        {
    //            ToggleVisibility(obj, !isActive);
    //        }
    //    }
    //}

    void ToggleVisibility(GameObject obj, bool visible)
    {
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in renderers)
        {
            r.enabled = visible;
        }
    }
}
