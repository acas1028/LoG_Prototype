using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blink_text_script : MonoBehaviour
{
    public Text starting_text;

    Color present_Color;
    public float time;
    public float present_Color_a;
    bool is_full;
    bool is_Max_or_MIN;

    private void Start()
    {
        time = 0;
        is_full = false;
        is_Max_or_MIN = false;
    }
    private void Update()
    {
        Text_Blink();
        Is_Max_or_Min();
        Time_limit();
    }

    void Text_Blink()
    {
        if(is_full==false &&time>0.1)
        {
            Debug.Log(starting_text.color.a);
            present_Color_a = starting_text.color.a;
            present_Color_a--;
            present_Color = new Color(starting_text.color.r/255f, starting_text.color.g/255f, starting_text.color.b/255f, present_Color_a/255f);
            starting_text.color = present_Color;
            time = 0;
        }

        else if(is_full==true && time>0.1)
        {
            Debug.Log(starting_text.color.a);
            present_Color_a = starting_text.color.a;
            present_Color_a++;
            present_Color = new Color(starting_text.color.r/255f, starting_text.color.g/255f, starting_text.color.b/255f, present_Color_a/255f);
            starting_text.color = present_Color;
            time = 0;
        }

    }

    void Is_Max_or_Min()
    {
        if(starting_text.color.a<= 0)
        {
            is_full = true;
        }

        else if(starting_text.color.a>=254)
        {
            is_full = false;
        }
    }

    void Time_limit()
    {
        if(time<0.1)
        {
            time += Time.deltaTime;
        }
    }
}
