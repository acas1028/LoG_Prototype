using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hp_Zero_fill_Black : MonoBehaviour
{
    private GameObject hp_bar;
    private GameObject fill;

    void Start()
    {
        hp_bar = transform.parent.gameObject;
        fill = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        hp_bar_fill_black_in_zero();
    }

    void hp_bar_fill_black_in_zero()
    {
        if(hp_bar.GetComponent<Slider>().value ==0)
        {
            fill.GetComponent<Image>().color = Color.black;
        }
    }
}
