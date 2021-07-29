using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hp_Zero_fill_Black : MonoBehaviour
{
    private GameObject hp_bar; //캐릭터 밑의 hp 바
    private GameObject fill; //hp 바 배경

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

    void hp_bar_fill_black_in_zero() //hp가 0이 되었을때 시스템상 남는 더미 hp가 보이지 않게, 슬라이더의 배경을 검게 칠해버리는 함수.
    {
        if(hp_bar.GetComponent<Slider>().value ==0)
        {
            fill.GetComponent<Image>().color = Color.black;
        }
    }
}
