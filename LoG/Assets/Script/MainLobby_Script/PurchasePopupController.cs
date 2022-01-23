using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurchasePopupController : MonoBehaviour
{
    public GameObject Popup;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SettingData(Sprite dataSprite, string dataName, string dataPrice)
    {
        Popup.GetComponent<PurchasePopupDataChange>().dataSprite = dataSprite;
        Popup.GetComponent<PurchasePopupDataChange>().dataName = dataName;
        Popup.GetComponent<PurchasePopupDataChange>().dataPrice = dataPrice;
    }
    public void ActivatePopup()
    {
        Popup.SetActive(true);
    }
}
