using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using CharacterStats;

// UserInventory에 있는 유저의 재화를 바탕으로 서버와 연동하는 곳
// 해킹의 위험이 있으므로 정보은닉을 최대한으로 하여 신중하게 작성하여야 함
public class UserDataSynchronizer : MonoBehaviour
{
    [SerializeField] int coin;
    [SerializeField] List<CharacterSkill> unlockedSkillList = new List<CharacterSkill>();
    Dictionary<CharacterSkill, string> unlockedSkillInstanceIdList = new Dictionary<CharacterSkill, string>();

    void OnEnable()
    {
        if (!PlayFabClientAPI.IsClientLoggedIn())
            Debug.LogError("로그인이 필요합니다.");
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
            print("인벤토리 불러오기 성공");
        }, (error) => print("인벤토리 불러오기 실패"));
    }
}
