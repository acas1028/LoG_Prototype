using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Popup_Position_script : MonoBehaviour
{
    public GameObject popUp;

    public GameObject positionY;

    public void PopupTranslate()
    {
        popUp.transform.position = this.gameObject.transform.position;
        popUp.GetComponent<RectTransform>().position = new Vector3(popUp.GetComponent<RectTransform>().transform.position.x, positionY.GetComponent<RectTransform>().transform.position.y, popUp.GetComponent<RectTransform>().transform.position.z);
    }
}
