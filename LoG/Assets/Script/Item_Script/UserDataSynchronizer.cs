using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using PlayFab;
using PlayFab.ClientModels;
using CharacterStats;

// UserInventory�� �ִ� ������ ��ȭ�� �������� ������ �����ϴ� ��
// ��ŷ�� ������ �����Ƿ� ���������� �ִ������� �Ͽ� �����ϰ� �ۼ��Ͽ��� ��
public class UserDataSynchronizer : MonoBehaviour {
    [SerializeField] UIDataSynchronizer uiData;

    string nickname;
    int coin;
    public List<CharacterSkill> unlockedSkillList = new List<CharacterSkill>();
    Dictionary<CharacterSkill, string> unlockedSkillInstanceIdList = new Dictionary<CharacterSkill, string>();

    void OnEnable() {
        if (!PlayFabClientAPI.IsClientLoggedIn())
            Debug.LogError("�α����� �ʿ��մϴ�.");
        else
            GetUserDataFromServer();
    }

    public void GetUserDataFromServer() {
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), (result) => {
            nickname = result.AccountInfo.Username;
            uiData.SetNickName(nickname);
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

            uiData.SetCoin(coin.ToString());
            print("�κ��丮 �ҷ����� ����");
        }, (error) => print("�κ��丮 �ҷ����� ����"));
    }

    public string GetNickname() {
        return nickname;
    }

    public int GetCoin() {
        return coin;
    }

    public List<CharacterSkill> GetUnlockedSkillList() {
        return unlockedSkillList;
    }
}
