using UnityEngine;

public class CreateMode : MonoBehaviour
{
    public GameObject object_Create_Mode_Area;

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

            ObjectPlacer2D placer = FindObjectOfType<ObjectPlacer2D>();
            if (placer != null)
            {
                Color targetColor;

                if (isActive)
                {
                    // グリッド表示中は灰色の半透明に固定
                    targetColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
                }
                else
                {
                    // 非表示時は世界色に合わせて通常色
                    bool isBlackWorld = WorldFlipManager.Instance != null && WorldFlipManager.Instance.IsBlackWorld;
                    targetColor = isBlackWorld ? Color.white : Color.black;
                }

                placer.SetPlacedObjectsColor(targetColor);
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
