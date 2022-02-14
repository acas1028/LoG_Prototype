using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using PlayFab;
using PlayFab.ClientModels;
using CharacterStats;

using Photon.Pun;

// UserInventory�� �ִ� ������ ��ȭ�� �������� ������ �����ϴ� ��
public class UserDataSynchronizer : Singleton<UserDataSynchronizer> {
    [SerializeField] UIDataSynchronizer dataSynchronizer;
    [HideInInspector] public string nickname;
    [HideInInspector] public int coin;
    [HideInInspector] public List<CharacterSkill> unlockedSkillList = new List<CharacterSkill>();
    [HideInInspector] public Dictionary<CharacterSkill, string> unlockedSkillInstanceIdList = new Dictionary<CharacterSkill, string>();

    void OnEnable() {
        if (!PlayFabClientAPI.IsClientLoggedIn())
            Debug.LogError("�α����� �ʿ��մϴ�.");
        else
            GetUserDataFromServer();
    }

    public void GetUserDataFromServer() {
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), (result) => {
            nickname = result.AccountInfo.Username;
            PhotonNetwork.LocalPlayer.NickName = nickname;

            dataSynchronizer.UpdateAccountInfo();
            print("���� ���� �ҷ����� ����");
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

            dataSynchronizer.UpdateUserInventory();
            print("�κ��丮 �ҷ����� ����");

        }, (error) => print("�κ��丮 �ҷ����� ����"));
    }
}
