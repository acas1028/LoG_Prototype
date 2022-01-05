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
            Debug.LogError("로그인을 먼저 하십시오.");

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
        }, (error) => print("인벤토리 불러오기 실패"));
    }

    public void AddGold()
    {
        var request = new AddUserVirtualCurrencyRequest() { VirtualCurrency = "GD", Amount = 100 };
        PlayFabClientAPI.AddUserVirtualCurrency(request, (result) => { print("100 골드 획득"); GetInventory(); }, (error) => { print("골드 획득 실패"); GetInventory(); });
    }

    public void SubtractGold()
    {
        var request = new SubtractUserVirtualCurrencyRequest() { VirtualCurrency = "GD", Amount = 100 };
        PlayFabClientAPI.SubtractUserVirtualCurrency(request, (result) => { print("100 골드 차감"); GetInventory(); }, (error) => { print("골드 차감 실패"); GetInventory(); });
    }

    public void BuyApple()
    {
        var request = new PurchaseItemRequest() { CatalogVersion = "Items", ItemId = "Apple", VirtualCurrency = "GD", Price = 500 };
        PlayFabClientAPI.PurchaseItem(request, (result) => { print("사과 구입"); GetInventory(); }, (error) => { print("사과 구입 실패"); GetInventory(); });
    }

    public void BuyPeach()
    {
        var request = new PurchaseItemRequest() { CatalogVersion = "Items", ItemId = "Peach", VirtualCurrency = "GD", Price = 300 };
        PlayFabClientAPI.PurchaseItem(request, (result) => { print("복숭아 구입"); GetInventory(); }, (error) => { print("복숭아 구입 실패"); GetInventory(); });
    }

    public void EatApple()
    {
        var request = new ConsumeItemRequest() { ConsumeCount = 1, ItemInstanceId = lastAppleId };
        PlayFabClientAPI.ConsumeItem(request, (result) => { print("사과를 먹었다."); GetInventory(); }, (error) => { print("사과를 먹지 못했다."); GetInventory(); });
    }

    public void EatPeach()
    {
        var request = new ConsumeItemRequest() { ConsumeCount = 1, ItemInstanceId = lastPeachId };
        PlayFabClientAPI.ConsumeItem(request, (result) => { print("복숭아를 먹었다."); GetInventory(); }, (error) => { print("복숭아를 먹지 못했다."); GetInventory(); });
    }
}
