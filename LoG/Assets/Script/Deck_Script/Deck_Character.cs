using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck_Character : MonoBehaviour
{
    public bool Set_Active_Character = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        Deck_Manager cs = Deck_Manager.instance;
        for(int i=0;i<cs.Set_Character_.Length;i++)
        {
            cs.Set_Character_[i].SetActive(false);
        }
        Set_Active_Character = true;

        cs.Current_Character = this.gameObject;
    }
}
