using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_ID : MonoBehaviour
{
    public int Character_ID_Inventory;
    public SpriteRenderer Character_Image;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Get_Button()
    {
        Character_ID_Inventory = GameObject.Find("Team1_Character1").GetComponent<Character_Script>().character_ID_Number;
        Debug.Log(Character_ID_Inventory);
        Character_Image = GameObject.Find("Team1_Character1").GetComponent<SpriteRenderer>();
    }
}
