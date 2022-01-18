using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using CharacterStats;

// UserInventory�� �ִ� ������ ��ȭ�� �������� ������ �����ϴ� ��
// ��ŷ�� ������ �����Ƿ� ���������� �ִ������� �Ͽ� �����ϰ� �ۼ��Ͽ��� ��
public class UserDataSynchronizer : MonoBehaviour
{
    [SerializeField] int coin;
    [SerializeField] List<CharacterSkill> unlockedSkillList = new List<CharacterSkill>();
    Dictionary<CharacterSkill, string> unlockedSkillInstanceIdList = new Dictionary<CharacterSkill, string>();

    void OnEnable()
    {
        if (!PlayFabClientAPI.IsClientLoggedIn())
            Debug.LogError("�α����� �ʿ��մϴ�.");
        else
            GetUserDataFromServer();
    }

    void GetUserDataFromServer() {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), (result) => {
            coin = result.VirtualCurrency["CO"];
            unlockedSkillList.Clear();
            unlockedSkillInstanceIdList.Clear();

            for (int i = 0; i < result.Inventory.Count; i++) {
                var inven = result.Inventory[i];
                switch (inven.ItemId) {
                    case "Skill_Attack_Executioner":
                        unlockedSkillList.Add(CharacterSkill.Attack_Executioner);
                        unlockedSkillInstanceIdList.Add(CharacterSkill.Attack_Executioner, inven.ItemInstanceId);
                        break;
                    case "Skill_Defense_Disarm":
                        unlockedSkillList.Add(CharacterSkill.Defense_Disarm);
                        unlockedSkillInstanceIdList.Add(CharacterSkill.Defense_Disarm, inven.ItemInstanceId);
                        break;
                    default:
                        break;
                }
            }
            print("�κ��丮 �ҷ����� ����");
        }, (error) => print("�κ��丮 �ҷ����� ����"));
    }
}
