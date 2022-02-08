using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using PlayFab;
using PlayFab.ClientModels;
using CharacterStats;

// UserInventory에 있는 유저의 재화를 바탕으로 서버와 연동하는 곳
public class UserDataSynchronizer : Singleton<UserDataSynchronizer> {

    public bool isAllDataLoaded;

    public string nickname;
    public int coin;
    public List<CharacterSkill> unlockedSkillList = new List<CharacterSkill>();
    public Dictionary<CharacterSkill, string> unlockedSkillInstanceIdList = new Dictionary<CharacterSkill, string>();

    void OnEnable() {
        isAllDataLoaded = false;

        if (!PlayFabClientAPI.IsClientLoggedIn())
            Debug.LogError("로그인이 필요합니다.");
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
        }, (error) => print("계정 정보 불러오기 실패"));

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
            print("인벤토리 불러오기 성공");
            result2 = true;
            isAllDataLoaded = result1 && result2;
        }, (error) => print("인벤토리 불러오기 실패"));
    }
}
