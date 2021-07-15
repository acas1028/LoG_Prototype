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

        // Ű Ÿ���� string��, �� Ÿ���� int, bool �� �پ��� ������ �����ϴ� �ؽ����̺�
        // �ؽ����̺��� ��ųʸ��� �ٸ��� ���׸� Ÿ���� ������ ���� �ʾ� ���� ������ Ÿ�Կ� ������ ����. (object Ÿ������ �ν�) ������ �ڽ̰� ��ڽ� ������ �ʿ��ϴ�.
        // C# �ؽ����̺��� �⺻������ ���� �ؽ� ����� ����Ͽ� �����͸� �����Ѵ�.
        // �ؽ����̺��� ����ϴ� ������ Photon�� Custom Properties�� ����Ϸ��� Hashtable�� ����ؾ��ϱ� ����
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

    // Arrayment_Scene���� Ready ��ư�� ���� ��� ȣ����
    // Custom Properties �� �̿��Ͽ� ������ Team1�� Character_Script�� ����
    // https://doc.photonengine.com/ko-kr/pun/current/gameplay/synchronization-and-state : ����ȭ�ϴ� ��� 1.PhotonView 2.RPC 3.Custom Properties
    // https://doc.photonengine.com/ko-kr/pun/current/reference/serialization-in-photon : ������ �� �ִ� ������ Ÿ��
    public void DataSync(GameObject[] passData)
    {
        if (PhotonNetwork.OfflineMode)
        {
            SetArrayPhaseInOffline();
            return;
        }

        Debug.Log("<color=yellow>DataSync ȣ��</color>");

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
            Debug.Log("Team1 Custom Property ���� ����");
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
            Debug.Log("IsReady Custom Property ���� ����");
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

    #region ���� �ݹ� �Լ� : MonoBehaviourPunCallbacks Ŭ������ ����� �޴� �Լ�

    /// <summary>
    /// SetCustomProperties�� ���� �� �÷��̾��� Custom Property�� �ٲ� ��� ȣ��Ǵ� �ݹ� �Լ�
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
            // isReady_table �� �޾ƿ� ��쿡��
            if (changedProps.ContainsKey("PlayerIsReady"))
            {
                player.CustomProperties.TryGetValue("PlayerIsReady", out o_isEnemyReady);
                isEnemyReady = (bool)o_isEnemyReady;

                roomManager.SetIsEnemyReadyText(isEnemyReady);

                // ĳ���� ���� Property�� �ƴ� �غ� ���� Property�� �޾ƿ� ��� �Ʒ� ������ ��ŵ�Ѵ�.
                continue;
            }

            for (int i = 0; i < 5; i++)
            {
                // ������ �ִ� Team2�� Character_Script ������ ���� team2�� �����ϴ� ����

                // ��밡 �������� �ʾҰų�, Ready ��ư�� ������ ���� ���¿����� ������Ʈ�� ������ �� �����Ƿ� return ó��
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

        // �� �÷��̾� �غ� �Ϸ� �� ��ġ ����
        if (PhotonNetwork.IsMasterClient && isAllPlayerReady)
        {
            // roomManager.SetPreemptivePlayer();
            roomManager.StartArrayPhase();
            isReady = false;
        }
    }
    #endregion
}
