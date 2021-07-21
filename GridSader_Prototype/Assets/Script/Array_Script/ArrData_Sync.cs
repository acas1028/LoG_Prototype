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
    public void DataSync()
    {
        if (PhotonNetwork.OfflineMode)
            return;

        Debug.Log("<color=yellow>DataSync 호출</color>");

        bool result = false;
        Character c;

        Hashtable team1_table = new Hashtable();

        for (int i = 0; i < 5; i++)
        {
            c = arrayed_Data.team1[i].GetComponent<Character>();
            team1_table.Add((i + 1) + "_ID", c.character_ID);
            team1_table.Add((i + 1) + "_IsAlive", c.character_Is_Allive);
            team1_table.Add((i + 1) + "_HP", c.character_HP);
            team1_table.Add((i + 1) + "_AP", c.character_AP);
            team1_table.Add((i + 1) + "_AttackDamage", c.character_Attack_Damage);
            team1_table.Add((i + 1) + "_AttackRange", c.character_Attack_Range);
            team1_table.Add((i + 1) + "_GridNumber", c.character_Num_Of_Grid);
            team1_table.Add((i + 1) + "_AttackOrder", c.character_Attack_Order);
        }

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
                c = arrayed_Data.team2[0].GetComponent<Character>();

                c.Character_Setting(1);
                c.character_Num_Of_Grid = gridNumSet[0];
                c.character_Attack_Order = 1;
                c.Debuging_Character();

                c = arrayed_Data.team2[1].GetComponent<Character>();

                c.Character_Setting(2);
                c.character_Num_Of_Grid = gridNumSet[1];
                c.character_Attack_Order = 2;
                c.Debuging_Character();
                break;
            case (int)ArrayPhase.SECOND34:
                c = arrayed_Data.team2[2].GetComponent<Character>();

                c.Character_Setting(3);
                c.character_Num_Of_Grid = gridNumSet[2];
                c.character_Attack_Order = 3;
                c.Debuging_Character();

                c = arrayed_Data.team2[3].GetComponent<Character>();

                c.Character_Setting(4);
                c.character_Num_Of_Grid = gridNumSet[3];
                c.character_Attack_Order = 4;
                c.Debuging_Character();
                break;
            case (int)ArrayPhase.SECOND5:
                c = arrayed_Data.team2[4].GetComponent<Character>();

                c.Character_Setting(5);
                c.character_Num_Of_Grid = gridNumSet[4];
                c.character_Attack_Order = 5;
                c.Debuging_Character();
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
        Character c;

        for (int i = 0; i < 5; i++)
        {
            // 서버에 있는 Team2의 Character_Action 정보를 여기 team2에 저장하는 과정

            // 상대가 접속하지 않았거나, Ready 버튼을 누르지 않은 상태에서는 컴포넌트를 가져올 수 없으므로 return 처리
            c = arrayed_Data.team2[i].GetComponent<Character>();
            if (!c)
                return;

            targetPlayer.CustomProperties.TryGetValue((i + 1) + "_ID", out o_id);
            targetPlayer.CustomProperties.TryGetValue((i + 1) + "_IsAlive", out o_isAlive);
            targetPlayer.CustomProperties.TryGetValue((i + 1) + "_HP", out o_hp);
            targetPlayer.CustomProperties.TryGetValue((i + 1) + "_AP", out o_ap);
            targetPlayer.CustomProperties.TryGetValue((i + 1) + "_AttackDamage", out o_attackDamage);
            targetPlayer.CustomProperties.TryGetValue((i + 1) + "_AttackRange", out o_attackRange);
            targetPlayer.CustomProperties.TryGetValue((i + 1) + "_GridNumber", out o_gridNumber);
            targetPlayer.CustomProperties.TryGetValue((i + 1) + "_AttackOrder", out o_attackOrder);

            c.character_ID = (int)o_id;
            c.character_Is_Allive = (bool)o_isAlive;
            c.character_HP = (int)o_hp;
            c.character_AP = (int)o_ap;
            c.character_Attack_Damage = (int)o_attackDamage;
            c.character_Attack_Range = (bool[])o_attackRange;
            c.character_Num_Of_Grid = (int)o_gridNumber;
            c.character_Attack_Order = (int)o_attackOrder;

            c.Debuging_Character();
        }
    }
    #endregion
}
