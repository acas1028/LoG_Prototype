using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Deck_Manager : MonoBehaviour
{
    static public Deck_Manager instance;

    public GameObject[] Slot_Type;
    public GameObject[] Slot_Property;
    public GameObject[] Character_Slot;
    public GameObject[] Set_Character_;
    public GameObject[] Grid_Button;
    public List<GameObject> Skill_List = new List<GameObject>();
    public GameObject[] Skill_Button;
    public GameObject[] Property_Slot;
    public GameObject[] Page_Slot;
    public GameObject Current_Character;
    public GameObject Current_Grid;
    public GameObject Deck_Reset_Button;
    public GameObject Pre_Skill;
    public DeckDataSync deckDataSync;

    private Deck_Data_Send Deck_Data;

    public Sprite Grid_disselected_sprite;
    public Sprite Grid_selected_sprite;

    public Text[] Character_Stat;

    public int Page_Num;
    private int Pre_pageNum = -1;

    private void Awake()
    {
        instance = this;
        Deck_Data = Deck_Data_Send.instance;
    }
    void Start()
    {
        Page_Num = Deck_Data_Send.instance.lastPageNum;
        Load_Deck();
        Invoke("StartDeckClick", 0.05f); //불러올때 잘못된 특성과 스킬을 불러오는 경우가 있어 덱 페이지 1를 클릭한 체 시작하도록 함
    }

    void Update()
    {

    }
    public void Check_Stat()
    {
        Character current = Current_Character.GetComponentInChildren<Character>();
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
                Grid_Button[i].GetComponent<Deck_Grid>().is_Clicked_Grid = true;
            }
        }
        Show_Property_Slot();
        Select_Skill();
    }
    public void Click_Grid_2()
    {
        Character current = Current_Character.GetComponentInChildren<Character>();
        Deck_Grid CG = Current_Grid.GetComponent<Deck_Grid>();

        if (CG.is_Clicked_Grid == true)
        {
            current.character_AP += 20;
            current.character_Attack_Range[CG.Grid_Num] = false;
            CG.is_Clicked_Grid = false;
            Recolor_Grid(CG.Grid_Num);
            Check_Stat();
        }
        else//그리드 선택
        {
            if (current.character_AP < 20)
                return;
            else
            {
                current.character_AP -= 20;
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
            if (Character_Slot[i].GetComponentInChildren<Character>().character_ID == 0)
            {
                Debug.Log("캐릭터를 전부 할당해 주세요");
                cant_save = true;
                return;
            }
        }

        if (cant_save == false)
        {
            Character current = Current_Character.GetComponentInChildren<Character>();

            for (int i = 0; i < 7; i++)
            {
                Character ch = Character_Slot[i].GetComponentInChildren<Character>();

                if(ch.character_AP > 0)
                {
                    ch.character_Attack_Damage += ch.character_AP;
                    ch.character_AP = 0;
                    ch.Debuging_Character();
                }
            }

            for (int i = 0; i < 7; i++)
            {
                if (Character_Slot[i].activeSelf && Deck_Data.Save_Data[Page_Num, i].GetComponent<Character>().character_ID == 0)
                {
                    Deck_Data.Save_Data[Page_Num, i].GetComponent<Character>().Copy_Character_Stat(Character_Slot[i].transform.Find("Character_Prefab").gameObject);
                    Deck_Data.Save_Data[Page_Num, i].GetComponent<Character>().Debuging_Character();
                    deckDataSync.SetData(Page_Num, i, Deck_Data.Save_Data[Page_Num, i].GetComponent<Character>());
                    deckDataSync.SendLastPageNum(Page_Num);
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
            if (Deck_Data.Save_Data[0, i].GetComponent<Character>().character_ID==0)
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
            Character_Slot[i].GetComponentInChildren<Character>().Character_Reset();
            Character_Slot[i].GetComponentInChildren<Character>().Debuging_Character();
            Character_Slot[i].GetComponent<Deck_Character>().Set_Active_Character = false;
            Character_Slot[i].SetActive(false);
            Set_Character_[i].SetActive(true);
            Slot_Property[i].GetComponent<Image>().sprite = null;
            Deck_Data.Save_Data[Page_Num, i].GetComponent<Character>().Character_Reset();
            Deck_Data.Save_Data[Page_Num, i].GetComponent<Character>().Debuging_Character();
            Slot_Type[i].GetComponent<Deck_Type_Slot>().Change_Type(0);
        }
        Skill_List.Clear();
    }

    public void Reset_Grid()
    {
        for(int i=0;i<9;i++)
        {
            Button Grid = Grid_Button[i].GetComponent<Button>();
            SpriteState spriteState = Grid.spriteState;
            spriteState.disabledSprite = Grid_disselected_sprite;
            spriteState.highlightedSprite = Grid_disselected_sprite;
            spriteState.pressedSprite = Grid_disselected_sprite;
            spriteState.selectedSprite = Grid_disselected_sprite;
            Grid.spriteState = spriteState;
            Grid.GetComponent<Image>().sprite = Grid_disselected_sprite;
            Grid_Button[i].GetComponent<Deck_Grid>().is_Clicked_Grid = false;
        }
    }

    void Load_Deck()
    {
        // 로그인시, 혹은 오프라인 모드 입장시 덱 데이터를 불러온 후에 진행되도록 수정하였음
        for (int i = 0; i < 7; i++)
        {
            Character ch = Deck_Data.Save_Data[Page_Num, i].GetComponent<Character>();
            Character cs = Character_Slot[i].GetComponentInChildren<Character>();
            if (ch.character_ID != 0)
            {
                //Set_Character_[i].SetActive(false);
                Character_Slot[i].SetActive(true);
                Slot_Type[i].SetActive(true);
                Slot_Property[i].GetComponent<Property_Slot>().Change_property(cs.character_Skill.ToString());
                Slot_Type[i].GetComponent<Deck_Type_Slot>().Change_Type((int)ch.character_Type);
                cs.Copy_Character_Stat(Deck_Data.Save_Data[Page_Num, i]);
                cs.Debuging_Character();
            }
        }
        Load_Skill();
        
    }

    IEnumerator Switch_Page()
    {
        //Reset_Button();
        yield return StartCoroutine("Load_Deck");
        bool Switch = true;
        for(int i=0;i<7;i++)
        {
            Character cs = Character_Slot[i].GetComponentInChildren<Character>();
            if(cs.character_ID==0)
            {
                Switch = false;
            }
        }
        if(Switch == false)
        {
            for(int i = 0; i<7; i++)
            {
                Set_Character_[i].SetActive(true);
                Character_Slot[i].SetActive(false);
                Slot_Type[i].SetActive(false);
                //특성 (false);
            }
        }
    }
    private void Load_Skill()
    {
        if (Deck_Data.Save_Data[0, 0].GetComponent<Character>().character_ID == 0)
        {
            return;
        }
        else
        { 
            for (int i = 0; i < 7; i++)
            {
                int num = (int)Deck_Data.Save_Data[Page_Num, i].GetComponent<Character>().character_Skill;
                Skill_List.Add(Skill_Button[num]);
            }
            for (int i = 0; i < Skill_List.Count; i++)
            {
                Skill_List[i].GetComponent<Deck_Skill>().is_selected = true;
                Button SKill = Skill_List[i].GetComponent<Button>();
                ColorBlock CB = SKill.colors;
                Color Red_Grid = Color.red;
                CB.normalColor = Red_Grid;
                CB.pressedColor = Red_Grid;
                CB.selectedColor = Red_Grid;
                SKill.colors = CB;
            }
            Pre_Skill = Skill_List[6];
        }
    }
    private void Reset_Skill()
    {
        for(int i=0;i<Skill_List.Count;i++)
        {
            Skill_List[i].GetComponent<Deck_Skill>().is_selected = false;
            Button b_Skill = Skill_List[i].GetComponent<Button>();
            ColorBlock CB = b_Skill.colors;
            Color white = Color.white;
            CB.normalColor = white;
            CB.pressedColor = white;
            CB.selectedColor = white;
            b_Skill.colors = CB;
        }
    }
    private void Show_Property_Slot()
    {
        int j = 0;
        while (j < 7)
        {
            if (Current_Character == Character_Slot[j])
            {
                break;
            }
            j++;
        }
        switch((int)Slot_Type[j].GetComponent<Deck_Type_Slot>().Character_Type)
        {
            case 1:
                Property_Slot[0].SetActive(false);
                Property_Slot[1].SetActive(true);
                Property_Slot[2].SetActive(false);
                Property_Slot[3].SetActive(false);
                break;
            case 2:
                Property_Slot[0].SetActive(false);
                Property_Slot[1].SetActive(false);
                Property_Slot[2].SetActive(true);
                Property_Slot[3].SetActive(false);
                break;
            case 3:
                Property_Slot[0].SetActive(false);
                Property_Slot[1].SetActive(false);
                Property_Slot[2].SetActive(false);
                Property_Slot[3].SetActive(true);
                break;
        }
    }
    private void Recolor_Grid(int num)
    {
        Button Grid = Grid_Button[num].GetComponent<Button>();
        SpriteState spriteState = Grid.spriteState;
        spriteState.disabledSprite = Grid_disselected_sprite;
        spriteState.highlightedSprite = Grid_disselected_sprite;
        spriteState.pressedSprite = Grid_disselected_sprite;
        spriteState.selectedSprite = Grid_disselected_sprite;
        Grid.spriteState = spriteState;
        Grid.GetComponent<Image>().sprite = Grid_disselected_sprite;
    }
    private void Color_Grid(int num)
    {
        Button Grid = Grid_Button[num].GetComponent<Button>();
        SpriteState spriteState = Grid.spriteState;
        spriteState.disabledSprite = Grid_selected_sprite;
        spriteState.highlightedSprite = Grid_selected_sprite;
        spriteState.pressedSprite = Grid_selected_sprite;
        spriteState.selectedSprite = Grid_selected_sprite;
        Grid.spriteState = spriteState;
        Grid.GetComponent<Image>().sprite = Grid_selected_sprite;
    }
    private void Select_Skill()
    {
        for(int i=0;i<Skill_List.Count;i++)
        {
            if(Pre_Skill == Skill_List[i]&&i>0)
            {
                Button SKill = Skill_List[i-1].GetComponent<Button>();
                ColorBlock CB = SKill.colors;
                Color red_Grid = Color.red;
                CB.normalColor = red_Grid;
                CB.pressedColor = red_Grid;
                CB.selectedColor = red_Grid;
                SKill.colors = CB;
                return;
            }
        }
    }
    public void Cancle_Grid()
    {
        Character cs = Current_Character.GetComponentInChildren<Character>();
        bool[] Range = cs.character_Attack_Range;
        int ID = cs.character_ID;
        for (int i = 0; i < 9; i++)
        {
            if (Range[i])
            {
                Grid_Button[i].GetComponent<Deck_Grid>().is_Clicked_Grid = false;
                Recolor_Grid(i);
            }
        }
        cs.Character_Setting(ID);
        Check_Stat();
    }

    void StartDeckClick()
    {
        Page_Slot[0].GetComponent<Button>().onClick.Invoke();
    }
}
