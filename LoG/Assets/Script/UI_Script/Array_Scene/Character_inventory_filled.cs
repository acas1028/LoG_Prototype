using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character_inventory_filled : MonoBehaviour
{
    private Image characterImage;
    public GameObject spritePrefab;
    public GameObject character_inventory; // 캐릭터칸

    IEnumerator Start()
    {
        //yield return new WaitUntil(DeckDataSync.Instance.IsGetAllData);

        //characterImage = gameObject.GetComponent<Image>();
        //int pageNum = Deck_Data_Send.instance.lastPageNum;
        //int index = transform.parent.GetComponent<Inventory_ID>().m_Inventory_ID;
        //Debug.Log("pageNum: " + pageNum + ", index: " + index);
        //int characterId = Deck_Data_Send.instance.Save_Data[pageNum, index - 1].GetComponent<Character>().character_ID;
        //Debug.Log("캐릭터 ID: " + characterId);

        //characterImage.sprite = spritePrefab.GetComponent<CharacterSpriteManager>().characterSprites[(characterId - 1) % 5 + 1];
    }

    private void Update()
    {
        // inventory_image_filled();
    }

    void inventory_image_filled() // 캐릭터 인벤토리 내용물 채우기
    {
        //this.gameObject.GetComponent<Image>().sprite = Character_sprite[character_Id - 1];
        //this.gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Character_sprite[character_Id - 1].rect.height);
        //this.gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Character_sprite[character_Id - 1].rect.width);
    }
}
