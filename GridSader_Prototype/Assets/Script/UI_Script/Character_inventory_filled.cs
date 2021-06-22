using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character_inventory_filled : MonoBehaviour
{
    public Sprite[] Character_sprite;
    private GameObject character_inventory;
    private int character_Id;

    private void Start()
    {
        Character_sprite = GameObject.FindGameObjectWithTag("Sprite_Data").gameObject.GetComponent<Sprite_Data>().Character_Sprite;
        character_inventory = this.gameObject.transform.parent.gameObject;
        character_Id = character_inventory.GetComponent<Inventory_ID>().m_Character_ID;
    }

    private void Update()
    {
        inventory_image_filled();
    }

    void inventory_image_filled() // 캐릭터 인벤토리 내용물 채우기
    {
        this.gameObject.GetComponent<Image>().sprite = Character_sprite[character_Id - 1];
    }
}
