using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using PlayFab;
using PlayFab.ClientModels;

public class InventoryManager : MonoBehaviour
{
    private string playfabId;
    private string lastAppleId;
    private string lastPeachId;

    public Text gold;
    public Text apple;
    public Text peach;

    // Start is called before the first frame update
    void Start()
    {
        playfabId = PlayerPrefs.GetString("PlayFabId");
        if (playfabId == string.Empty)
            Debug.LogError("�α����� ���� �Ͻʽÿ�.");

        gold.text = "0";
        apple.text = "0";
        peach.text = "0";

        GetInventory();
    }

    public void GetInventory()
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), (result) =>
        {
            gold.text = result.VirtualCurrency["GD"].ToString();
            for (int i = 0; i < result.Inventory.Count; i++)
            {
                var inven = result.Inventory[i];
                switch (inven.DisplayName)
                {
                    case "Apple":
                        apple.text = inven.RemainingUses.ToString();
                        lastAppleId = inven.ItemInstanceId;
                        break;
                    case "Peach":
                        peach.text = inven.RemainingUses.ToString();
                        lastPeachId = inven.ItemInstanceId;
                        break;
                    default:
                        break;
                }
            }
        }, (error) => print("�κ��丮 �ҷ����� ����"));
    }

    public void AddGold()
    {
        var request = new AddUserVirtualCurrencyRequest() { VirtualCurrency = "GD", Amount = 100 };
        PlayFabClientAPI.AddUserVirtualCurrency(request, (result) => { print("100 ��� ȹ��"); GetInventory(); }, (error) => { print("��� ȹ�� ����"); GetInventory(); });
    }

    public void SubtractGold()
    {
        var request = new SubtractUserVirtualCurrencyRequest() { VirtualCurrency = "GD", Amount = 100 };
        PlayFabClientAPI.SubtractUserVirtualCurrency(request, (result) => { print("100 ��� ����"); GetInventory(); }, (error) => { print("��� ���� ����"); GetInventory(); });
    }

    public void BuyApple()
    {
        var request = new PurchaseItemRequest() { CatalogVersion = "Items", ItemId = "Apple", VirtualCurrency = "GD", Price = 500 };
        PlayFabClientAPI.PurchaseItem(request, (result) => { print("��� ����"); GetInventory(); }, (error) => { print("��� ���� ����"); GetInventory(); });
    }

    public void BuyPeach()
    {
        var request = new PurchaseItemRequest() { CatalogVersion = "Items", ItemId = "Peach", VirtualCurrency = "GD", Price = 300 };
        PlayFabClientAPI.PurchaseItem(request, (result) => { print("������ ����"); GetInventory(); }, (error) => { print("������ ���� ����"); GetInventory(); });
    }

    public void EatApple()
    {
        var request = new ConsumeItemRequest() { ConsumeCount = 1, ItemInstanceId = lastAppleId };
        PlayFabClientAPI.ConsumeItem(request, (result) => { print("����� �Ծ���."); GetInventory(); }, (error) => { print("����� ���� ���ߴ�."); GetInventory(); });
    }

    public void EatPeach()
    {
        var request = new ConsumeItemRequest() { ConsumeCount = 1, ItemInstanceId = lastPeachId };
        PlayFabClientAPI.ConsumeItem(request, (result) => { print("�����Ƹ� �Ծ���."); GetInventory(); }, (error) => { print("�����Ƹ� ���� ���ߴ�."); GetInventory(); });
    }
}
