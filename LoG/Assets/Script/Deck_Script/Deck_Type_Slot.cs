using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CharacterStats;

public class Deck_Type_Slot : MonoBehaviour
{
    public CharacterType characterType;
    public Sprite[] Type_Image;


    void Update()
    {
        NullisTransparent();
    }

    public void Change_Type(int num)
    {
        GetComponent<Image>().sprite = Type_Image[num];
        characterType = (CharacterType)num;
    }

    void NullisTransparent()
    {
        if(gameObject.GetComponent<Image>().sprite==null)
        {
            gameObject.GetComponent<Image>().color = Color.clear;
        }

        else
        {
            gameObject.GetComponent<Image>().color = Color.white;
        }
    }

}
