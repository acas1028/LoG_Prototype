using System.Collections.Generic;

using UnityEngine;

using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;

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

    #region �ܺο��� ȣ��Ǵ� public �Լ�
    public void DataSync(int index)
    {
        if (PhotonNetwork.OfflineMode)
            return;

        if (index < 0 || index > 4)
        {
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

    public void SetArrayPhaseInOffline()
    {
        Character c;
        switch (roomManager.GetArrayPhase())
        {
            case (int)ArrayPhase.SECOND12:
                c = Arrayed_Data.instance.team2[0].GetComponent<Character>();

                c.Character_Setting(1);
                c.character_Num_Of_Grid = gridNumSet[0];
                c.character_Attack_Order = 1;
                c.Debuging_Character();

                c = Arrayed_Data.instance.team2[1].GetComponent<Character>();

                c.Character_Setting(2);
                c.character_Num_Of_Grid = gridNumSet[1];
                c.character_Attack_Order = 2;
                c.Debuging_Character();
                break;
            case (int)ArrayPhase.SECOND34:
                c = Arrayed_Data.instance.team2[2].GetComponent<Character>();

                c.Character_Setting(3);
                c.character_Num_Of_Grid = gridNumSet[2];
                c.character_Attack_Order = 3;
                c.Debuging_Character();

                c = Arrayed_Data.instance.team2[3].GetComponent<Character>();

                c.Character_Setting(4);
                c.character_Num_Of_Grid = gridNumSet[3];
                c.character_Attack_Order = 4;
                c.Debuging_Character();
                break;
            case (int)ArrayPhase.SECOND5:
                c = Arrayed_Data.instance.team2[4].GetComponent<Character>();

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

    #region ���� �ݹ� �Լ� : MonoBehaviourPunCallbacks Ŭ������ ����� �޴� �Լ�

    /// <summary>
    /// SetCustomProperties�� ���� �� �÷��̾��� Custom Property�� �ٲ� ��� ȣ��Ǵ� �ݹ� �Լ�
    /// </summary>
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (PhotonNetwork.OfflineMode)
            return;

        if (changedProps.ContainsKey("IsPreemptive") || changedProps.ContainsKey("PlayerIsReady"))
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

            if(character_Arrayment_Showing.my_Count!= 0 && Arrayed_Data.instance.team1[character_Arrayment_Showing.my_Count-1].GetComponent<Character>().character_ID == 0)
            {
                Debug.Log("mine");
                character_Arrayment_Showing.cancel_Character = Arrayed_Data.instance.team1[character_Arrayment_Showing.my_Count - 1];
                character_Arrayment_Showing.is_Mine = true;
                character_Arrayment_Showing.Set_AttackRange_Ui(false);
                character_Arrayment_Showing.my_Count--;
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

        // ������ �ִ� Team2�� Character_Action ������ ���� team2�� �����ϴ� ����

        targetPlayer.CustomProperties.TryGetValue("Character_Index", out o_index);
        int index = (int)o_index;

        // ��밡 �������� �ʾҰų�, Ready ��ư�� ������ ���� ���¿����� ������Ʈ�� ������ �� �����Ƿ� return ó��
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
        c.character_Type = (Character.Type)o_type;
        c.character_Skill = (Character.Skill)o_skill;
        c.character_Is_Allive = (bool)o_isAlive;
        c.character_HP = (int)o_hp;
        c.character_AP = (int)o_ap;
        c.character_Attack_Damage = (int)o_attackDamage;
        c.character_Attack_Range = (bool[])o_attackRange;
        c.character_Num_Of_Grid = (int)o_gridNumber;
        c.character_Attack_Order = (int)o_attackOrder;
        is_datasync = true;
        c.Debuging_Character();


        if (Arrayed_Data.instance.team2[character_Arrayment_Showing.Oppenent_Count].GetComponent<Character>().character_ID != 0)
        {
            Debug.Log("Not mine");
            character_Arrayment_Showing.Character_List.Add(Arrayed_Data.instance.team2[character_Arrayment_Showing.Oppenent_Count]);
            character_Arrayment_Showing.is_Sprite_Change = true;
            character_Arrayment_Showing.is_Mine = false;
            character_Arrayment_Showing.Set_AttackRange_Ui(true);
            character_Arrayment_Showing.Oppenent_Count++;
        }
        if (character_Arrayment_Showing.my_Count != 0 && Arrayed_Data.instance.team2[character_Arrayment_Showing.Oppenent_Count - 1].GetComponent<Character>().character_ID == 0)
        {
            Debug.Log("Not mine");
            character_Arrayment_Showing.cancel_Character = Arrayed_Data.instance.team2[character_Arrayment_Showing.Oppenent_Count - 1];
            character_Arrayment_Showing.is_Mine = false;
            character_Arrayment_Showing.Set_AttackRange_Ui(false);
            character_Arrayment_Showing.Oppenent_Count--;
        }
    }
    #endregion
}

