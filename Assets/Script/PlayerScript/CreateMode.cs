
using UnityEngine;

public class CreateMode : MonoBehaviour
{
    public GameObject object_Create_Mode_Area;
    public GameObject object1;
    private bool isActive = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            isActive = !isActive; // ��Ԃ��g�O��
            object_Create_Mode_Area.SetActive(isActive); // ��Ԃɍ��킹�ĕ\���؂�ւ�
            object1.SetActive(!isActive);
        }
    }
}