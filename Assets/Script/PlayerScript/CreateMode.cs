using UnityEngine;

public class CreateMode : MonoBehaviour
{
    public GameObject object_Create_Mode_Area;

    // ������ GameObject ���A�T�C���ł���悤�� List �ɕύX
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

            // ObjectPlacer2D ��T���i�V�[���ɕ�������ꍇ��ID�ōi���Ă�OK�j
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

    //        // 1. ���[�h�؂�ւ��p�I�u�W�F�N�g�̕\��
    //        object_Create_Mode_Area.SetActive(isActive);

    //        // 2. �����I�u�W�F�N�g�̕\���؂�ւ�
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
