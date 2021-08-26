using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetGridCharacters : MonoBehaviour
{
    public GameObject[] Grids;
    public GameObject[] Enemy_Grids;
    public GameObject[] Inventory;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject ac = Arrayed_Data.instance.team1[i];
            Character c = Grids[ac.GetComponent<Character>().character_Num_Of_Grid - 1].GetComponentInChildren<Character>();
            Grids[ac.GetComponent<Character>().character_Num_Of_Grid - 1].tag = "Character";
            c.Copy_Character_Stat(ac);
            c.InitializeCharacterSprite();

            ac = Arrayed_Data.instance.team2[i];
            c = Enemy_Grids[ac.GetComponent<Character>().character_Num_Of_Grid - 1].GetComponentInChildren<Character>();
            Enemy_Grids[ac.GetComponent<Character>().character_Num_Of_Grid - 1].tag = "Character";
            c.Copy_Character_Stat(ac);
            c.InitializeCharacterSprite();
        }

        for (int i = 0; i < 7; i++)
        {
            Inventory[i].GetComponent<Inventory_ID>().Block_Inventory.SetActive(true);
        }
    }
}
