using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck_Character : MonoBehaviour
{
    public Sprite[] Character_Sprite;
    public int Deck_Character_ID;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Get_Deck_Character(int num)
    {
        this.gameObject.GetComponent<Character_Script>().Character_Setting(num);
        this.gameObject.GetComponent<Character_Script>().Debuging_Character();
    }
    
}
