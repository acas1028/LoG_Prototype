using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Limit_Popup : MonoBehaviour
{
    public GameObject popup;
    public GameObject limit_Popup_button;
    public GameObject present_Popup;
    public GameObject arrayamanager;

    public List<GameObject> popup_List = new List<GameObject>();


    GameObject is_button;

    private static Limit_Popup Limit_Popup_Manager;

    public static Limit_Popup Limit_Poup_instance
    {
        get
        {
            if(!Limit_Popup_Manager)
            {
                Limit_Popup_Manager = FindObjectOfType(typeof(Limit_Popup)) as Limit_Popup;

                if (Limit_Popup_Manager == null)
                    Debug.Log("no Singleton obj");

            }

            return Limit_Popup_Manager;
        }
    }

    private void Awake()
    {
        if(Limit_Popup_Manager ==null)
        {
            Limit_Popup_Manager = this;
        }
        else if (!Limit_Popup_Manager!= this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        limit_Popup_button.SetActive(false);
    }

    private void Update()
    {
        Popup_limit_Only_One();
        BoolPopupOn();
    }

    public void Popup_OutsideClick()
    {
        if (popup.activeSelf == true)
        {
            if (popup.GetComponent<animation_On_off>() != null)
            {
                
                Debug.Log("outside");
                popup.GetComponent<animation_On_off>().AnimationOff();
                BoolPopupOn_Off();


            }

            

            limit_Popup_button.SetActive(false);
        }
    }

    void Popup_limit_Only_One()
    {
        if (!popup)
            return;

        if(present_Popup!= popup)
        {
            for(int i=0; i<popup_List.Count;i++)
            {
                if (popup_List[i].GetComponent<animation_On_off>() != null)
                {
                    if (popup_List[i].activeSelf == true && popup_List[i] != popup)
                    {
                        Debug.Log("change");
                        popup_List[i].GetComponent<animation_On_off>().AnimationOff();
                    }
                }
            }

            present_Popup = popup;
        }
    }

    public void PopupOff()
    {
        popup.GetComponent<animation_On_off>().AnimationOff();
    }

    public void BoolPopupOn()
    {
        if (arrayamanager == null)
            return;
        if (arrayamanager.GetComponent<Arrayment_Manager>() != null)
        {
            if (arrayamanager.GetComponent<Arrayment_Manager>().getisPopupOn() == false)
            {
                for (int i = 0; i < popup_List.Count; i++)
                {
                    if (popup_List[i].tag != "Popup")
                    {

                        if (popup_List[i].activeSelf == true)
                        {

                            Debug.Log(popup_List[i].name);
                            if (arrayamanager.GetComponent<Arrayment_Manager>() != null)
                            {
                                Debug.Log("PopupOn");
                                arrayamanager.GetComponent<Arrayment_Manager>().SetIsPopupOn(true);
                            }

                        }
                    }
                }
            }
        }

        if (arrayamanager.GetComponent<PVE_Arrayment>() != null)
        {
            if (arrayamanager.GetComponent<PVE_Arrayment>().getisPopupOn() == false)
            {
                for (int i = 0; i < popup_List.Count; i++)
                {
                    if (popup_List[i].tag != "Popup")
                    {

                        if (popup_List[i].activeSelf == true)
                        {

                            Debug.Log(popup_List[i].name);
                            if (arrayamanager.GetComponent<PVE_Arrayment>() != null)
                            {
                                Debug.Log("PopupOn");
                                arrayamanager.GetComponent<PVE_Arrayment>().SetIsPopupOn(true);
                            }

                        }
                    }
                }
            }
        }
    }

    public void BoolPopupOn_Off()
    {
        if (arrayamanager == null)
            return;

        if (arrayamanager.GetComponent<Arrayment_Manager>() != null)
        {
            Debug.Log("PopupFalse");
            arrayamanager.GetComponent<Arrayment_Manager>().SetIsPopupOn(false);
        }
        if(arrayamanager.GetComponent<PVE_Arrayment>() != null)
        {
            Debug.Log("PopupFalse");
            arrayamanager.GetComponent<PVE_Arrayment>().SetIsPopupOn(false);
        }
    }





    public GameObject GetPopup()
    {
        return popup;
    }

    public GameObject GetisButton()
    {
        return is_button;
    }

    public void SetPopup(GameObject The_PoPup)
    {
        popup = The_PoPup;
    }

    public void SetIsbutton(GameObject button)
    {
        is_button = button;
    }
}
