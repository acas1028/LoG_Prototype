using System;
using UnityEngine;
using UnityEngine.UI;

using CharacterStats;
using PlayFab;
using PlayFab.ClientModels;

public class PurchasePopupManager : MonoBehaviour
{
    [SerializeField] UserDataSynchronizer data;

    [SerializeField] Button[] closeButtons;
    [SerializeField] Image goodsImage;
    [SerializeField] Text goodsName;
    [SerializeField] Text goodsPrice;
    [SerializeField] Button purchaseButton;

    CharacterSkill skill;
    int price;

    private void Start() {
        foreach (var button in closeButtons) {
            button.onClick.AddListener(ClosePopup);
        }
    }

    private void OnEnable() {
        purchaseButton.onClick.AddListener(PurchaseSkill);
    }

    public void SetSkill(CharacterSkill skill) {
        this.skill = skill;
    }

    public void SetImage(Sprite sprite) {
        goodsImage.sprite = sprite;
    }
    
    public void SetName(string name) {
        goodsName.text = name;
    }
    
    public void SetPrice(int price) {
        this.price = price;
        goodsPrice.text = price.ToString();
    }

    public void PurchaseSkill() {
        var request = new PurchaseItemRequest() { CatalogVersion = "Skill", ItemId = "SKILL_" + ((int)skill).ToString(), VirtualCurrency = "CO", Price = price };
        PlayFabClientAPI.PurchaseItem(request,
            (result) => {
                data.GetUserDataFromServer();
                Debug.Log($"{result.Items} 구매 성공");
            },
            (error) => Debug.Log($"{error.ErrorMessage}, 구매 실패"));
    }

    void ClosePopup() {
        gameObject.SetActive(false);
    }
}
