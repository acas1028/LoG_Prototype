using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PVE_InventoryImage : MonoBehaviour
{
    private Image characterImage;
    public GameObject spritePrefab;

    private void Start()
    { 
        characterImage = gameObject.GetComponent<Image>();

        int characterID = spritePrefab.GetComponent<Character>().character_ID;

        characterImage.sprite = spritePrefab.GetComponentInChildren<CharacterSpriteManager>().characterSprites[(characterID - 1) % 5 + 1];
    }
}
