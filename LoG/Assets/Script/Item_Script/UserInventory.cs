using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using CharacterStats;

// ������ ������ �ִ� ����, ������ �� ��ȭ�� ���� Ŭ���̾�Ʈ ����(������ �����ϱ� �� �ӽ÷� ������ �ִ� ��)
public class UserInventory : MonoBehaviour
{
    private static int coin;
    public static int Coin { get => coin; }

    private static List<CharacterSkill> unlockedSkillList;
    public static List<CharacterSkill> UnlockedSkillList { get => unlockedSkillList; }

    private void Start() {
        if (!PlayFabClientAPI.IsClientLoggedIn())
            Debug.LogError("�α����� �ʿ��մϴ�.");
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
