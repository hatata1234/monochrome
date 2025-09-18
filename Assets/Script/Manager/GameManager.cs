using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        if (Timer.Instance != null)
        {
            Timer.Instance.StartTimer();
        }
    }
}
