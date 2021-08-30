using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Click_Character : MonoBehaviour
{
    public GameObject[] Character;

    public void click_Character(GameObject present_Character)
    {
        for (int i = 0; i < Character.Length; i++)
        {
            if(Character[i].GetComponent<Image>().color == Color.yellow)
            {
                Character[i].GetComponent<Image>().color = Color.white;
            }
        }

        present_Character.GetComponent<Image>().color = Color.yellow;
        
    }
}
