using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrayment_Popup_Script : MonoBehaviour
{
    public GameObject Grid;
    public GameObject Popup;
    public GameObject ArrayCancle;

    public AudioClip arrayaudio;
    public AudioClip Popupauido;


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

        PlaySound.Instance.ChangeSoundAndPlay(arrayaudio); //°í¹Î Áß

        GameObject made_Popup = Instantiate(Popup);
         
        made_Popup.transform.SetParent(this.transform);
        made_Popup.transform.localPosition = new Vector3(0, 0, 0);
        made_Popup.SetActive(false);
        made_Popup.GetComponent<animation_On_off>().temporary_button = ArrayCancle;
        Limit_Popup.Limit_Poup_instance.popup_List.Add(made_Popup);

    }

    void DestoryPopup()
    {
        if (Grid.tag != "Null_Character")
            return;
        if (this.transform.childCount != 1)
            return;

        Limit_Popup.Limit_Poup_instance.popup_List.Remove(this.transform.GetChild(0).gameObject);
        Destroy(this.transform.GetChild(0).gameObject);
    }
}
