using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;

public class PVE_Arrayment : MonoBehaviourPunCallbacks
{
    #region
    public GameObject[] MyGrids;
    public GameObject[] EnemyGrids;
    [SerializeField]
    private GameObject MyDeckData;
    [SerializeField]
    private Arrayed_Data m_ArrayData;
    [SerializeField]
    private GameObject[] EnemyData;
    [SerializeField]
    private GameObject[] Inventory;
    private GameObject CancleCharacter;
    [SerializeField]
    private GameObject ArrayCancleButton;
    [SerializeField]
    private Button ReadyButton;
    [SerializeField]
    private List<GameObject> MyTeamList = new List<GameObject>();
    private List<int> EnemyLocation = new List<int>()
    {1,2,3,4,5};
    private bool isArray;
    private bool isCancle;
    private bool click_inventory = false;
    private int InvenNum;
    [SerializeField]
    private bool is_PopOn = false;

    private int syncCount = 0;
    #endregion

    void Start()
    {
        ReadyButton.onClick.AddListener(GoBattle);
        Invoke("SetInventoryID", 0.5f);
        Invoke("InitArrayData", 0.6f);
        Invoke("EnemySetting", 0.7f);
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

            if (is_PopOn == true)
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

                Limit_Popup.Limit_Poup_instance.PopupOff();
            }
        }
    }

    private void ArrayOnGrid(int inventoryNum, int gridNum)
    {
        MyGrids[gridNum - 1].tag = "Character";
        Character gridCharacter = MyGrids[gridNum - 1].GetComponentInChildren<Character>();
        gridCharacter.Copy_Character_Stat(MyDeckData.transform.GetChild(inventoryNum-1).gameObject);
        gridCharacter.character_Num_Of_Grid = gridNum;
        gridCharacter.InitializeCharacterSprite();

        isArray = true;

        for (int i = 0; i < 5; i++)
        {
            if (m_ArrayData.team1[i].GetComponent<Character>().character_ID == 0)
            {
                m_ArrayData.team1[i].GetComponent<Character>().Copy_Character_Stat(MyGrids[gridNum - 1].transform.Find("Character_Prefab").gameObject);
                ArrayOrder(gridCharacter.gameObject);
                break;
            }
        }

        Inventory[inventoryNum - 1].GetComponent<Inventory_ID>().SetArrayed();

        if (MyTeamList.Count >= 5)
        {
            ReadyButton.gameObject.SetActive(true);
            return;
        }
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
        ReadyButton.gameObject.SetActive(false);

        Character cs = CancleCharacter.GetComponentInChildren<Character>();
        Arrayed_Data T = Arrayed_Data.instance;

        for(int i=0;i<7;i++)
        {
            Inventory_ID inven = Inventory[i].GetComponent<Inventory_ID>();
            if (inven.GetCharacterID()==cs.character_ID)
            {
                inven.SetNotArrayed();
            }
        }

        for (int i = 0; i < 5; i++)
        {
            if (cs.character_ID == T.team1[i].GetComponent<Character>().character_ID)
            {
                ArrayCancleButton.SetActive(false);
                ArrayOrder(T.team1[i]);
                T.team1[i].GetComponent<Character>().Character_Reset();
            }
        }
        CancleCharacter.tag = "Null_Character";
        cs.Character_Reset();
        cs.InitializeCharacterSprite();

    }

    void EnemySetting()//적 배치 -> 플레이어에게 정보 제공.
    {
        for (int i = 0; i < 5; i++)
        {
            int num = EnemyData[i].GetComponent<Character>().character_Num_Of_Grid-1;
            Debug.Log(num);
            Character Enemy = EnemyGrids[num].GetComponentInChildren<Character>();
            Enemy.Copy_Character_Stat(Arrayed_Data.instance.team2[i]);
            Enemy.InitializeCharacterSprite();
            EnemyGrids[num].tag = "Character";
            Debug.Log(EnemyGrids[num]);
        }

        EnemySpriteSetting();
    }

    void EnemySpriteSetting()
    {
        for(int i=0; i< 5;i++)
        {
            if (EnemyData != null)
            {
                int num = EnemyData[i].GetComponent<Character>().character_Num_Of_Grid - 1;
                Character Enemy = EnemyGrids[num].GetComponentInChildren<Character>();
                Enemy.spriteManager.SetPveCharacterSprite();
            }
        }


    }

    int[] RandomInt()
    {
        int[] Rand = new int[5];
        for (int i = 0; i < 5; i++)
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
        if (MyTeamList.Count == 5)
            return;
        click_inventory = true;
        InvenNum = Inventory[num - 1].GetComponent<Inventory_ID>().GetInventoryNum();
    }

    void GoBattle() {
        InitCustomProperties();
    }

    public bool GetIsClickInventory()
    {
        return click_inventory;
    }

    public void setIsClickInventory(bool isclick)
    {
        click_inventory = isclick;
    }

    public bool getisPopupOn()
    {
        return is_PopOn;
    }

    public void SetIsPopupOn(bool ispopupOn)
    {
        is_PopOn = ispopupOn;
    }

    private void InitCustomProperties() {
        bool result = false;

        ExitGames.Client.Photon.Hashtable table = new ExitGames.Client.Photon.Hashtable() { { "RoundWinCount", 0 } };
        result = PhotonNetwork.SetPlayerCustomProperties(table);
        if (!result) {
            Debug.LogError("PlayerCustomProperties 동기화 실패");
        }

        if (PhotonNetwork.IsMasterClient) {
            table = new ExitGames.Client.Photon.Hashtable() { { "RoundCount", 1 } };
            result = PhotonNetwork.CurrentRoom.SetCustomProperties(table);
            if (!result) {
                Debug.LogError("RoundCount 동기화 실패");
            }

            table = new ExitGames.Client.Photon.Hashtable() { { "IsPVE", true } };
            result = PhotonNetwork.CurrentRoom.SetCustomProperties(table);
            if (!result) {
                Debug.LogError("IsPVE 동기화 실패");
            }
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps) {
        syncCount++;

        if (syncCount >= 3) {
            PhotonNetwork.LoadLevel((int)Move_Scene.ENUM_SCENE.BATTLE_SCENE);
        }
    }

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged) {
        syncCount++;

        if (syncCount >= 3) {
            PhotonNetwork.LoadLevel((int)Move_Scene.ENUM_SCENE.BATTLE_SCENE);
        }
    }

    private void InitArrayData()
    {
        m_ArrayData = Arrayed_Data.instance;
        if (EnemyData == null)
        {
            //EnemyData = m_ArrayData.transform.GetChild(1).gameObject;
        }
    }

    public void ClearData()
    {
        for(int i=0;i<5;i++)
        {
            m_ArrayData.team1[i].GetComponent<Character>().Character_Reset();
        }
    }

    public void OnClickReady()
    {
        for(int i=0;i<5;i++)
        {
            m_ArrayData.team1[i].GetComponent<Character>().character_Attack_Order = MyTeamList[i].GetComponent<Character>().character_Attack_Order;
        }
    }
}
