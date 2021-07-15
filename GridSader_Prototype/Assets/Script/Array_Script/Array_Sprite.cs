using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Array_Sprite : MonoBehaviour
{
    public Sprite[] Character_Sprite;
    public int Character_ID;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.CompareTag("Character"))
        {
            Character_Sprite_Setting();
        }
    }

    public void Character_Sprite_Setting()
    {
        Character_ID = this.gameObject.GetComponent<Character_Script>().character_ID;
        this.gameObject.GetComponent<SpriteRenderer>().sprite = Character_Sprite[Character_ID - 1];
    }
}