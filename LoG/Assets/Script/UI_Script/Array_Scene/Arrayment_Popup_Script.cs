using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrayment_Popup_Script : MonoBehaviour
{
    public GameObject Grid;
    public GameObject Popup;


    private void Update()
    {
        MakePopup_In_Child();
        DestoryPopup();
    }

    void MakePopup_In_Child()
    {
        if (Grid.tag != "Character")
            return;
        if (this.transform.childCount != 0)
            return;

        GameObject made_Popup = Instantiate(Popup);
         
        made_Popup.transform.SetParent(this.transform);
        made_Popup.transform.localPosition = new Vector3(0, 0, 0);
        made_Popup.SetActive(false);

    }

    void DestoryPopup()
    {
        if (Grid.tag != "Null_Character")
            return;
        if (this.transform.childCount != 1)
            return;

        Destroy(this.transform.GetChild(0).gameObject);
    }
}
