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
    void Start()
    {
        playfabId = PlayerPrefs.GetString("PlayFabId");
        if (playfabId == string.Empty)
            Debug.LogError("�α����� ���� �Ͻʽÿ�.");
    }

    public void SetData()
    {
        var data = new List<Dictionary<string, string>>();

        data.Add(new Dictionary<string, string>() { { "character_type", character_id } });
        data.Add(new Dictionary<string, string>() { { "character_type", character_type } });
        data.Add(new Dictionary<string, string>() { { "character_skill", character_skill } });
        data.Add(new Dictionary<string, string>() { { "character_hp", character_hp } });
        data.Add(new Dictionary<string, string>() { { "character_attack_damage", character_attack_damage } });
        data.Add(new Dictionary<string, string>() { { "character_attack_range", character_attack_range } });

        foreach (var item in data)
        {
            SendData(item);
        }
    }

    public void SendData(Dictionary<string, string> data)
    {
        // Key �� ����� ���: value ���� null �� ���ش�.
        var request = new UpdateUserDataRequest() { Data = data, Permission = UserDataPermission.Private };
        PlayFabClientAPI.UpdateUserData(request,
            result => {
                foreach (var item in request.Data)
                {
                    Debug.LogFormat("�÷��̾� ������ ���� ����: {0} / {1}", item.Key, item.Value);
                }
            }, error => Debug.LogWarningFormat("�÷��̾� ������ ���� ����: {0}", error.ErrorMessage)
        );
    }

    public void GetData()
    {
        var request = new GetUserDataRequest() { PlayFabId = playfabId };
        PlayFabClientAPI.GetUserData(request,
            result => {
                foreach (var item in result.Data)
                {
                    Debug.LogFormat("�ҷ��� ������: {0} / {1}", item.Key, item.Value.Value);
                    switch (item.Key)
                    {
                        case "character_id":
                            character_id = item.Value.Value;
                            break;
                        case "character_type":
                            character_type = item.Value.Value;
                            break;
                        case "character_skill":
                            character_skill = item.Value.Value;
                            break;
                        case "character_hp":
                            character_hp = item.Value.Value;
                            break;
                        case "character_attack_damage":
                            character_attack_damage = item.Value.Value;
                            break;
                        case "character_attack_range":
                            character_attack_range = item.Value.Value;
                            break;
                        default:
                            break;
                    }
                }
            }, error => Debug.LogWarningFormat("������ �ҷ����� ����: {0}", error.ErrorMessage)
        );
    }
}
