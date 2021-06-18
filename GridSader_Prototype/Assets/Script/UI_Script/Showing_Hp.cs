using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Showing_Hp : MonoBehaviour
{
    private GameObject character; //캐릭터 지칭

    private int character_full_hp;//캐릭터의 본래 최대 피

    private Slider hp_slider;//캐릭터 hp바

    private void Start()
    {
        character = transform.parent.gameObject;
        hp_slider = this.GetComponent<Slider>();
    }




    private void Update()
    {
        //hp바를 캐릭터 밑에 소환하기만 하면 끝
        Full_Hp();
        Hp_Change();
    }

    void Full_Hp()//캐릭터 full hp관련 오류가 생겨 만든 함수로 full hp가 0으로 시작되게 되면 slider 자체에 오류가 생겨 만든 함수
    {
        if(character_full_hp==0)
        {
            character_full_hp = character.GetComponent<Character_Script>().character_HP;
        }
    }

    void Hp_Change()// hp변화를 보여주는 함수
    {
        if (character_full_hp != 0)
        {
            hp_slider.value = (float)character.GetComponent<Character_Script>().character_HP / character_full_hp;
        }
    }



    

  

}
