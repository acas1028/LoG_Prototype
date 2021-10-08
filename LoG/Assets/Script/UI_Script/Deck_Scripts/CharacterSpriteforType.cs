using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSpriteforType : MonoBehaviour
{
    public int Character_Type = 0;


    public  Sprite[] CharacterImage;

    public void ChangeSprite(int num)
    {
        CharacterSpriteforType cs = this.gameObject.GetComponent<CharacterSpriteforType>();
        cs.Character_Type = num;
        
        switch(cs.Character_Type)
        {
            case 0:
                this.GetComponent<Image>().sprite=CharacterImage[0];
                break;
            case 1:
                this.GetComponent<Image>().sprite = CharacterImage[1];
                break;
            case 2:
                this.GetComponent<Image>().sprite = CharacterImage[2];
                break;
            case 3:
                this.GetComponent<Image>().sprite = CharacterImage[3];
                break;

        }
    }
}
