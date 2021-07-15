using System.Collections.Generic;

using UnityEngine;

using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;

public class ArrData_Sync : MonoBehaviourPunCallbacks
{
    public ArrRoomManager roomManager;
    public Arrayed_Data arrayed_Data;

    private Hashtable team1_table;
    private Hashtable isReady_table;

    private bool isReady;
    private bool isEnemyReady;

    List<int> gridNumSet;

    private void Start()
    {
        isReady = false;
        isEnemyReady = false;

        gridNumSet = new List<int>();
        for (int i = 0; i < 9; i++)
        {
            gridNumSet.Add(i + 1);
        }

        ShuffleList<int>(gridNumSet);

        if (PhotonNetwork.OfflineMode)
            return;

        // 키 타입은 string형, 값 타입은 int, bool 등 다양한 형으로 저장하는 해시테이블
        // 해시테이블은 딕셔너리와 다르게 제네릭 타입이 정해져 있지 않아 들어가는 데이터 타입에 제한이 없다. (object 타입으로 인식) 하지만 박싱과 언박싱 과정이 필요하다.
        // C# 해시테이블은 기본적으로 이중 해싱 방법을 사용하여 데이터를 저장한다.
        // 해시테이블을 사용하는 이유는 Photon의 Custom Properties를 사용하려면 Hashtable을 사용해야하기 때문
        team1_table = new Hashtable();
        isReady_table = new Hashtable();
        isReady_table.Add("PlayerIsReady", false);
        PhotonNetwork.LocalPlayer.SetCustomProperties(isReady_table);

        for (int i = 0; i < 5; i++)
        {
            team1_table.Add((i + 1) + "_ID", null);
            team1_table.Add((i + 1) + "_IsAlive", null);
            team1_table.Add((i + 1) + "_HP", null);
            team1_table.Add((i + 1) + "_AP", null);
            team1_table.Add((i + 1) + "_AttackDamage", null);
            team1_table.Add((i + 1) + "_AttackRange", null);
            team1_table.Add((i + 1) + "_GridNumber", null);
            team1_table.Add((i + 1) + "_AttackOrder", null);
            team1_table.Add((i + 1) + "_AttackCount", null);
            team1_table.Add((i + 1) + "_Damaged", null);
            team1_table.Add((i + 1) + "_BuffedAttack", null);
            team1_table.Add((i + 1) + "_BuffedDamaged", null);
            team1_table.Add((i + 1) + "_DivineShield", null);
            team1_table.Add((i + 1) + "_Revivial", null);
        }
    }

    // Start is called before the first frame update

    // Arrayment_Scene에서 Ready 버튼을 누른 즉시 호출함
    // Custom Properties 를 이용하여 서버에 Team1의 Character_Script를 전송
    // https://doc.photonengine.com/ko-kr/pun/current/gameplay/synchronization-and-state : 동기화하는 방법 1.PhotonView 2.RPC 3.Custom Properties
    // https://doc.photonengine.com/ko-kr/pun/current/reference/serialization-in-photon : 전송할 수 있는 데이터 타입
    public void DataSync(GameObject[] passData)
    {
        if (PhotonNetwork.OfflineMode)
        {
            SetArrayPhaseInOffline();
            return;
        }

        Debug.Log("<color=yellow>DataSync 호출</color>");

        bool result = false;
        Character_Script cs;
        arrayed_Data.team1 = passData;

        for (int i = 0; i < 5; i++)
        {
            cs = arrayed_Data.team1[i].GetComponent<Character_Script>();
            team1_table[(i + 1) + "_ID"] = cs.character_ID;
            team1_table[(i + 1) + "_IsAlive"] = cs.character_Is_Allive;
            team1_table[(i + 1) + "_HP"] = cs.character_HP;
            team1_table[(i + 1) + "_AP"] = cs.character_AP;
            team1_table[(i + 1) + "_AttackDamage"] = cs.character_Attack_Damage;
            team1_table[(i + 1) + "_AttackRange"] = cs.character_Attack_Range;
            team1_table[(i + 1) + "_GridNumber"] = cs.character_Num_Of_Grid;
            team1_table[(i + 1) + "_AttackOrder"] = cs.character_Attack_Order;
            team1_table[(i + 1) + "_AttackCount"] = cs.character_Attack_Count;
            team1_table[(i + 1) + "_Damaged"] = cs.character_Damaged;
            team1_table[(i + 1) + "_BuffedAttack"] = cs.character_Buffed_Attack;
            team1_table[(i + 1) + "_BuffedDamaged"] = cs.character_Buffed_Damaged;
            team1_table[(i + 1) + "_DivineShield"] = cs.character_Divine_Shield;
            team1_table[(i + 1) + "_Revivial"] = cs.character_Revivial;
        }

        result = PhotonNetwork.LocalPlayer.SetCustomProperties(team1_table);
        if (!result)
            Debug.Log("Team1 Custom Property 설정 실패");
    }

    public void SetReady()
    {
        if (PhotonNetwork.OfflineMode)
        {
            roomManager.StartArrayPhase();
            return;
        }

        bool result = false;
        isReady = !isReady;
        roomManager.SetReadyButtonStatus(isReady);

        isReady_table["PlayerIsReady"] = isReady;

        result = PhotonNetwork.LocalPlayer.SetCustomProperties(isReady_table);
        if (!result)
            Debug.Log("IsReady Custom Property 설정 실패");
    }

    private void SetArrayPhaseInOffline()
    {
        roomManager.StartArrayPhase();

        Character_Script cs;
        switch (roomManager.GetArrayPhase())
        {
            case (int)ArrayPhase.SECOND12:
                cs = arrayed_Data.team2[0].GetComponent<Character_Script>();

                cs.Character_Setting(1);
                cs.character_Num_Of_Grid = gridNumSet[0];
                cs.character_Attack_Order = 1;
                cs.Debuging_Character();

                cs = arrayed_Data.team2[1].GetComponent<Character_Script>();

                cs.Character_Setting(2);
                cs.character_Num_Of_Grid = gridNumSet[1];
                cs.character_Attack_Order = 2;
                cs.Debuging_Character();
                break;
            case (int)ArrayPhase.SECOND34:
                cs = arrayed_Data.team2[2].GetComponent<Character_Script>();

                cs.Character_Setting(3);
                cs.character_Num_Of_Grid = gridNumSet[2];
                cs.character_Attack_Order = 3;
                cs.Debuging_Character();

                cs = arrayed_Data.team2[3].GetComponent<Character_Script>();

                cs.Character_Setting(4);
                cs.character_Num_Of_Grid = gridNumSet[3];
                cs.character_Attack_Order = 4;
                cs.Debuging_Character();
                break;
            case (int)ArrayPhase.SECOND5:
                cs = arrayed_Data.team2[4].GetComponent<Character_Script>();

                cs.Character_Setting(5);
                cs.character_Num_Of_Grid = gridNumSet[4];
                cs.character_Attack_Order = 5;
                cs.Debuging_Character();
                break;
            default:
                break;
        }
    }

    private List<T> ShuffleList<T>(List<T> list)
    {
        int random1, random2;
        T temp;

        for (int i = 0; i < list.Count; ++i)
        {
            random1 = Random.Range(0, list.Count);
            random2 = Random.Range(0, list.Count);

            temp = list[random1];
            list[random1] = list[random2];
            list[random2] = temp;
        }

        return list;
    }

    #region 포톤 콜백 함수 : MonoBehaviourPunCallbacks 클래스의 상속을 받는 함수

    /// <summary>
    /// SetCustomProperties를 통해 각 플레이어의 Custom Property가 바뀐 경우 호출되는 콜백 함수
    /// </summary>
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (PhotonNetwork.OfflineMode)
            return;

        object o_isEnemyReady;
        bool isAllPlayerReady;

        object o_id;
        object o_isAlive;
        object o_hp;
        object o_ap;
        object o_attackDamage;
        object o_attackRange;
        object o_gridNumber;
        object o_attackOrder;
        object o_attackCount;
        object o_damaged;
        object o_buffedAttack;
        object o_buffedDamaged;
        object o_divineShield;
        object o_revivial;
        Character_Script cs;

        foreach (Player player in PhotonNetwork.PlayerListOthers)
        {
            // isReady_table 을 받아온 경우에만
            if (changedProps.ContainsKey("PlayerIsReady"))
            {
                player.CustomProperties.TryGetValue("PlayerIsReady", out o_isEnemyReady);
                isEnemyReady = (bool)o_isEnemyReady;

                roomManager.SetIsEnemyReadyText(isEnemyReady);

                // 캐릭터 정보 Property가 아닌 준비 여부 Property를 받아온 경우 아래 과정을 스킵한다.
                continue;
            }

            for (int i = 0; i < 5; i++)
            {
                // 서버에 있는 Team2의 Character_Script 정보를 여기 team2에 저장하는 과정

                // 상대가 접속하지 않았거나, Ready 버튼을 누르지 않은 상태에서는 컴포넌트를 가져올 수 없으므로 return 처리
                cs = arrayed_Data.team2[i].GetComponent<Character_Script>();
                if (!cs)
                    return;

                player.CustomProperties.TryGetValue((i + 1) + "_ID", out o_id);
                player.CustomProperties.TryGetValue((i + 1) + "_IsAlive", out o_isAlive);
                player.CustomProperties.TryGetValue((i + 1) + "_HP", out o_hp);
                player.CustomProperties.TryGetValue((i + 1) + "_AP", out o_ap);
                player.CustomProperties.TryGetValue((i + 1) + "_AttackDamage", out o_attackDamage);
                player.CustomProperties.TryGetValue((i + 1) + "_AttackRange", out o_attackRange);
                player.CustomProperties.TryGetValue((i + 1) + "_GridNumber", out o_gridNumber);
                player.CustomProperties.TryGetValue((i + 1) + "_AttackOrder", out o_attackOrder);
                player.CustomProperties.TryGetValue((i + 1) + "_AttackCount", out o_attackCount);
                player.CustomProperties.TryGetValue((i + 1) + "_Damaged", out o_damaged);
                player.CustomProperties.TryGetValue((i + 1) + "_BuffedAttack", out o_buffedAttack);
                player.CustomProperties.TryGetValue((i + 1) + "_BuffedDamaged", out o_buffedDamaged);
                player.CustomProperties.TryGetValue((i + 1) + "_DivineShield", out o_divineShield);
                player.CustomProperties.TryGetValue((i + 1) + "_Revivial", out o_revivial);

                cs.character_ID = (int)o_id;
                cs.character_Is_Allive = (bool)o_isAlive;
                cs.character_HP = (int)o_hp;
                cs.character_AP = (int)o_ap;
                cs.character_Attack_Damage = (int)o_attackDamage;
                cs.character_Attack_Range = (bool[])o_attackRange;
                cs.character_Num_Of_Grid = (int)o_gridNumber;
                cs.character_Attack_Order = (int)o_attackOrder;
                cs.character_Attack_Count = (int)o_attackCount;
                cs.character_Damaged = (int)o_damaged;
                cs.character_Buffed_Attack = (int)o_buffedAttack;
                cs.character_Buffed_Damaged = (int)o_buffedDamaged;
                cs.character_Divine_Shield = (bool)o_divineShield;
                cs.character_Revivial = (bool)o_revivial;

                cs.Debuging_Character();
            }
        }

        isAllPlayerReady = isReady && isEnemyReady;

        Debug.LogFormat("Player Properties Updated due to <color=green>{0}</color>", changedProps.ToString());

        // 두 플레이어 준비 완료 후 배치 시작
        if (PhotonNetwork.IsMasterClient && isAllPlayerReady)
        {
            // roomManager.SetPreemptivePlayer();
            roomManager.StartArrayPhase();
            isReady = false;
        }
    }
    #endregion
}
