using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PVE_Arrayment : MonoBehaviour
{
    #region
    public GameObject[] MyGrids;
    public GameObject[] EnemyGrids;
    [SerializeField]
    private GameObject MyDeckData;
    [SerializeField]
    private GameObject[] Inventory;
    private GameObject CancleCharacter;
    [SerializeField]
    private GameObject ArrayCancleButton;
    private List<int> EnemyLocation = new List<int>()
    {0,1,2,3,4,5,6,7,8 };
    private List<GameObject> MyTeamList = new List<GameObject>();
    private bool isArray;
    private bool isCancle;
    private bool click_inventory = false;
    private int InvenNum;
    #endregion

    void Start()
    {
        EnemySetting();
        SetInventoryID();
    }

    // Update is called once per frame
    void Update()
    {
        Raycast();
    }

    void Raycast()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Arrayed_Data T = Arrayed_Data.instance;
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 1000);

            if (!hit)
                return;
            if (hit.transform.tag == "Character")
            {
                if (hit.transform.gameObject.GetComponent<GridCharacter_To_PopupPosition>().Popup_Position.transform.GetChild(0).gameObject.activeSelf == false)
                {
                    hit.transform.gameObject.GetComponent<GridCharacter_To_PopupPosition>().Popup_Position.transform.GetChild(0).gameObject.SetActive(true);
                    hit.transform.gameObject.GetComponent<GridCharacter_To_PopupPosition>().Popup_Position.transform.GetChild(0).transform.GetChild(0).GetComponent<ShowingCharacterStat_In_Arrayment>().ShowingStatInarray();
                    Limit_Popup.Limit_Poup_instance.SetIsbutton(this.gameObject);
                    Limit_Popup.Limit_Poup_instance.SetPopup(hit.transform.gameObject.GetComponent<GridCharacter_To_PopupPosition>().Popup_Position.transform.GetChild(0).gameObject);
                    Limit_Popup.Limit_Poup_instance.limit_Popup_button.SetActive(true);
                }
                else
                {
                    hit.transform.gameObject.GetComponent<GridCharacter_To_PopupPosition>().Popup_Position.transform.GetChild(0).gameObject.SetActive(false);
                    Limit_Popup.Limit_Poup_instance.SetIsbutton(null);
                    Limit_Popup.Limit_Poup_instance.limit_Popup_button.SetActive(false);
                }
                for (int i = 0; i < 9; i++)
                {
                    if (hit.transform.gameObject == MyGrids[i])//자신의 그리드에 있는 캐릭터를 클릭한 경우.
                    {
                        CancleCharacter = hit.transform.gameObject;
                        ArrayCancleButton.SetActive(!ArrayCancleButton.activeSelf);
                    }
                    else
                    {
                        ArrayCancleButton.SetActive(false);
                    }
                }
            }
            if (hit.transform.tag == "Null_Character" && click_inventory == true)// 인벤토리를 누르고, 빈 그리드를 클릭 -> 정상적인 배치를 한 경우.
            {
                GameObject Grid_Character = hit.transform.gameObject;
                int gridNum = 0;
                for (int i = 0; i < 9; i++)
                {
                    if (Grid_Character == MyGrids[i])
                    {
                        gridNum = i + 1;
                        break;
                    }
                }

                if (gridNum <= 0 || gridNum > 9)
                {
                    Debug.LogError("Grid_Character 에 해당하는 Grids[i]의 개체 찾을 수 없음");
                    return;
                }

                ArrayOnGrid(InvenNum, gridNum);
                click_inventory = false;
            }
        }
    }

    private void ArrayOnGrid(int inventoryNum, int gridNum)
    {
        if (MyTeamList.Count >= 5)
            return;

        MyGrids[gridNum - 1].tag = "Character";
        Character gridCharacter = MyGrids[gridNum - 1].GetComponentInChildren<Character>();
        gridCharacter.Copy_Character_Stat(MyDeckData.transform.GetChild(inventoryNum-1).gameObject);
        gridCharacter.character_Num_Of_Grid = gridNum;
        gridCharacter.InitializeCharacterSprite();

        Arrayed_Data arrayedData = Arrayed_Data.instance;
        isArray = true;

        for (int i = 0; i < 5; i++)
        {
            if (arrayedData.team1[i].GetComponent<Character>().character_ID == 0)
            {
                arrayedData.team1[i].GetComponent<Character>().Copy_Character_Stat(MyGrids[gridNum - 1].transform.Find("Character_Prefab").gameObject);
                ArrayOrder(arrayedData.team1[i]);
                break;
            }
        }

        Inventory[inventoryNum - 1].GetComponent<Inventory_ID>().SetArrayed();
    }

    void ArrayOrder(GameObject obj)
    {
        if(isArray)
        {
            MyTeamList.Add(obj);
            obj.GetComponent<Character>().character_Attack_Order = MyTeamList.Count;
            isArray = false;
        }
        if(isCancle)
        {
            MyTeamList.Remove(obj);
            for(int i=0;i<MyTeamList.Count;i++)
            {
                MyTeamList[i].GetComponent<Character>().character_Attack_Order = MyTeamList.Count;
            }
            isCancle = false;
        }
    }

    public void Arraycancle()
    {
        isCancle = true;

        Character cs = CancleCharacter.GetComponentInChildren<Character>();
        Arrayed_Data T = Arrayed_Data.instance;
        for (int i = 0; i < 5; i++)
        {
            if (cs.character_ID == T.team1[i].GetComponent<Character>().character_ID)
            {
                ArrayCancleButton.SetActive(false);
                ArrayOrder(T.team1[i]);
            }
        }
        CancleCharacter.tag = "Null_Character";
        cs.Character_Reset();
        cs.InitializeCharacterSprite();

    }

    void EnemySetting()//적 배치 -> 플레이어에게 정보 제공.
    {
        int[] num = RandomInt();

        for (int i = 0; i < 5; i++)
        {
            Character Enemy = EnemyGrids[num[i]].GetComponentInChildren<Character>();
            Enemy.Copy_Character_Stat(Arrayed_Data.instance.team2[i]);
            Enemy.InitializeCharacterSprite();
            EnemyGrids[num[i]].tag = "Character";
        }
    }

    int[] RandomInt()
    {
        int[] Rand = new int[5];
        for(int i=0;i<5;i++)
        {
            int num = Random.Range(0, EnemyLocation.Count);
            Rand[i] = EnemyLocation[num];
            EnemyLocation.RemoveAt(num);
        }

        return Rand;
    }

    void SetInventoryID()
    {
        for(int i=0;i<7;i++)
        {
            Inventory_ID Inven = Inventory[i].GetComponent<Inventory_ID>();
            Character TeamID = MyDeckData.transform.GetChild(i).GetComponent<Character>();
            Inven.SetCharacterID(TeamID.character_ID);
        }
    }

    public void ClickInventory(int num)
    {
        click_inventory = true;
        InvenNum = Inventory[num - 1].GetComponent<Inventory_ID>().GetInventoryNum();
    }

}
