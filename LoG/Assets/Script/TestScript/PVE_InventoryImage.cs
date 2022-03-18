using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CharacterStats;

public class PVE_InventoryImage : MonoBehaviour
{
    private Image characterImage;
    public GameObject spritePrefab;
    public GameObject stagedata;

    public bool isPve;

    

    private void Start()
    { 
        characterImage = gameObject.GetComponent<Image>();

        int characterID = spritePrefab.GetComponent<Character>().character_ID;

        if(isPve==true)
        {
            StageDataController stageDataController = stagedata.GetComponent<StageDataController>();
            int Num =gameObject.transform.parent.GetComponent<Inventory_ID>().GetInventoryNum();
            Character character = stageDataController.PlayerCharacters[Num - 1].GetComponent<Character>();
            CharacterType type = character.character_Type;

            switch(type)
            {
                case CharacterType.Attacker:
                    characterImage.sprite = character.spriteManager.characterSprites[1];
                    break;
                case CharacterType.Balance:
                    characterImage.sprite = character.spriteManager.characterSprites[2];
                    break;
                case CharacterType.Defender:
                    characterImage.sprite = character.spriteManager.characterSprites[3];
                    break;
            }

            


        }

        else
        {
            characterImage.sprite = spritePrefab.GetComponentInChildren<CharacterSpriteManager>().characterSprites[(characterID - 1) % 5 + 1];
        }
    }
}
