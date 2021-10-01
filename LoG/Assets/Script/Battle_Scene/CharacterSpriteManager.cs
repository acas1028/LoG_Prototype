using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpriteManager : MonoBehaviour
{
    public Sprite[] characterSprites;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetInitialSprite(int id)
    {
        // 현재 ID 개수(14개)만큼의 스프라이트가 없어서 임시로 적용함
        // gameObject.GetComponent<SpriteRenderer>().sprite = Character_Sprite[Character_ID];
        transform.GetComponentInParent<SpriteRenderer>().sprite = characterSprites[(id - 1) % 5 + 1];
    }

    public void SetSortingLayer(int character_Num_Of_Grid)
    {

        if (character_Num_Of_Grid == 1 || character_Num_Of_Grid == 2 || character_Num_Of_Grid == 3)
        {
            transform.GetComponentInParent<SpriteRenderer>().sortingLayerName = "Characters_Line1";
        }

        if (character_Num_Of_Grid == 4 || character_Num_Of_Grid == 5 || character_Num_Of_Grid == 6)
        {
            transform.GetComponentInParent<SpriteRenderer>().sortingLayerName = "Characters_Line2";
        }

        if (character_Num_Of_Grid == 7 || character_Num_Of_Grid == 8 || character_Num_Of_Grid == 9)
        {
            transform.GetComponentInParent<SpriteRenderer>().sortingLayerName = "Characters_Line3";
        }
    }
    public void SetDeadSprite()
    {
        transform.GetComponentInParent<SpriteRenderer>().sprite = characterSprites[0];
    }
}
