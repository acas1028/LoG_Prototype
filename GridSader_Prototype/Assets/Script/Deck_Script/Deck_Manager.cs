using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Deck_Manager : MonoBehaviour
{
    public List<Button> Deck_Characters = new List<Button>();
    public List<Button> D_Inventory = new List<Button>();
    
    private int Deck_Count = 1;

    void Start()
    {
        for (int i = 0; i < D_Inventory.Count; i++)
        {
            D_Inventory[i].GetComponent<Character_Script>().character_ID = 0;
        }
    }

    void Update()
    {
        
    }

    public void Click_Deck_Character()
    {
        switch(Deck_Count)
        {
            case 1:

                break;
        }

    }
}
