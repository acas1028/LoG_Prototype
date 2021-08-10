using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

using PlayFab;
using PlayFab.ClientModels;

public class DeckDataSync : MonoBehaviour
{
    private string playfabId;

    private string character_id;
    private string character_type;
    private string character_skill;
    private string character_hp;
    private string character_attack_damage;
    private string character_attack_range;

    // Start is called before the first frame update
    void Awake()
    {
        playfabId = PlayerPrefs.GetString("PlayFabId");
        if (playfabId == string.Empty)
            Debug.LogError("로그인을 먼저 하십시오.");
    }

    public void SetData(int pageNum, int deckIndex, Character character)
    {
        // 서버에 저장되는 Key 값 양식
        // 0_3_character_type
        // 0은 페이지 번호 (맨 앞 페이지인 경우 0)
        // 3은 해당 페이지 내에서 캐릭터의 위치, 인덱스 번호 (맨 앞 인덱스인 경우 0)

        character_id = character.character_ID.ToString();
        character_type = ((int)character.character_Type).ToString();
        character_skill = ((int)character.character_Skill).ToString();
        character_hp = character.character_HP.ToString();
        character_attack_damage = character.character_Attack_Damage.ToString();

        // 오버헤드를 줄이기 위하여 StringBuilder 사용
        StringBuilder sb = new StringBuilder();
        foreach (var item in character.character_Attack_Range)
        {
            sb.Append(item ? 1 : 0);
        }
        character_attack_range = sb.ToString();

        var data = new Dictionary<string, string>() {
            { pageNum + "_" + deckIndex + "_character_id", character_id },
            { pageNum + "_" + deckIndex + "_character_type", character_type },
            { pageNum + "_" + deckIndex + "_character_skill", character_skill },
            { pageNum + "_" + deckIndex + "_character_hp", character_hp },
            { pageNum + "_" + deckIndex + "_character_attack_damage", character_attack_damage },
            { pageNum + "_" + deckIndex + "_character_attack_range", character_attack_range }
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

    public Character GetData(int pageNum, int deckIndex)
    {
        var request = new GetUserDataRequest() { PlayFabId = playfabId };
        Character character = gameObject.AddComponent<Character>();

        PlayFabClientAPI.GetUserData(request,
            result =>
            {
                foreach (var item in result.Data)
                {
                    if (item.Key == pageNum + "_" + deckIndex + "_character_id")
                    {
                        character_id = item.Value.Value;
                        character.character_ID = int.Parse(character_id);
                        Debug.LogFormat("받은 데이터: {0} / {1}", item.Key, character_id);
                    }
                    else if (item.Key == pageNum + "_" + deckIndex + "_character_type")
                    {
                        character_type = item.Value.Value;
                        character.character_Type = (Character.Type)Enum.Parse(typeof(Character.Type), character_type);
                        Debug.LogFormat("받은 데이터: {0} / {1}", item.Key, character_type);
                    }
                    else if (item.Key == pageNum + "_" + deckIndex + "_character_skill")
                    {
                        character_skill = item.Value.Value;
                        character.character_Skill = (Character.Skill)Enum.Parse(typeof(Character.Skill), character_skill);
                        Debug.LogFormat("받은 데이터: {0} / {1}", item.Key, character_skill);
                    }
                    else if (item.Key == pageNum + "_" + deckIndex + "_character_hp")
                    {
                        character_hp = item.Value.Value;
                        character.character_HP = int.Parse(character_hp);
                        Debug.LogFormat("받은 데이터: {0} / {1}", item.Key, character_hp);
                    }
                    else if (item.Key == pageNum + "_" + deckIndex + "_character_attack_damage")
                    {
                        character_attack_damage = item.Value.Value;
                        character.character_Attack_Damage = int.Parse(character_attack_damage);
                        Debug.LogFormat("받은 데이터: {0} / {1}", item.Key, character_attack_damage);
                    }
                    else if (item.Key == pageNum + "_" + deckIndex + "_character_attack_range")
                    {
                        character_attack_range = item.Value.Value;
                        for (int i = 0; i < 9; i++)
                        {
                            character.character_Attack_Range[i] = (character_attack_range[i] != '0');
                        }
                        Debug.LogFormat("받은 데이터: {0} / {1}", item.Key, character_attack_range);
                    }
                }
            }, error => Debug.LogWarningFormat("데이터 불러오기 실패: {0}", error.ErrorMessage)
        );

        character.Debuging_Character();

        return character;
    }
}
