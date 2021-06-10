using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Receive_Array_Data : MonoBehaviour
{
    public GameObject[] Receive_Character;

    private void Awake()
    {
        Receive_Character = GameObject.FindGameObjectsWithTag("Character");
        for (int i = 0; i < Receive_Character.Length; i++)
        {

        }
    }
}
