using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterStats;

public class CharacterSpriteManager : MonoBehaviour
{
    public Sprite[] characterSprites;
    public Sprite[] pveCharacterSprites;
    public Sprite characterSprite;

    public enum PveCharacter_color { Red, Green, Blue };

    public PveCharacter_color character_Color;

    // Start is called before the first frame update
    void Start()
    {

    }


    public void SetInitialSprite(int id)
    {
        // 현재 ID 개수(14개)만큼의 스프라이트가 없어서 임시로 적용함
        // gameObject.GetComponent<SpriteRenderer>().sprite = Character_Sprite[Character_ID];

        switch(transform.parent.GetComponent<Character>().character_Type)
        {
            case CharacterType.Attacker:
                transform.GetComponentInParent<SpriteRenderer>().sprite = characterSprites[1];
                break;
            case CharacterType.Balance:
                transform.GetComponentInParent<SpriteRenderer>().sprite = characterSprites[2];
                break;
            case CharacterType.Defender:
                transform.GetComponentInParent<SpriteRenderer>().sprite = characterSprites[3];
                break;
            case CharacterType.Null:
                transform.GetComponentInParent<SpriteRenderer>().sprite = characterSprites[0];
                break;
        }

        //transform.GetComponentInParent<SpriteRenderer>().sprite = characterSprites[(id - 1) % 5 + 1];
    }

    public void SetPveCharacterSprite()
    {
        GameObject[] OppenetEnemy_Grid= PVE_Arrayment.instance.EnemyGrids;

        for (int i=0; i<OppenetEnemy_Grid.Length;i++)
        {
            if(OppenetEnemy_Grid[i].tag == "Character")
            {
                Debug.Log("asdgf");
                switch(character_Color)
                {
                    case PveCharacter_color.Red:
                        OppenetEnemy_Grid[i].transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = pveCharacterSprites[0];
                        break;

                    case PveCharacter_color.Green:
                        OppenetEnemy_Grid[i].transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = pveCharacterSprites[1];
                        break;

                    case PveCharacter_color.Blue:                       
                        OppenetEnemy_Grid[i].transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = pveCharacterSprites[2];                       
                        break;

                }
            }
        }
        
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
