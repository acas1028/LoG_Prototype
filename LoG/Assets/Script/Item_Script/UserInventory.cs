using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using CharacterStats;

// 유저가 가지고 있는 코인, 아이템 등 재화에 대한 클라이언트 변수(서버에 저장하기 전 임시로 가지고 있는 값)
public class UserInventory : MonoBehaviour
{
    private static int coin;
    public static int Coin { get => coin; }

    private static List<CharacterSkill> unlockedSkillList;
    public static List<CharacterSkill> UnlockedSkillList { get => unlockedSkillList; }

    private void Start() {
        if (!PlayFabClientAPI.IsClientLoggedIn())
            Debug.LogError("로그인이 필요합니다.");
    }

    public static void SetCoin(int value) {
        coin = value;
    }

    public static void SetUnlockedSkillList(List<CharacterSkill> list) {
        unlockedSkillList = list;
    }

    public static void SetUnlockedSkillList(List<int> list) {
        unlockedSkillList.Clear();

        foreach (var item in list) {
            unlockedSkillList.Add((CharacterSkill)item);
        }
    }
}
