using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMode : MonoBehaviour
{
    public GameObject object_Create_Mode_Area;
    private bool isActive = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            isActive = !isActive; // ó‘Ô‚ğƒgƒOƒ‹
            object_Create_Mode_Area.SetActive(isActive); // ó‘Ô‚É‡‚í‚¹‚Ä•\¦Ø‚è‘Ö‚¦
        }
    }
}