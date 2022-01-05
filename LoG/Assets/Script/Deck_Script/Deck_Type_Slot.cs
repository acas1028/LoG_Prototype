using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CharacterStats;

public class Deck_Type_Slot : MonoBehaviour
{
    public CharacterType characterType;
    public Sprite[] Type_Image;

    public void Change_Type(int num)
    {
        GetComponent<Image>().sprite = Type_Image[num];
        characterType = (CharacterType)num;
    }

}
