using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Showing_Hp : MonoBehaviour
{

    private float hp; //캐릭터 현재 hp
    private float original_hp; //캐릭터의 기본 hp
    private int is_this_original_hp_counting=0; // original_hp를 채웠는가를 묻는 변수
    private int hp_bar_y_position; // 캐릭터의 hp 바와 캐릭터과의 거리.
    private GameObject hp_bar; //hp 바
    private GameObject canvas; 

    // 아래의 변수는 inspector창에 변수를 설정해놓았다.
    public int character_count; // 같은 팀내의 캐릭터 구별 변수
    public int Team_count; //팀 구별 변수


    private void Start()
    {
        original_hp = 1;
        hp_bar_y_position = 175;
        canvas = this.gameObject.transform.parent.gameObject;
        hp_bar = this.gameObject;

    }

    void showing_Hp_point() //현재 hp를 hp bar로 표현
    {
        if (Team_count == 1)
        {
            hp = BattleManager.Instance.bM_Character_Team1[character_count].GetComponent<Character>().character_HP;
            hp_bar.GetComponent<Slider>().value = hp;
        }

        else
        {
            hp = BattleManager.Instance.bM_Character_Team2[character_count].GetComponent<Character>().character_HP;
            hp_bar.GetComponent<Slider>().value = hp;
        }
    }

    void original_Hp() // 캐릭터 기본 hp를 original_hp에 기입하기 위한 함수
    {
        if(hp>0&& is_this_original_hp_counting==0)
        {
            original_hp = hp;
            hp_bar.GetComponent<Slider>().maxValue = original_hp;
            hp_bar.GetComponent<Slider>().value = original_hp;
            is_this_original_hp_counting++;
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

    void Hp_bar_position_translate() //hp bar들의 위치 이동시키는 함수
    {
        if (BattleManager.Instance.bM_Character_Team2[character_count] == null)
            return;

        if (Team_count == 1)
        {
            hp_bar.transform.position = worldToUISpace(canvas.GetComponent<Canvas>(), BattleManager.Instance.bM_Character_Team1[character_count].transform.position);
            hp_bar.transform.Translate(0, hp_bar_y_position, 0);
        }

        else
        {
            hp_bar.transform.position = worldToUISpace(canvas.GetComponent<Canvas>(), BattleManager.Instance.bM_Character_Team2[character_count].transform.position);
            hp_bar.transform.Translate(0, hp_bar_y_position, 0);
        }
    }

    private void Update()
    {
        if (BattleManager.Instance.bM_Phase >= 1)
        {
            original_Hp();

            showing_Hp_point();
        }

        Hp_bar_position_translate();
    }








}
