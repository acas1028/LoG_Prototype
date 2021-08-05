using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character_inventory_filled : MonoBehaviour
{
    public Sprite[] Character_sprite; //ĳ���� ���� �� �̹���
    public GameObject character_inventory; // ĳ����ĭ
    private int character_Id; // ĳ���� ������ ���̵�

    private void Start() 
    {
        Character_sprite = GameObject.FindGameObjectWithTag("Sprite_Data").gameObject.GetComponent<Sprite_Data>().Character_Sprite;
        character_Id = character_inventory.GetComponent<Inventory_ID>().m_Character_ID;
    }

    private void Update()
    {
        inventory_image_filled();
    }

    void inventory_image_filled() // ĳ���� �κ��丮 ���빰 ä���
    {
        this.gameObject.GetComponent<Image>().sprite = Character_sprite[character_Id - 1];
        //this.gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Character_sprite[character_Id - 1].rect.height);
        //this.gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Character_sprite[character_Id - 1].rect.width);
    }
}
