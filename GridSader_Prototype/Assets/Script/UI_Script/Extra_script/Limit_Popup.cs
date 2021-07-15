using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Limit_Popup : MonoBehaviour
{
    GameObject[] Popup;

    GameObject canvas;

    int Popup_count;

    private void Start()
    {
        canvas = GameObject.Find("Canvas");
        Popup_count = 0;
    }

    void Popup_off_in_multi()
    {
        if(Input.GetMouseButtonUp(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 1000);
            if (!hit)
                return;

            if(hit.transform.tag != Popup[Popup_count].tag)
            {
                //Popup[Popup_count].
            }





        }
    }

    

    void Present_popup_setActive_true()
    {
        for (int i = 0; i < Popup.Length; i++)
        {
            if (Popup[i].activeSelf == true)
            {
                Popup_count = i;
            }
        }
    }


    public Vector3 worldToUISpace(Canvas parentCanvas, Vector3 worldPos) //캔버스의 포지션과 월드의 포지션의 통로 역할을 해주는 함수
    {
        //Convert the world for screen point so that it can be used with ScreenPointToLocalPointInRectangle function
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        Vector2 movePos;

        //Convert the screenpoint to ui rectangle local point
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentCanvas.transform as RectTransform, screenPos, parentCanvas.worldCamera, out movePos);
        //Convert the local point to world point
        return parentCanvas.transform.TransformPoint(movePos);
    }
}
