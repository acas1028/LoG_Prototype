using System.Collections.Generic;

using UnityEngine;

using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;

public class ArrData_Sync : MonoBehaviourPunCallbacks
{
    public ArrRoomManager roomManager;
    public Arrayed_Data arrayed_Data;

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
    public void DataSync(GameObject[] passData)
    {
        if (PhotonNetwork.OfflineMode)
            return;

        Debug.Log("<color=yellow>DataSync 호출</color>");

        bool result = false;
        Character_Script cs;
        arrayed_Data.team1 = passData;

        Hashtable team1_table = new Hashtable();

        for (int i = 0; i < 5; i++)
        {
            cs = arrayed_Data.team1[i].GetComponent<Character_Script>();
            team1_table.Add((i + 1) + "_ID", cs.character_ID);
            team1_table.Add((i + 1) + "_IsAlive", cs.character_Is_Allive);
            team1_table.Add((i + 1) + "_HP", cs.character_HP);
            team1_table.Add((i + 1) + "_AP", cs.character_AP);
            team1_table.Add((i + 1) + "_AttackDamage", cs.character_Attack_Damage);
            team1_table.Add((i + 1) + "_AttackRange", cs.character_Attack_Range);
            team1_table.Add((i + 1) + "_GridNumber", cs.character_Num_Of_Grid);
            team1_table.Add((i + 1) + "_AttackOrder", cs.character_Attack_Order);
        }

        result = PhotonNetwork.LocalPlayer.SetCustomProperties(team1_table);
        if (!result)
            Debug.LogWarning("Team1 Custom Property 설정 실패");
    }

    public void SetArrayPhaseInOffline()
    {
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

        if (changedProps.ContainsKey("IsPreemptive") || changedProps.ContainsKey("PlayerIsReady"))
            return;

        if (targetPlayer == PhotonNetwork.LocalPlayer)
            return;

        Debug.LogFormat("Player <color=lightblue>#{0} {1}</color> Properties Updated due to <color=green>{2}</color>", targetPlayer.ActorNumber, targetPlayer.NickName, changedProps.ToString());

        object o_id;
        object o_isAlive;
        object o_hp;
        object o_ap;
        object o_attackDamage;
        object o_attackRange;
        object o_gridNumber;
        object o_attackOrder;
        Character_Script cs;

        for (int i = 0; i < 5; i++)
        {
            // 서버에 있는 Team2의 Character_Script 정보를 여기 team2에 저장하는 과정

            // 상대가 접속하지 않았거나, Ready 버튼을 누르지 않은 상태에서는 컴포넌트를 가져올 수 없으므로 return 처리
            cs = arrayed_Data.team2[i].GetComponent<Character_Script>();
            if (!cs)
                return;

            targetPlayer.CustomProperties.TryGetValue((i + 1) + "_ID", out o_id);
            targetPlayer.CustomProperties.TryGetValue((i + 1) + "_IsAlive", out o_isAlive);
            targetPlayer.CustomProperties.TryGetValue((i + 1) + "_HP", out o_hp);
            targetPlayer.CustomProperties.TryGetValue((i + 1) + "_AP", out o_ap);
            targetPlayer.CustomProperties.TryGetValue((i + 1) + "_AttackDamage", out o_attackDamage);
            targetPlayer.CustomProperties.TryGetValue((i + 1) + "_AttackRange", out o_attackRange);
            targetPlayer.CustomProperties.TryGetValue((i + 1) + "_GridNumber", out o_gridNumber);
            targetPlayer.CustomProperties.TryGetValue((i + 1) + "_AttackOrder", out o_attackOrder);

            cs.character_ID = (int)o_id;
            cs.character_Is_Allive = (bool)o_isAlive;
            cs.character_HP = (int)o_hp;
            cs.character_AP = (int)o_ap;
            cs.character_Attack_Damage = (int)o_attackDamage;
            cs.character_Attack_Range = (bool[])o_attackRange;
            cs.character_Num_Of_Grid = (int)o_gridNumber;
            cs.character_Attack_Order = (int)o_attackOrder;

            cs.Debuging_Character();
        }
    }
    #endregion
}
