using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using CharacterStats;

public class GoodsDataController : MonoBehaviour {
    [SerializeField] PurchasePopupManager popup;
    [SerializeField] Button purchaseButton;

    public CharacterSkill skill;
    public GameObject descriptionPopup;

    public List<string> nameList;

    public Image itemImage;
    public Text itemName;
    public Text itemDescription;
    public Text itemPrice;

    Button thisButton;

    // Start is called before the first frame update
    void Start() {
        thisButton = GetComponent<Button>();
        thisButton.onClick.AddListener(ToggleDescription);
        purchaseButton.onClick.AddListener(PurchasePopup);
        GetSkillItem();
    }

    private void OnEnable() {
        if (UserDataSynchronizer.unlockedSkillList.Contains(skill)) {
            purchaseButton.interactable = false;
            itemPrice.text = "보유중";
        }
    }

    void GetSkillItem() {
        PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest { CatalogVersion = "Skill" },
            result => {
                foreach (var item in result.Catalog) {
                    if (item.ItemId == ("SKILL_" + (int)skill)) {
                        itemImage.sprite = Resources.Load(item.ItemImageUrl, typeof(Sprite)) as Sprite;
                        itemName.text = item.DisplayName;
                        itemDescription.text = item.Description;
                        itemPrice.text = item.VirtualCurrencyPrices["CO"].ToString();

                        if (UserDataSynchronizer.unlockedSkillList.Contains(skill)) {
                            purchaseButton.interactable = false;
                            itemPrice.text = "보유중";
                        }
                        
                        return;
                    }
                }
                print("Skill_" + skill.ToString() + " 발견 실패");
            }, error => print("Skill_" + skill.ToString() + " 아이템 불러오기 실패: " + error.ErrorMessage)
        );
    }

    public void ToggleDescription() {
        GameObject description = GameObject.FindWithTag("Description_Popup");
        if (description != null && description != descriptionPopup)
            description.SetActive(false);

        descriptionPopup.SetActive(!descriptionPopup.activeSelf);
    }

    void PurchasePopup() {
        popup.SetSkill(skill);
        popup.SetImage(itemImage.sprite);
        popup.SetName(itemName.text);
        popup.SetPrice(int.Parse(itemPrice.text));

        popup.gameObject.SetActive(true);
    }
}
