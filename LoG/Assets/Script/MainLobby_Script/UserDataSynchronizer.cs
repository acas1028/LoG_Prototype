using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using PlayFab;
using PlayFab.ClientModels;
using CharacterStats;

// UserInventory�� �ִ� ������ ��ȭ�� �������� ������ �����ϴ� ��
public class UserDataSynchronizer : Singleton<UserDataSynchronizer> {

    public bool isAllDataLoaded;

    public string nickname;
    public int coin;
    public List<CharacterSkill> unlockedSkillList = new List<CharacterSkill>();
    public Dictionary<CharacterSkill, string> unlockedSkillInstanceIdList = new Dictionary<CharacterSkill, string>();

    void OnEnable() {
        isAllDataLoaded = false;

        if (!PlayFabClientAPI.IsClientLoggedIn())
            Debug.LogError("�α����� �ʿ��մϴ�.");
        else
            GetUserDataFromServer();
    }

    public void GetUserDataFromServer() {
        bool result1 = false;
        bool result2 = false;

        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), (result) => {
            nickname = result.AccountInfo.Username;
            result1 = true;
            isAllDataLoaded = result1 && result2;
        }, (error) => print("���� ���� �ҷ����� ����"));

        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), (result) => {
            coin = result.VirtualCurrency["CO"];
            unlockedSkillList.Clear();
            unlockedSkillInstanceIdList.Clear();

            for (int i = 0; i < result.Inventory.Count; i++) {
                var item = result.Inventory[i];
                if (item.ItemId.StartsWith("SKILL_")) {
                    CharacterSkill characterSkill = (CharacterSkill)Enum.Parse(typeof(CharacterSkill), item.ItemId.Substring(6));
                    unlockedSkillList.Add(characterSkill);
                    unlockedSkillInstanceIdList.Add(characterSkill, item.ItemInstanceId);
                }
            }
            print("�κ��丮 �ҷ����� ����");
            result2 = true;
            isAllDataLoaded = result1 && result2;
        }, (error) => print("�κ��丮 �ҷ����� ����"));
    }
}
