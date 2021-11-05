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
            Debug.LogError("·Î±×ÀÎÀ» ¸ÕÀú ÇÏ½Ê½Ã¿À.");
    }

    public void GetInventory()
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), (result) =>
        {
            print("ÇöÀç °ñµå: " + result.VirtualCurrency["GD"]);
            gold.text = result.VirtualCurrency["GD"].ToString();
        }, (error) => print("ÇöÀç °ñµå ºÒ·¯¿À±â ½ÇÆÐ"));
    }

    public void AddGold()
    {
        var request = new AddUserVirtualCurrencyRequest() { VirtualCurrency = "GD", Amount = 100 };
        PlayFabClientAPI.AddUserVirtualCurrency(request, (result) => { print("100 °ñµå È¹µæ"); gold.text = result.Balance.ToString(); }, (error) => print("°ñµå È¹µæ ½ÇÆÐ"));
    }

    public void SubtractGold()
    {
        var request = new SubtractUserVirtualCurrencyRequest() { VirtualCurrency = "GD", Amount = 100 };
        PlayFabClientAPI.SubtractUserVirtualCurrency(request, (result) => { print("100 °ñµå Â÷°¨"); gold.text = result.Balance.ToString(); }, (error) => print("°ñµå Â÷°¨ ½ÇÆÐ"));
    }
}
