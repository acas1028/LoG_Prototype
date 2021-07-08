using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Showing_Hp : MonoBehaviour
{

    private float hp;
    private float original_hp;
    private int is_this_original_hp_counting=0;
    private GameObject hp_bar;
    private GameObject canvas;

    public int character_count;
    public int Team_count;


    private void Start()
    {
        canvas = this.gameObject.transform.parent.gameObject;
        original_hp = 1;
        hp_bar = this.gameObject;
        
    }

    void showing_Hp_point()
    {
        if (Team_count == 1)
        {
            hp = BattleManager.Instance.bM_Character_Team1[character_count].GetComponent<Character_Script>().character_HP;
            hp_bar.GetComponent<Slider>().value = hp;
        }

        else
        {
            hp = BattleManager.Instance.bM_Character_Team2[character_count].GetComponent<Character_Script>().character_HP;
            hp_bar.GetComponent<Slider>().value = hp;
        }
    }

    void original_Hp()
    {
        if(hp>0&& is_this_original_hp_counting==0)
        {
            original_hp = hp;
            hp_bar.GetComponent<Slider>().maxValue = original_hp;
            hp_bar.GetComponent<Slider>().value = original_hp;
            is_this_original_hp_counting++;
        }
    }

    public Vector3 worldToUISpace(Canvas parentCanvas, Vector3 worldPos)
    {
        //Convert the world for screen point so that it can be used with ScreenPointToLocalPointInRectangle function
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        Vector2 movePos;

        //Convert the screenpoint to ui rectangle local point
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentCanvas.transform as RectTransform, screenPos, parentCanvas.worldCamera, out movePos);
        //Convert the local point to world point
        return parentCanvas.transform.TransformPoint(movePos);
    }

    void Hp_bar_position_translate()
    {
        if (Team_count == 1)
        {
            hp_bar.transform.position = worldToUISpace(canvas.GetComponent<Canvas>(), BattleManager.Instance.bM_Character_Team1[character_count].transform.position);
            hp_bar.transform.Translate(0, -70, 0);
        }

        else
        {
            hp_bar.transform.position = worldToUISpace(canvas.GetComponent<Canvas>(), BattleManager.Instance.bM_Character_Team2[character_count].transform.position);
            hp_bar.transform.Translate(0, -70, 0);
        }
    }

    private void Update()
    {
        Debug.Log(BattleManager.Instance.bM_Character_Team1[0]);
        Debug.Log(BattleManager.Instance.bM_Character_Team1[1]);

        Debug.Log(BattleManager.Instance.bM_Phase);
        if (BattleManager.Instance.bM_Phase >= 1)
        {
            original_Hp();

            showing_Hp_point();
        }

        Hp_bar_position_translate();
    }








}
