using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character_sprite_layer : MonoBehaviour
{
    private void Update()
    {
        Set_Layer();
    }

    void Set_Layer()
    {
        if (this.GetComponent<SpriteRenderer>().sortingOrder != 0)
            return;

        switch(this.GetComponent<Character_Action>().character_Num_Of_Grid)
        {
            
            case 1:
                this.GetComponent<SpriteRenderer>().sortingOrder = 1;
                break;
            case 2:
                this.GetComponent<SpriteRenderer>().sortingOrder = 1;
                break;
            case 3:
                this.GetComponent<SpriteRenderer>().sortingOrder = 1;
                break;
            case 4:
                this.GetComponent<SpriteRenderer>().sortingOrder = 2;
                break;
            case 5:
                this.GetComponent<SpriteRenderer>().sortingOrder = 2;
                break;
            case 6:
                this.GetComponent<SpriteRenderer>().sortingOrder = 2;
                break;
            case 7:
                this.GetComponent<SpriteRenderer>().sortingOrder = 3;
                break;
            case 8:
                this.GetComponent<SpriteRenderer>().sortingOrder = 3;
                break;
            case 9:
                this.GetComponent<SpriteRenderer>().sortingOrder = 3;
                break;
        }
    }
}
