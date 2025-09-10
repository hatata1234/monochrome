using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class TeleportGimmick : MonoBehaviour
{
    public Transform targetPoint;
 

    public void toTeleport()
    {
        GameObject player = GameObject.FindWithTag("Player");

        if (player != null)
        {
            if ( targetPoint != null)
            {
                player.transform.position = targetPoint.position;
            }
        }
        //�t�H�[�J�X����
        EventSystem.current.SetSelectedGameObject(null);
    }
}