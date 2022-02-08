using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PveScebe_Map_ScaleScript : MonoBehaviour
{
    private Camera cam;

    private GameObject[] backGround;

    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        backGround = GameObject.FindGameObjectsWithTag("BackGround");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Set_Pve_BackGround_Scale()
    {
        var vertExtent = cam.orthographicSize;
        var horzExtent = cam.aspect * vertExtent;

        for(int i = 0; i < backGround.Length;i++)
        {
            var horRatio = backGround[i].GetComponent<SpriteRenderer>().bounds.size.x / 2;
            var vertRatio = backGround[i].GetComponent<SpriteRenderer>().bounds.size.y / 2;

            backGround[i].transform.localScale = new Vector3(horzExtent / horRatio, vertExtent / vertRatio, 1);
        }



    }
}
