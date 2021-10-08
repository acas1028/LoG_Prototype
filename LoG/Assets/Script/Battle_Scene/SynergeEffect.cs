using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynergeEffect : MonoBehaviour
{
    // Start is called before the first frame update
    // 0 : nothing 1: ������(��) 2: �����(��)  3: �뷱����(��)

    public GameObject RedEffect;
    public GameObject BlueEffect;
    public GameObject GreenEffect;


    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Insert(int a1,int a2,int a3,int lineNum)
    {
        switch(a1)
        {
            case 0:
                break;
            case 1:
                RedEffect = Instantiate(RedEffect, new Vector3(-75, 75 - (lineNum * 75) , 0), Quaternion.identity);
                RedEffect.transform.parent = gameObject.transform;
                RedEffect.transform.localPosition = new Vector3(-75, 75 - (lineNum * 75));
                break;
            case 2:
                BlueEffect = Instantiate(BlueEffect, new Vector3(-75, 75 - (lineNum * 75), 0), Quaternion.identity);
                BlueEffect.transform.parent = gameObject.transform;
                BlueEffect.transform.localPosition = new Vector3(-75, 75 - (lineNum * 75), 0);
                break;
            case 3:
                GreenEffect = Instantiate(GreenEffect, new Vector3(-75, 75 - (lineNum * 75), 0), Quaternion.identity);
                GreenEffect.transform.parent = gameObject.transform;
                GreenEffect.transform.localPosition = new Vector3(-75, 75 - (lineNum * 75), 0);
                break;
        }

        switch (a2)
        {
            case 0:
                break;
            case 1:
                RedEffect = Instantiate(RedEffect, new Vector3(0, 75 - (lineNum * 75), 0), Quaternion.identity) as GameObject;
                RedEffect.transform.parent = gameObject.transform;
                RedEffect.transform.localPosition = new Vector3(0, 75 - (lineNum * 75));
                break;
            case 2:
                BlueEffect = Instantiate(BlueEffect, new Vector3(0, 75 - (lineNum * 75), 0), Quaternion.identity) as GameObject;
                BlueEffect.transform.parent = gameObject.transform;
                BlueEffect.transform.localPosition = new Vector3(0, 75 - (lineNum * 75), 0);
                break;
            case 3:
                GreenEffect = Instantiate(GreenEffect, new Vector3(0, 75 - (lineNum * 75), 0), Quaternion.identity) as GameObject;
                GreenEffect.transform.parent = gameObject.transform;
                GreenEffect.transform.localPosition = new Vector3(0, 75 - (lineNum * 75), 0);
                break;
        }

        switch (a3)
        {
            case 0:
                break;
            case 1:
                RedEffect = Instantiate(RedEffect, new Vector3(75, 75 - (lineNum * 75), 0), Quaternion.identity) as GameObject;
                RedEffect.transform.parent = gameObject.transform;
                RedEffect.transform.localPosition = new Vector3(75, 75 - (lineNum * 75));
                break;
            case 2:
                BlueEffect = Instantiate(BlueEffect, new Vector3(75, 75 - (lineNum * 75), 0), Quaternion.identity) as GameObject;
                BlueEffect.transform.parent = gameObject.transform;
                BlueEffect.transform.localPosition = new Vector3(75, 75 - (lineNum * 75), 0);
                break;
            case 3:
                GreenEffect = Instantiate(GreenEffect, new Vector3(75, 75 - (lineNum * 75), 0), Quaternion.identity) as GameObject;
                GreenEffect.transform.parent = gameObject.transform;
                GreenEffect.transform.localPosition = new Vector3(75, 75 - (lineNum * 75), 0);
                break;
        }


    }


}