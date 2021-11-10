using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Showing_Hp : MonoBehaviour
{

    private float hp; //ĳ���� ���� hp
    private float original_hp; //ĳ������ �⺻ hp
    private int is_this_original_hp_counting=0; // original_hp�� ä���°��� ���� ����
    private int hp_bar_y_position; // ĳ������ hp �ٿ� ĳ���Ͱ��� �Ÿ�.
    private GameObject hp_bar; //hp ��
    private GameObject canvas; 

    // �Ʒ��� ������ inspectorâ�� ������ �����س��Ҵ�.
    public int character_count; // ���� ������ ĳ���� ���� ����
    public int Team_count; //�� ���� ����


    private void Start()
    {
        original_hp = 1;
        hp_bar_y_position = 175;
        canvas = this.gameObject.transform.parent.gameObject;
        hp_bar = this.gameObject;

    }

    void showing_Hp_point() //���� hp�� hp bar�� ǥ��
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

    void original_Hp() // ĳ���� �⺻ hp�� original_hp�� �����ϱ� ���� �Լ�
    {
        if(hp>0&& is_this_original_hp_counting==0)
        {
            original_hp = hp;
            hp_bar.GetComponent<Slider>().maxValue = original_hp;
            hp_bar.GetComponent<Slider>().value = original_hp;
            is_this_original_hp_counting++;
        }
    }

    public Vector3 worldToUISpace(Canvas parentCanvas, Vector3 worldPos) //ĵ������ �����ǰ� ������ �������� ��� ������ ���ִ� �Լ�
    {
        //Convert the world for screen point so that it can be used with ScreenPointToLocalPointInRectangle function
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        Vector2 movePos;

        //Convert the screenpoint to ui rectangle local point
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentCanvas.transform as RectTransform, screenPos, parentCanvas.worldCamera, out movePos);
        //Convert the local point to world point
        return parentCanvas.transform.TransformPoint(movePos);
    }

    void Hp_bar_position_translate() //hp bar���� ��ġ �̵���Ű�� �Լ�
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
