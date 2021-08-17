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
    public List<GameObject> Skill_Button = new List<GameObject>();
    public GameObject[] D_Page;
    public GameObject Current_Character;
    public GameObject Current_Grid;
    public GameObject Deck_Reset_Button;
    public GameObject Pre_Skill;
    public DeckDataSync deckDataSync;
    public Deck_Data_Send Deck_Data;

    public Sprite Grid_disselected_sprite;
    public Sprite Grid_selected_sprite;

    public Text[] Character_Stat;

    private void Awake()
    {
        instance = this;
        Deck_Data = Deck_Data_Send.instance;
    }
    void Start()
    {
        StartCoroutine("Load_Deck");
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
        Character_Stat[3].text = "(" + "+" + current.character_AP + ")";
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
        bool cant_save = false;
        for (int i = 0; i < 7; i++)
        {
            if (Character_Slot[i].GetComponent<Character>().character_ID == 0)
            {
                Debug.Log("캐릭터를 전부 할당해 주세요");
                cant_save = true;
                return;
            }
        }

        if (cant_save == false)
        {
            Character current = Current_Character.GetComponent<Character>();

            for (int i = 0; i < 7; i++)
            {
                Character ch = Character_Slot[i].GetComponent<Character>();

                while (ch.character_AP > 0)
                {
                    ch.character_Attack_Damage += 10;
                    ch.character_AP -= 10;//임의
                    ch.Debuging_Character();
                }
            }

            for (int i = 0; i < 7; i++)
            {
                if (Character_Slot[i].activeSelf && Deck_Data.Save_Data[i].GetComponent<Character>().character_ID == 0)
                {
                    Deck_Data.Save_Data[i].GetComponent<Character>().Copy_Character_Stat(Character_Slot[i]);
                    Deck_Data.Save_Data[i].GetComponent<Character>().Debuging_Character();
                    deckDataSync.SetData(0, i, Deck_Data.Save_Data[i].GetComponent<Character>());
                }
            }
            Character_Stat[1].text = current.character_AP.ToString();
            Character_Stat[2].text = current.character_Attack_Damage.ToString();
            Character_Stat[3].text = "(" + "+" + current.character_AP + ")";
        }
    }

    public void Auto_Save()
    {
        bool save = false;
        for (int i = 0; i < 7; i++)
        {
            if (Deck_Data.Save_Data[i].GetComponent<Character>().character_ID==0)
            {
                save = true;
                return;
            }
        }
        if(save)
        {
            Save_Button();
        }
    }
    public void Reset_Button()
    {
        Reset_Grid();
        Reset_Skill();
        for (int i=0;i<7;i++)
        {
            Character_Slot[i].GetComponent<Character>().Character_Reset();
            Character_Slot[i].GetComponent<Character>().Debuging_Character();
            Deck_Data.Save_Data[i].GetComponent<Character>().Character_Reset();
            Deck_Data.Save_Data[i].GetComponent<Character>().Debuging_Character();
            Slot_Type[i].GetComponent<Deck_Type_Slot>().Change_Type(0);
        }
        Skill_Button.Clear();
    }

    public void Reset_Grid()
    {
        for(int i=0;i<9;i++)
        {
            Button Grid = Grid_Button[i].GetComponent<Button>();
            //ColorBlock CB = Grid.colors;
            //Color white_Grid = Color.white;
            //CB.normalColor = white_Grid;
            //CB.pressedColor = white_Grid;
            //CB.selectedColor = white_Grid;
            //Grid.colors = CB;
            SpriteState spriteState = Grid.spriteState;
            spriteState.disabledSprite = Grid_disselected_sprite;
            spriteState.highlightedSprite = Grid_disselected_sprite;
            spriteState.pressedSprite = Grid_disselected_sprite;
            spriteState.selectedSprite = Grid_disselected_sprite;
            Grid.spriteState = spriteState;
            Grid_Button[i].GetComponent<Deck_Grid>().is_Clicked_Grid = false;
        }
    }

    IEnumerator Load_Deck()
    {
        yield return new WaitForSeconds(0.2f);
        for (int i = 0; i < 7; i++)
        {
            Character ch = Deck_Data.Save_Data[i].GetComponent<Character>();
            Character cs = Character_Slot[i].GetComponent<Character>();
            if (ch.character_ID != 0)
            {
                Set_Character_[i].SetActive(false);
                Character_Slot[i].SetActive(true);
                Slot_Type[i].SetActive(true);
                Slot_Type[i].GetComponent<Deck_Type_Slot>().Change_Type((int)ch.character_Type);
                cs.Copy_Character_Stat(Deck_Data.Save_Data[i]);
                cs.Debuging_Character();
            }
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
        //ColorBlock CB = Grid.colors;
        //Color white_Grid = Color.white;
        //CB.normalColor = white_Grid;
        //CB.pressedColor = white_Grid;
        //CB.selectedColor = white_Grid;
        //Grid.colors = CB;
        SpriteState spriteState = Grid.spriteState;
        spriteState.disabledSprite = Grid_disselected_sprite;
        spriteState.highlightedSprite = Grid_disselected_sprite;
        spriteState.pressedSprite = Grid_disselected_sprite;
        spriteState.selectedSprite = Grid_disselected_sprite;
        Grid.spriteState = spriteState;
    }
    private void Color_Grid(int num)
    {
        Button Grid = Grid_Button[num].GetComponent<Button>();
        //ColorBlock CB = Grid.colors;
        //Color Red_Grid = Color.red;
        //CB.normalColor = Red_Grid;
        //CB.pressedColor = Red_Grid;
        //CB.selectedColor = Red_Grid;
        //Grid.colors = CB;
        SpriteState spriteState = Grid.spriteState;
        spriteState.disabledSprite = Grid_selected_sprite;
        spriteState.highlightedSprite = Grid_selected_sprite;
        spriteState.pressedSprite = Grid_selected_sprite;
        spriteState.selectedSprite = Grid_selected_sprite;
        Grid.spriteState = spriteState;
    }
}
