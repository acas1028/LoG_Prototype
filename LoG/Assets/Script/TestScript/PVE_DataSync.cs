using System.Collections.Generic;

using UnityEngine;

using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;

public class PVE_DataSync : MonoBehaviourPunCallbacks {
    public PVE_RoomManager roomManager;

    #region �ܺο��� ȣ��Ǵ� public �Լ�
    public void DataSync(int index) {
        if (PhotonNetwork.OfflineMode)
            return;

        if (index < 0 || index > 4) {
            Debug.LogError("DataSync ����: ĳ���� �ε����� 0�̻� 4���Ͽ��� �մϴ�.");
            return;
        }

        Debug.Log("<color=yellow>DataSync ȣ��</color>");

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
            Debug.LogWarning("Team1 Custom Property ���� ����");
    }
    #endregion
}

