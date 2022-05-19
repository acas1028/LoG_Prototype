using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSolution : MonoBehaviour
{
    GameObject[] backGround;

    public bool is_Pve;

    private void Awake()
    {
        backGround = GameObject.FindGameObjectsWithTag("BackGround");

    }

    private void Start()
    {
        
        for (int i = 0; i < backGround.Length; i++)
        {
            backGround[i].GetComponent<RectTransform>().sizeDelta = new Vector2(GameObject.Find("Canvas").GetComponent<RectTransform>().rect.width,  GameObject.Find("Canvas").GetComponent<RectTransform>().rect.height);
        }
       

    }
}
