using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Deck_Manager : MonoBehaviour
{
    private int nowPageIdx;
    public int NowPageIdx
    {
        get => nowPageIdx;
    }
    public GameObject[] Slot_Type;
    public GameObject[] Slot_Property;
    public GameObject[] Character_Slot;
    public GameObject[] Set_Character_;
    public GameObject[] Grid_Button;
    public List<GameObject> Skill_List = new List<GameObject>();
    public GameObject[] Skill_Button;
    public GameObject[] Property_Slot;
    public GameObject[] Page_Slot;
    public GameObject[] CharacterSpace;
    public GameObject Current_Character;
    public GameObject Current_Grid;
    public GameObject Deck_Reset_Button;
    public GameObject Pre_Skill;
    public DeckDataSync deckDataSync;
    [SerializeField]
    private GameObject NotSavePopUp;

    private Deck_Data_Send Deck_Data;

    public Sprite Grid_disselected_sprite;
    public Sprite Grid_selected_sprite;

    public Text[] Character_Stat;

    private void Awake() {
        Deck_Data = Deck_Data_Send.instance;
        nowPageIdx = Deck_Data_Send.instance.lastPageNum;
        Page_Slot[0].GetComponent<Button>().onClick.Invoke();
        //CheckSkillIsUnlocked();
    }

    private void Start()
    {
        if(Deck_Data.Save_Data[nowPageIdx,0].GetComponent<Character>().character_ID == 0)
        {
            Debug.Log("Empty");
        }

        else
        {
            StartCoroutine("InitialCharacterID");
        }
        
    }

    private void CheckSkillIsUnlocked() {
        foreach (var skill in Skill_Button) {
            if (!UserDataSynchronizer.unlockedSkillList.Contains(skill.GetComponent<Deck_Skill>().Character_Skill))
                skill.GetComponent<Button>().interactable = false;
        }

    }

    public void SetNowPageIndex(int pageIdx)
    {
        nowPageIdx = pageIdx;
        if (pageIdx < 0) nowPageIdx = 0;
        else if (pageIdx > 4) nowPageIdx = 4;
    }

    public void Check_Stat()
    {
        Character current = Current_Character.GetComponentInChildren<Character>();
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
        Check_Skill();
        
        if(current.character_Type == CharacterStats.CharacterType.Attacker)
        {
            Property_Slot[0].SetActive(false);
            Property_Slot[1].SetActive(true);
            Property_Slot[2].SetActive(false);
            Property_Slot[3].SetActive(false);
        }
        else if(current.character_Type == CharacterStats.CharacterType.Balance)
        {
            Property_Slot[0].SetActive(false);
            Property_Slot[1].SetActive(false);
            Property_Slot[2].SetActive(true);
            Property_Slot[3].SetActive(false);
        }
        else if(current.character_Type == CharacterStats.CharacterType.Defender)
        {
            Property_Slot[0].SetActive(false);
            Property_Slot[1].SetActive(false);
            Property_Slot[2].SetActive(false);
            Property_Slot[3].SetActive(true);
        }
    }

    public void Click_Grid()
    {
        Character current = Current_Character.GetComponentInChildren<Character>();
        Current_Grid = EventSystem.current.currentSelectedGameObject;
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
        for (int i = 0; i < 7; i++)
        {
            if (Character_Slot[i].GetComponentInChildren<Character>().character_ID == 0)
            {
                NotSavePopUp.SetActive(true);
                return;
            }
        }

        Character current = Current_Character.GetComponentInChildren<Character>();

        for (int i = 0; i < 7; i++)
        {
            Character ch = Character_Slot[i].GetComponentInChildren<Character>();

            if(ch.character_AP > 0)
            {
                ch.character_Attack_Damage += ch.character_AP;
                ch.character_AP = 0;
            }
        }

        for (int i = 0; i < 7; i++)
        {
            if (Character_Slot[i].activeSelf)
            {
                Deck_Data.Save_Data[nowPageIdx, i].GetComponent<Character>().Copy_Character_Stat(Character_Slot[i].transform.Find("Character_Prefab").gameObject);
                deckDataSync.SetData(nowPageIdx, i, Deck_Data.Save_Data[nowPageIdx, i].GetComponent<Character>());
                deckDataSync.SendLastPageNum(nowPageIdx);
            }
        }
        Character_Stat[1].text = current.character_AP.ToString();
        Character_Stat[2].text = current.character_Attack_Damage.ToString();
        Character_Stat[3].text = "(" + "+" + current.character_AP + ")";
    }

    public void Reset_Button()
    {
        Reset_Grid();
        Reset_Skill();
        for (int i=0;i<7;i++)
        {
            Character_Slot[i].GetComponentInChildren<Character>().Character_Reset();
            Character_Slot[i].GetComponent<Deck_Character>().Set_Active_Character = false;
            Character_Slot[i].SetActive(false);
            Set_Character_[i].SetActive(true);
            Slot_Property[i].GetComponent<Image>().sprite = null;
            Deck_Data.Save_Data[nowPageIdx, i].GetComponent<Character>().Character_Reset();
            Slot_Type[i].GetComponent<Deck_Type_Slot>().Change_Type(0);
            CharacterSpace[i].GetComponent<ShowSprite_SaveDeckData>().CharacterSpriteReset();
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
            Character ch = Deck_Data.Save_Data[nowPageIdx, i].GetComponent<Character>();
            Character cs = Character_Slot[i].GetComponentInChildren<Character>();
            if (ch.character_ID != 0)
            {
                //Set_Character_[i].SetActive(false);
                Character_Slot[i].SetActive(true);
                Slot_Type[i].SetActive(true);
                Slot_Property[i].GetComponent<Property_Slot>().Change_property(cs.character_Skill.ToString());
                Slot_Type[i].GetComponent<Deck_Type_Slot>().Change_Type((int)ch.character_Type);
                cs.Copy_Character_Stat(Deck_Data.Save_Data[nowPageIdx, i]);
            }
        }
        Load_Skill();
        
    }

    public void Switch_Page()
    {
        if (Deck_Data.Save_Data[nowPageIdx, 0].GetComponent<Character>().character_ID == 0)
        {
            for (int i = 0; i < 7; i++)
            {
                Set_Character_[i].SetActive(true);
                Character_Slot[i].SetActive(false);
                Slot_Type[i].SetActive(false);
            }
        }

        for(int i=0; i<7; i++) //덱 페이지 바뀌었을 때 스프라이트 변화, 선택되었던 스킬 버튼 색은 다시 본래대로 변화.
        {
            CharacterSpace[i].GetComponent<ShowSprite_SaveDeckData>().SetSprite();
            Character_Slot[i].GetComponent<Image>().color = Color.white;
        }

        for(int i=0; i<Grid_Button.Length;i++)
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
        Skill_List.Clear();
        Reset_Skill();
        Reset_Grid();
        Load_Skill();
        StartCoroutine("InitialCharacterID");

        for(int i=0;i<3;i++)
        {
            Character_Stat[i].text = "-";
        }
    }

    private void Load_Skill()
    {
        if(Deck_Data.Save_Data[nowPageIdx, 0].GetComponent<Character>().character_ID == 0) // 그 덱 페이지의 캐릭터의 아이디가 0일 경우 
        {
            return;
        }
        else
        { 
            for (int i = 0; i < 7; i++)
            {
                int num = (int)Deck_Data.Save_Data[nowPageIdx, i].GetComponent<Character>().character_Skill;
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
        }
    }
    private void Reset_Skill()
    {
        if (Skill_List.Count == 0)
        {
            for (int i = 0; i < Skill_Button.Length; i++)
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
        switch((int)Slot_Type[j].GetComponent<Deck_Type_Slot>().characterType)
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
        for (int i=0;i<Skill_List.Count;i++)
        {
            if(Pre_Skill == Skill_List[i])
            {
                Button SKill = Skill_List[i].GetComponent<Button>();
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
    void Check_Skill()
    {
        for(int i=0;i<Skill_List.Count;i++)
        {
            Button SKill_List = Skill_List[i].GetComponent<Button>();
            ColorBlock block = SKill_List.colors;
            Color red_Grid = Color.red;
            block.normalColor = red_Grid;
            block.pressedColor = red_Grid;
            block.selectedColor = red_Grid;
            SKill_List.colors = block;
        }

        if (Current_Character.GetComponentInChildren<Character>().character_ID == 0)
            return;

        int num = Current_Character.GetComponentInChildren<Character>().character_ID;

        if (num == 8 || num == 16 || num == 24)
            return;
        else
        { 
            Button SKill = Skill_Button[num - 1].GetComponent<Button>();
            ColorBlock CB = SKill.colors;
            Color yellow_Grid = Color.yellow;
            CB.normalColor = yellow_Grid;
            CB.pressedColor = yellow_Grid;
            CB.selectedColor = yellow_Grid;
            SKill.colors = CB;
        }
    }

    IEnumerator InitialCharacterID()
    {
        yield return new WaitForSeconds(0.5f);

        for(int i=0;i<7;i++)
        {
            Character_Slot[i].GetComponentInChildren<Character>().Copy_Character_Stat(Deck_Data.Save_Data[nowPageIdx,i]);
        }
    }
}
