
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
            isActive = !isActive; // ó‘Ô‚ğƒgƒOƒ‹
            object_Create_Mode_Area.SetActive(isActive); // ó‘Ô‚É‡‚í‚¹‚Ä•\¦Ø‚è‘Ö‚¦
            object1.SetActive(!isActive);
        }
    }
}