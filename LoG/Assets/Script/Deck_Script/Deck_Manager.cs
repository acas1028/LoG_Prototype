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
    public GameObject[] Save_Characters;
    public List<GameObject> Skill_Button = new List<GameObject>();
    public GameObject Current_Character;
    public GameObject Current_Grid;
    public GameObject Deck_Reset_Button;
    public GameObject Pre_Skill;
    public DeckDataSync deckDataSync;

    public Text[] Character_Stat;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        deckDataSync.GetData(0);
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

        if (CG.is_Clicked_Grid == true)
        {
            current.character_AP += 10;
            current.character_Attack_Range[CG.Grid_Num] = false;
            CG.is_Clicked_Grid = false;
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
                CG.is_Clicked_Grid = true;
                Color_Grid(CG.Grid_Num);
                Check_Stat();
            }
        }
    }
    public void Save_Button()
    {
        Character current = Current_Character.GetComponent<Character>();

        for (int i=0;i<7;i++)
        {
            while(Character_Slot[i].GetComponent<Character>().character_AP>0)
            {
                Character_Slot[i].GetComponent<Character>().character_Attack_Damage += 10;
                Character_Slot[i].GetComponent<Character>().character_AP -= 10;//임의
                Character_Slot[i].GetComponent<Character>().Debuging_Character();
            }
        }

        for (int i = 0; i < 7; i++)
        {
            if (Character_Slot[i].activeSelf && Save_Characters[i].GetComponent<Character>().character_ID == 0)
            {
                Save_Characters[i].GetComponent<Character>().Copy_Character_Stat(Character_Slot[i]);
                Save_Characters[i].GetComponent<Character>().Debuging_Character();
                deckDataSync.SetData(0, i, Save_Characters[i].GetComponent<Character>());
            }
        }
        Character_Stat[1].text = current.character_AP.ToString();
        Character_Stat[2].text = current.character_Attack_Damage.ToString();
    }
    public void Reset_Button()
    {
        Reset_Grid();
        Reset_Skill();
        for (int i=0;i<7;i++)
        {
            Character_Slot[i].GetComponent<Character>().Character_Reset();
            Character_Slot[i].GetComponent<Character>().Debuging_Character();
            Save_Characters[i].GetComponent<Character>().Character_Reset();
            Save_Characters[i].GetComponent<Character>().Debuging_Character();
            Slot_Type[i].GetComponent<Deck_Type_Slot>().Change_Type(0);
        }
        Skill_Button.Clear();
    }

    public void Reset_Grid()
    {
        for(int i=0;i<9;i++)
        {
            Button Grid = Grid_Button[i].GetComponent<Button>();
            ColorBlock CB = Grid.colors;
            Color white_Grid = Color.white;
            CB.normalColor = white_Grid;
            CB.pressedColor = white_Grid;
            CB.selectedColor = white_Grid;
            Grid.colors = CB;
            Grid_Button[i].GetComponent<Deck_Grid>().is_Clicked_Grid = false;
        }
    }
    private void Reset_Skill()
    {
        for(int i=0;i<Skill_Button.Count;i++)
        {
            Skill_Button[i].GetComponent<Deck_Skill>().is_selected = false;
            Button b_Skill = Skill_Button[i].GetComponent<Button>();
            ColorBlock CB = b_Skill.colors;
            Color white = Color.white;
            CB.normalColor = white;
            CB.pressedColor = white;
            CB.selectedColor = white;
            b_Skill.colors = CB;
        }
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
