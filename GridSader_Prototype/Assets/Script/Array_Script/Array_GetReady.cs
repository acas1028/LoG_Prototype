using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Array_GetReady : MonoBehaviour
{
    public bool get_Ready = false;

    private void OnDisable()
    {
        get_Ready = true;
    }
}
