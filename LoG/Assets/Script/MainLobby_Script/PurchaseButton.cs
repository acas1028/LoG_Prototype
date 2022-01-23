using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseButton : MonoBehaviour
{
    public GameObject ShopGoods;

    public Sprite imageData;
    public string priceData;
    public string nameData;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivatePurchasePopup()
    {

        GameObject dum = GameObject.FindWithTag("Description_Popup");

        if (dum != null)
            dum.SetActive(false);


        //imageData = ShopGoods.GetComponent<GoodsDataController>().GoodsImage.sprite;
        //priceData = ShopGoods.GetComponent<GoodsDataController>().GoodsPrice.text;
        //nameData = ShopGoods.GetComponent<GoodsDataController>().GoodsName;

        GameObject popupController;

        popupController = GameObject.FindGameObjectWithTag("Purchase_Popup");

        popupController.GetComponent<PurchasePopupController>().ActivatePopup();
        popupController.GetComponent<PurchasePopupController>().SettingData(imageData, nameData, priceData);
    }
}
