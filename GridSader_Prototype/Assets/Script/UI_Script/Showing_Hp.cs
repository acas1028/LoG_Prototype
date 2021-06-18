using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Showing_Hp : MonoBehaviour
{
    private GameObject character; //ĳ���� ��Ī

    private int character_full_hp;//ĳ������ ���� �ִ� ��

    private Slider hp_slider;//ĳ���� hp��

    private void Start()
    {
        character = transform.parent.gameObject;
        hp_slider = this.GetComponent<Slider>();
    }




    private void Update()
    {
        //hp�ٸ� ĳ���� �ؿ� ��ȯ�ϱ⸸ �ϸ� ��
        Full_Hp();
        Hp_Change();
    }

    void Full_Hp()//ĳ���� full hp���� ������ ���� ���� �Լ��� full hp�� 0���� ���۵ǰ� �Ǹ� slider ��ü�� ������ ���� ���� �Լ�
    {
        if(character_full_hp==0)
        {
            character_full_hp = character.GetComponent<Character_Script>().character_HP;
        }
    }

    void Hp_Change()// hp��ȭ�� �����ִ� �Լ�
    {
        if (character_full_hp != 0)
        {
            hp_slider.value = (float)character.GetComponent<Character_Script>().character_HP / character_full_hp;
        }
    }



    

  

}
