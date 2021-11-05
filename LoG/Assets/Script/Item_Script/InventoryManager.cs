using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using PlayFab;
using PlayFab.ClientModels;

public class InventoryManager : MonoBehaviour
{
    private string playfabId;

    public Text gold;

    // Start is called before the first frame update
    void Start()
    {
        playfabId = PlayerPrefs.GetString("PlayFabId");
        if (playfabId == string.Empty)
            Debug.LogError("�α����� ���� �Ͻʽÿ�.");
    }

    public void GetInventory()
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), (result) =>
        {
            print("���� ���: " + result.VirtualCurrency["GD"]);
            gold.text = result.VirtualCurrency["GD"].ToString();
        }, (error) => print("���� ��� �ҷ����� ����"));
    }

    public void AddGold()
    {
        var request = new AddUserVirtualCurrencyRequest() { VirtualCurrency = "GD", Amount = 100 };
        PlayFabClientAPI.AddUserVirtualCurrency(request, (result) => { print("100 ��� ȹ��"); gold.text = result.Balance.ToString(); }, (error) => print("��� ȹ�� ����"));
    }

    public void SubtractGold()
    {
        var request = new SubtractUserVirtualCurrencyRequest() { VirtualCurrency = "GD", Amount = 100 };
        PlayFabClientAPI.SubtractUserVirtualCurrency(request, (result) => { print("100 ��� ����"); gold.text = result.Balance.ToString(); }, (error) => print("��� ���� ����"));
    }
}
