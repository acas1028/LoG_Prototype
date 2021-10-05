using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectOnOff : MonoBehaviour
{
    public GameObject Object;

    public void Onoff()
    {
        if(Object.activeSelf==true)
        {
            Object.SetActive(false);
        }

        else
        {
            Object.SetActive(true);
        }
    }
}
