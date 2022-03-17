using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayCharacterAnimation : MonoBehaviour
{
    public GameObject[] myGrid;
    public GameObject[] oppenentGrid;


    private void Update()
    {
        ArrayIdleAnimation();
    }

    void ArrayIdleAnimation()
    {
        for(int i =0; i<myGrid.Length;i++)
        {
            if(myGrid[i].tag== "Character")
            {
                if(myGrid[i].transform.Find("Character_Prefab").GetComponent<Animator>().enabled==false)
                {
                    myGrid[i].transform.Find("Character_Prefab").GetComponent<Animator>().enabled = true;
                }


                if(myGrid[i].transform.Find("Character_Prefab").GetComponent<SpriteRenderer>().sprite== myGrid[i].transform.Find("Character_Prefab").GetChild(0).GetComponent<CharacterSpriteManager>().characterSprites[1])
                {
                    myGrid[i].transform.Find("Character_Prefab").GetComponent<Animator>().SetInteger("IdleNum", 2);
                }

                if(myGrid[i].transform.Find("Character_Prefab").GetComponent<SpriteRenderer>().sprite == myGrid[i].transform.Find("Character_Prefab").GetChild(0).GetComponent<CharacterSpriteManager>().characterSprites[2])
                { 
                    myGrid[i].transform.Find("Character_Prefab").GetComponent<Animator>().SetInteger("IdleNum", 3);
                }

                if (myGrid[i].transform.Find("Character_Prefab").GetComponent<SpriteRenderer>().sprite == myGrid[i].transform.Find("Character_Prefab").GetChild(0).GetComponent<CharacterSpriteManager>().characterSprites[3])
                {
                    myGrid[i].transform.Find("Character_Prefab").GetComponent<Animator>().SetInteger("IdleNum", 1);
                }

                if (myGrid[i].transform.Find("Character_Prefab").GetComponent<SpriteRenderer>().sprite == myGrid[i].transform.Find("Character_Prefab").GetChild(0).GetComponent<CharacterSpriteManager>().characterSprites[4])
                {
                    myGrid[i].transform.Find("Character_Prefab").GetComponent<Animator>().SetInteger("IdleNum", 0);
                }
            }

            else
            {
                if (myGrid[i].transform.Find("Character_Prefab").GetComponent<Animator>().enabled == true)
                {
                    myGrid[i].transform.Find("Character_Prefab").GetComponent<Animator>().enabled = false;
                }
            }

        }

        for (int i = 0; i < oppenentGrid.Length; i++)
        {
            if(oppenentGrid[i].tag== "Character")
            {
                if(oppenentGrid[i].transform.Find("Character_Prefab").GetComponent<Animator>().enabled==false)
                {
                    oppenentGrid[i].transform.Find("Character_Prefab").GetComponent<Animator>().enabled = true;
                }

                if (oppenentGrid[i].transform.Find("Character_Prefab").GetComponent<SpriteRenderer>().sprite == oppenentGrid[i].transform.Find("Character_Prefab").GetChild(0).GetComponent<CharacterSpriteManager>().characterSprites[1])
                {
                    oppenentGrid[i].transform.Find("Character_Prefab").GetComponent<Animator>().SetInteger("IdleNum", 2);
                }

                if (oppenentGrid[i].transform.Find("Character_Prefab").GetComponent<SpriteRenderer>().sprite == oppenentGrid[i].transform.Find("Character_Prefab").GetChild(0).GetComponent<CharacterSpriteManager>().characterSprites[2])
                {
                    oppenentGrid[i].transform.Find("Character_Prefab").GetComponent<Animator>().SetInteger("IdleNum", 3);
                }

                if (oppenentGrid[i].transform.Find("Character_Prefab").GetComponent<SpriteRenderer>().sprite == oppenentGrid[i].transform.Find("Character_Prefab").GetChild(0).GetComponent<CharacterSpriteManager>().characterSprites[3])
                {
                    oppenentGrid[i].transform.Find("Character_Prefab").GetComponent<Animator>().SetInteger("IdleNum", 1);
                }

                if (oppenentGrid[i].transform.Find("Character_Prefab").GetComponent<SpriteRenderer>().sprite == oppenentGrid[i].transform.Find("Character_Prefab").GetChild(0).GetComponent<CharacterSpriteManager>().characterSprites[4])
                {
                    oppenentGrid[i].transform.Find("Character_Prefab").GetComponent<Animator>().SetInteger("IdleNum", 0);
                }
            }

            else
            {
                if (oppenentGrid[i].transform.Find("Character_Prefab").GetComponent<Animator>().enabled == true)
                {
                    oppenentGrid[i].transform.Find("Character_Prefab").GetComponent<Animator>().enabled = false;
                }
            }
        }
    }
}
