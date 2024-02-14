using System.Collections.Generic;

using UnityEngine;

using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;

using CharacterStats;

public class ArrData_Sync : MonoBehaviourPunCallbacks
{
    public ArrRoomManager roomManager;
    public bool is_datasync = false;
    public Character_arrayment_showing character_Arrayment_Showing;
    List<int> gridNumSet;

    private void Start()
    {
        if (PhotonNetwork.OfflineMode)
        {
            gridNumSet = new List<int>();
            for (int i = 0; i < 9; i++)
            {
                gridNumSet.Add(i + 1);
            }

            ShuffleList<int>(gridNumSet);

            return;
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

    #region 외부에서 호출되는 public 함수
    public void DataSync(int index)
    {
        if (PhotonNetwork.OfflineMode)
            return;

        if (index < 0 || index > 4)
        {
            Debug.LogError("DataSync 실패: 캐릭터 인덱스는 0이상 4이하여야 합니다.");
            return;
        }    

        Debug.Log("<color=yellow>DataSync 호출</color>");

        bool result = false;
        Character c;

        Hashtable team1_table = new Hashtable();

        c = Arrayed_Data.instance.team1[index].GetComponent<Character>();
        team1_table.Add("Character_Index", index);

        team1_table.Add(index + "_ID", c.character_ID);
        team1_table.Add(index + "_Type", c.character_Type);
        team1_table.Add(index + "_Skill", c.character_Skill);
        team1_table.Add(index + "_IsAlive", c.character_Is_Allive);
        team1_table.Add(index + "_HP", c.character_HP);
        team1_table.Add(index + "_AP", c.character_AP);
        team1_table.Add(index + "_AttackDamage", c.character_Attack_Damage);
        team1_table.Add(index + "_AttackRange", c.character_Attack_Range);
        team1_table.Add(index + "_GridNumber", c.character_Num_Of_Grid);
        team1_table.Add(index + "_AttackOrder", c.character_Attack_Order);

        result = PhotonNetwork.LocalPlayer.SetCustomProperties(team1_table);
        if (!result)
            Debug.LogWarning("Team1 Custom Property 설정 실패");
    }

    public void SetArrayPhaseInOffline()
    {
        Character c;
        switch (roomManager.GetArrayPhase())
        {
            case (int)ArrayPhase.SECOND12:
                c = Arrayed_Data.instance.team2[0].GetComponent<Character>();

                c.Character_Setting(1);
                c.character_Num_Of_Grid = gridNumSet[0];
                c.character_Attack_Range = new bool[] { true, true, false, true, false, false, false, true, false };
                c.character_Attack_Order = 1;

                c = Arrayed_Data.instance.team2[1].GetComponent<Character>();

                c.Character_Setting(2);
                c.character_Num_Of_Grid = gridNumSet[1];
                c.character_Attack_Range = new bool[] { false, false, false, false, false, true, false, true, true };
                c.character_Attack_Order = 2;
                break;
            case (int)ArrayPhase.SECOND34:
                c = Arrayed_Data.instance.team2[2].GetComponent<Character>();

                c.Character_Setting(3);
                c.character_Num_Of_Grid = gridNumSet[2];
                c.character_Attack_Range = new bool[] { false, true, false, false, true, false, false, true, false };
                c.character_Attack_Order = 3;

                c = Arrayed_Data.instance.team2[3].GetComponent<Character>();

                c.Character_Setting(4);
                c.character_Num_Of_Grid = gridNumSet[3];
                c.character_Attack_Range = new bool[] { true, false, false, true, false, false, true, false, false };
                c.character_Attack_Order = 4;
                break;
            case (int)ArrayPhase.SECOND5:
                c = Arrayed_Data.instance.team2[4].GetComponent<Character>();

                c.Character_Setting(5);
                c.character_Num_Of_Grid = gridNumSet[4];
                c.character_Attack_Range = new bool[] { false, false, false, true, true, true, false, false, false };
                c.character_Attack_Order = 5;
                break;
            default:
                break;
        }

        roomManager.StartArrayPhase();
    }
    #endregion

    #region 포톤 콜백 함수 : MonoBehaviourPunCallbacks 클래스의 상속을 받는 함수

    /// <summary>
    /// SetCustomProperties를 통해 각 플레이어의 Custom Property가 바뀐 경우 호출되는 콜백 함수
    /// </summary>
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (PhotonNetwork.OfflineMode)
            return;

        if (changedProps.ContainsKey("IsPreemptive") || changedProps.ContainsKey("RoundWinCount"))
            return;

        if (targetPlayer == PhotonNetwork.LocalPlayer)
        {
            if (Arrayed_Data.instance.team1[character_Arrayment_Showing.my_Count].GetComponent<Character>().character_ID != 0)
            {
                Debug.Log("mine");
                character_Arrayment_Showing.Character_List.Add(Arrayed_Data.instance.team1[character_Arrayment_Showing.my_Count]);
                character_Arrayment_Showing.is_Sprite_Change = true;
                character_Arrayment_Showing.is_Mine = true;
                character_Arrayment_Showing.Set_AttackRange_Ui(true);
                character_Arrayment_Showing.my_Count++;
            }

            if (character_Arrayment_Showing.my_Count != 0 && Arrayed_Data.instance.team1[character_Arrayment_Showing.my_Count - 1].GetComponent<Character>().character_ID == 0)
            {
                character_Arrayment_Showing.is_Mine = true;
            }


            return;
        }

        Debug.LogFormat("Player <color=lightblue>#{0} {1}</color> Properties Updated due to <color=green>{2}</color>", targetPlayer.ActorNumber, targetPlayer.NickName, changedProps.ToString());

        object o_index;

        object o_id;
        object o_type;
        object o_skill;
        object o_isAlive;
        object o_hp;
        object o_ap;
        object o_attackDamage;
        object o_attackRange;
        object o_gridNumber;
        object o_attackOrder;
        Character c;

        // 서버에 있는 Team2의 Character_Action 정보를 여기 team2에 저장하는 과정

        targetPlayer.CustomProperties.TryGetValue("Character_Index", out o_index);
        int index = (int)o_index;

        // 상대가 접속하지 않았거나, Ready 버튼을 누르지 않은 상태에서는 컴포넌트를 가져올 수 없으므로 return 처리
        c = Arrayed_Data.instance.team2[index].GetComponent<Character>();
        if (!c)
            return;

        targetPlayer.CustomProperties.TryGetValue(index + "_ID", out o_id);
        targetPlayer.CustomProperties.TryGetValue(index + "_Type", out o_type);
        targetPlayer.CustomProperties.TryGetValue(index + "_Skill", out o_skill);
        targetPlayer.CustomProperties.TryGetValue(index + "_IsAlive", out o_isAlive);
        targetPlayer.CustomProperties.TryGetValue(index + "_HP", out o_hp);
        targetPlayer.CustomProperties.TryGetValue(index + "_AP", out o_ap);
        targetPlayer.CustomProperties.TryGetValue(index + "_AttackDamage", out o_attackDamage);
        targetPlayer.CustomProperties.TryGetValue(index + "_AttackRange", out o_attackRange);
        targetPlayer.CustomProperties.TryGetValue(index + "_GridNumber", out o_gridNumber);
        targetPlayer.CustomProperties.TryGetValue(index + "_AttackOrder", out o_attackOrder);

        c.character_ID = (int)o_id;
        c.character_Type = (CharacterType)o_type;
        c.character_Skill = (CharacterSkill)o_skill;
        c.character_Is_Allive = (bool)o_isAlive;
        c.character_HP = (int)o_hp;
        c.character_AP = (int)o_ap;
        c.character_Attack_Damage = (int)o_attackDamage;
        c.character_Attack_Range = (bool[])o_attackRange;
        c.character_Num_Of_Grid = (int)o_gridNumber;
        c.character_Attack_Order = (int)o_attackOrder;
        is_datasync = true;


        if (Arrayed_Data.instance.team2[character_Arrayment_Showing.Oppenent_Count].GetComponent<Character>().character_ID != 0)
        {
            Debug.Log("Not mine");
            character_Arrayment_Showing.Character_List.Add(Arrayed_Data.instance.team2[character_Arrayment_Showing.Oppenent_Count]);
            character_Arrayment_Showing.is_Sprite_Change = true;
            character_Arrayment_Showing.is_Mine = false;
            character_Arrayment_Showing.Set_AttackRange_Ui(true);
            character_Arrayment_Showing.Oppenent_Count++;
        }
        if (character_Arrayment_Showing.Oppenent_Count != 0 && Arrayed_Data.instance.team2[character_Arrayment_Showing.Oppenent_Count - 1].GetComponent<Character>().character_ID == 0)
        {
            character_Arrayment_Showing.is_Mine = false;
        }
    }
    #endregion
}

