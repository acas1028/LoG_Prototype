using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundScaleScript : MonoBehaviour
{
    
    private Camera cam;
    
    private GameObject backGround;

    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        backGround = GameObject.FindGameObjectWithTag("BackGround");
        Set_BackGround_Scale();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Set_BackGround_Scale()
    {
        var vertExtent = cam.orthographicSize;
        var horzExtent = cam.aspect * vertExtent;

        var horRatio = backGround.GetComponent<SpriteRenderer>().bounds.size.x/2;
        var vertRatio = backGround.GetComponent<SpriteRenderer>().bounds.size.y/2;

        backGround.transform.localScale = new Vector3(horzExtent / horRatio, vertExtent / vertRatio, 1);
        


    }
}
