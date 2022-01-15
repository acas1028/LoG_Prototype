using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DescriptionPopupController : MonoBehaviour
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

    public void ActivateDescriptionPopup()
    {
        GameObject dum = GameObject.FindWithTag("Description_Popup");

        if (dum != null && dum == Popup)
        {
            dum.SetActive(false);
            return;
        }

        if (dum != null && dum != Popup)
        {
            dum.SetActive(false);
        }

        Popup.SetActive(true);
        Popup.GetComponent<DescriptionPopup>().SettingData(GetComponent<GoodsDataController>().GoodsName, GetComponent<GoodsDataController>().GoodsDescription);
    }
}
