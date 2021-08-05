using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character_inventory_filled : MonoBehaviour
{
    public Sprite[] Character_sprite; //캐릭터 내에 들어갈 이미지
    public GameObject character_inventory; // 캐릭터칸
    private int character_Id; // 캐릭터 각자의 아이디

    private void Start() 
    {
        Character_sprite = GameObject.FindGameObjectWithTag("Sprite_Data").gameObject.GetComponent<Sprite_Data>().Character_Sprite;
        character_Id = character_inventory.GetComponent<Inventory_ID>().m_Character_ID;
    }

    private void Update()
    {
        inventory_image_filled();
    }

    void inventory_image_filled() // 캐릭터 인벤토리 내용물 채우기
    {
        this.gameObject.GetComponent<Image>().sprite = Character_sprite[character_Id - 1];
        //this.gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Character_sprite[character_Id - 1].rect.height);
        //this.gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Character_sprite[character_Id - 1].rect.width);
    }
}
