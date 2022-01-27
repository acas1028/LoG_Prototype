using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using CharacterStats;

public class GoodsDataController : MonoBehaviour {
    public CharacterSkill Goods_Data;
    public GameObject descriptionPopup;

    public List<string> nameList;

    public Image itemImage;
    public Text itemName;
    public Text itemDescription;
    public Text itemCost;

    // Start is called before the first frame update
    void Start() {
        var thisButton = GetComponent<Button>();
        thisButton.onClick.AddListener(ToggleDescription);
        GetSkillItem();
    }

    void GetSkillItem() {
        PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest { CatalogVersion = "Skill Items" },
            result => {
                foreach (var item in result.Catalog) {
                    if (item.ItemId == ("Skill_" + Goods_Data.ToString())) {
                        itemImage.sprite = Resources.Load(item.ItemImageUrl, typeof(Sprite)) as Sprite;
                        itemName.text = item.DisplayName;
                        itemDescription.text = item.Description;
                        itemCost.text = item.VirtualCurrencyPrices["CO"].ToString();
                        return;
                    }
                }
                print("Skill_" + Goods_Data.ToString() + " 발견 실패");
            }, error => print("Skill_" + Goods_Data.ToString() + " 아이템 불러오기 실패: " + error.ErrorMessage));
    }

    void ToggleDescription() {
        GameObject description = GameObject.FindWithTag("Description_Popup");
        if (description != null)
            description.SetActive(false);

        descriptionPopup.SetActive(!descriptionPopup.activeSelf);
    }
}
