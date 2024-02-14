using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

using PlayFab;
using PlayFab.ClientModels;

using CharacterStats;

public class DeckDataSync : MonoBehaviour
{
    public static DeckDataSync Instance;
    bool isGetAllData;

    // Start is called before the first frame update
    void Awake()
    {
        if (!Instance)
            Instance = this;
        else
            Destroy(gameObject);

        if (!PlayFabClientAPI.IsClientLoggedIn())
            Debug.LogError("로그인이 필요합니다.");

        isGetAllData = false;

        GetData();
    }

    public void SetData(int pageNum, int deckIndex, Character character)
    {
        // 서버에 저장되는 Key 값 양식
        // 0_3_character_stats
        // 0은 페이지 번호 (맨 앞 페이지인 경우 0)
        // 3은 해당 페이지 내에서 캐릭터의 위치, 인덱스 번호 (맨 앞 인덱스인 경우 0)

        string character_id = character.character_ID.ToString();
        string character_type = ((int)character.character_Type).ToString();
        string character_skill = ((int)character.character_Skill).ToString();
        string character_hp = character.character_HP.ToString();
        string character_attack_damage = character.character_Attack_Damage.ToString();

        // 오버헤드를 줄이기 위하여 StringBuilder 사용
        StringBuilder sb = new StringBuilder();
        foreach (var item in character.character_Attack_Range)
        {
            sb.Append(item ? 1 : 0);
        }
        string character_attack_range = sb.ToString();

        // 저장되는 데이터의 Key와 Value값 예시
        // Key값: 0_5_character_stats
        // Value값: id14_type2_skill13_hp350_damage40_range111110101
        var data = new Dictionary<string, string>() {
            { pageNum + "_" + deckIndex + "_character_stats", string.Format("id{0}_type{1}_skill{2}_hp{3}_damage{4}_range{5}",
            character_id, character_type, character_skill, character_hp, character_attack_damage, character_attack_range) },
        };

        SendData(data);
    }

    private void SendData(Dictionary<string, string> data)
    {
        // Key 값 지우는 방법: value 값을 null 로 해준다.
        var request = new UpdateUserDataRequest() { Data = data, Permission = UserDataPermission.Private };
        PlayFabClientAPI.UpdateUserData(request,
            result =>
            {
                foreach (var item in request.Data)
                {
                    Debug.LogFormat("플레이어 데이터 저장 성공: {0} / {1}", item.Key, item.Value);
                }
            }, error => Debug.LogWarningFormat("플레이어 데이터 저장 실패: {0}", error.ErrorMessage)
        );
    }

    public void GetData()
    {
        int lastPageNum = -1;

        var request = new GetUserDataRequest();

        PlayFabClientAPI.GetUserData(request,
            result =>
            {
                foreach (var item in result.Data)
                {
                    if (item.Key == "lastPageNum")
                    {
                        lastPageNum = int.Parse(item.Value.Value);
                        Deck_Data_Send.instance.lastPageNum = lastPageNum;
                        Debug.Log("마지막 페이지 번호 불러오기: " + lastPageNum);
                    }
                    else if (item.Key.Contains("character_stats"))
                    {
                        int pageNum = (int)(item.Key[0] - '0');
                        int index = (int)(item.Key[2] - '0');
                        if (index == -1)
                        {
                            Debug.LogError("인덱스 번호가 -1 이므로 덱 정보를 불러올 수 없습니다.");
                            break;
                        }

                        Character character = Deck_Data_Send.instance.Save_Data[pageNum, index].GetComponent<Character>();
                        int indexOfId = item.Value.Value.IndexOf("id");
                        int indexOfType = item.Value.Value.IndexOf("type");
                        int indexOfSkill = item.Value.Value.IndexOf("skill");
                        int indexOfHp = item.Value.Value.IndexOf("hp");
                        int indexOfDamage = item.Value.Value.IndexOf("damage");
                        int indexOfRange = item.Value.Value.IndexOf("range");

                        character.character_ID = int.Parse(item.Value.Value.Substring(indexOfId + 2, indexOfType - indexOfId - 3));
                        character.character_Type = (CharacterType)Enum.Parse(typeof(CharacterType), item.Value.Value.Substring(indexOfType + 4, indexOfSkill - indexOfType - 5));
                        character.character_Skill = (CharacterSkill)Enum.Parse(typeof(CharacterSkill), item.Value.Value.Substring(indexOfSkill + 5, indexOfHp - indexOfSkill - 6));
                        character.character_HP = int.Parse(item.Value.Value.Substring(indexOfHp + 2, indexOfDamage - indexOfHp - 3));
                        character.character_Attack_Damage = int.Parse(item.Value.Value.Substring(indexOfDamage + 6, indexOfRange - indexOfDamage - 7));

                        string attack_range_temp = item.Value.Value.Substring(indexOfRange + 5);
                        for (int i = 0; i < 9; i++)
                        {
                            character.character_Attack_Range[i] = (attack_range_temp[i] != '0');
                        }
                        Debug.LogFormat("받은 데이터: {0} / {1}", item.Key, item.Value.Value);
                    }
                }
                isGetAllData = true;
            }, error => Debug.LogWarningFormat("데이터 불러오기 실패: {0}", error.ErrorMessage)
        );
    }

    public void SendLastPageNum(int lastPageNum)
    {
        // Key 값 지우는 방법: value 값을 null 로 해준다.
        var request = new UpdateUserDataRequest() { Data = new Dictionary<string, string>() { { "lastPageNum", lastPageNum.ToString() } }, Permission = UserDataPermission.Private };
        PlayFabClientAPI.UpdateUserData(request,
            result =>
            {
                foreach (var item in request.Data)
                {
                    Debug.LogFormat("마지막 페이지 번호 저장 성공: {0} / {1}", item.Key, item.Value);
                }
            }, error => Debug.LogWarningFormat("마지막 페이지 번호 저장 실패: {0}", error.ErrorMessage)
        );
    }

    public bool IsGetAllData()
    {
        return isGetAllData;
    }
}