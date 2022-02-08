using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using PlayFab;
using PlayFab.ClientModels;
using CharacterStats;

// UserInventory에 있는 유저의 재화를 바탕으로 서버와 연동하는 곳
// 해킹의 위험이 있으므로 정보은닉을 최대한으로 하여 신중하게 작성하여야 함
public class UserDataSynchronizer : MonoBehaviour {
    [SerializeField] UIDataSynchronizer uiData;

    string nickname;
    int coin;
    public List<CharacterSkill> unlockedSkillList = new List<CharacterSkill>();
    Dictionary<CharacterSkill, string> unlockedSkillInstanceIdList = new Dictionary<CharacterSkill, string>();

    void OnEnable() {
        if (!PlayFabClientAPI.IsClientLoggedIn())
            Debug.LogError("로그인이 필요합니다.");
        else
            GetUserDataFromServer();
    }

    public void GetUserDataFromServer() {
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), (result) => {
            nickname = result.AccountInfo.Username;
            uiData.SetNickName(nickname);
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

            uiData.SetCoin(coin.ToString());
            print("인벤토리 불러오기 성공");
        }, (error) => print("인벤토리 불러오기 실패"));
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
