using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Deck_Type_Slot : MonoBehaviour
{
    public int Character_Type = 0;
    public Sprite[] Type_Image;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Change_Type(int num)
    {
        Deck_Type_Slot cs = this.gameObject.GetComponent<Deck_Type_Slot>();

        cs.Character_Type = num;

        switch(cs.Character_Type)
        {
            case 0:
                this.gameObject.GetComponent<Image>().sprite = Type_Image[0];
                break;
            case 1:
                this.gameObject.GetComponent<Image>().sprite= Type_Image[1];
                break;
            case 2:
                this.gameObject.GetComponent<Image>().sprite = Type_Image[2];
                break;
            case 3:
                this.gameObject.GetComponent<Image>().sprite = Type_Image[3];
                break;
        }
    }

}
