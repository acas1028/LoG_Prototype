using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Deck_Manager : MonoBehaviour
{
    static public Deck_Manager instance;

    public GameObject[] Slot_Type;
    public GameObject[] Character_Slot;
    public GameObject[] Set_Character_;
    public GameObject[] Grid_Button;
    public GameObject Current_Character;
    public GameObject Current_Grid;

    public Text[] Character_Stat;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {

    }

    void Update()
    {

    }
    public void Check_Stat()
    {
        Character current = Current_Character.GetComponent<Character>();
        current.Debuging_Character();
        Character_Stat[0].text = current.character_HP.ToString();
        Character_Stat[1].text = current.character_AP.ToString();
        Character_Stat[2].text = current.character_Attack_Damage.ToString();
        for (int i = 0; i < 9; i++)
        {
            if (current.character_Attack_Range[i] == true)
            {
                Color_Grid(i);
            }
        }
    }
    public void Click_Grid_2()
    {
        Character current = Current_Character.GetComponent<Character>();
        Deck_Grid CG = Current_Grid.GetComponent<Deck_Grid>();

        if (CG.is_Clicked_Grid == false)//그리드 취소
        {
            current.character_AP += 10;
            current.character_Attack_Range[CG.Grid_Num] = false;
            Recolor_Grid(CG.Grid_Num);
            Check_Stat();
        }
        else//그리드 선택
        {
            if (current.character_AP < 10)
                return;
            else
            {
                current.character_AP -= 10;
                current.character_Attack_Range[CG.Grid_Num] = true;
                Color_Grid(CG.Grid_Num);
                Check_Stat();
            }
        }
    }
    public void Save_Button()
    {
        Character current = Current_Character.GetComponent<Character>();
        for (int i = 0; i < Set_Character_.Length; i++)
        {
            if (Character_Slot[i].GetComponent<Deck_Character>().Set_Active_Character == false)
            {
                Set_Character_[i].SetActive(true);
            }
        }
        for (int i = 0; i < 9; i++)
        {
            Recolor_Grid(i);
            Grid_Button[i].GetComponent<Deck_Grid>().is_Clicked_Grid = false;
        }
        while (current.character_AP > 1)
        {
            current.character_Attack_Damage += 10; //임의
            current.character_AP -= 10;
        }
        current.Debuging_Character();
        Character_Stat[1].text = current.character_AP.ToString();
        Character_Stat[2].text = current.character_Attack_Damage.ToString();

    }
    private void Recolor_Grid(int num)
    {

        Button Grid = Grid_Button[num].GetComponent<Button>();
        ColorBlock CB = Grid.colors;
        Color white_Grid = Color.white;
        CB.normalColor = white_Grid;
        CB.pressedColor = white_Grid;
        CB.selectedColor = white_Grid;
        Grid.colors = CB;

    }
    private void Color_Grid(int num)
    {
        Button Grid = Grid_Button[num].GetComponent<Button>();
        ColorBlock CB = Grid.colors;
        Color Red_Grid = Color.red;
        CB.normalColor = Red_Grid;
        CB.pressedColor = Red_Grid;
        CB.selectedColor = Red_Grid;
        Grid.colors = CB;
    }
}
