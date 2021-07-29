using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popup_Position_script : MonoBehaviour
{
    public GameObject popUp;


    public void PopupTranslate()
    {
        popUp.transform.position = this.gameObject.transform.position;
        popUp.transform.Translate(0, 250, 0);
    }
}
