using System;
using UnityEngine;
using UnityEngine.UI;

using CharacterStats;
using PlayFab;
using PlayFab.ClientModels;

public class PurchasePopupManager : MonoBehaviour
{
    UserDataSynchronizer data;

    [SerializeField] Button[] closeButtons;
    [SerializeField] Image goodsImage;
    [SerializeField] Text goodsName;
    [SerializeField] Text goodsPrice;
    [SerializeField] Button purchaseButton;

    CharacterSkill skill;
    int price;

    private void Start() {
        data = UserDataSynchronizer.Instance;
        foreach (var button in closeButtons) {
            button.onClick.AddListener(ClosePopup);
        }
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
        if (UserDataSynchronizer.Instance.unlockedSkillList.Contains(skill)) {
            Debug.Log("���� ���� Ư���Դϴ�.");
            return;
        }

        var request = new PurchaseItemRequest() { CatalogVersion = "Skill", ItemId = "SKILL_" + ((int)skill).ToString(), VirtualCurrency = "CO", Price = price };
        PlayFabClientAPI.PurchaseItem(request,
            (result) => {
                data.GetUserDataFromServer();
                Debug.Log($"{result.Items} ���� ����");
            },
            (error) => Debug.Log($"{error.ErrorMessage}, ���� ����"));
    }

    void ClosePopup() {
        gameObject.SetActive(false);
    }
}
