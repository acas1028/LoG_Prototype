using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Deck_Character_TypeSlot : MonoBehaviour,IDropHandler
{
    public int Character_Type = 0;
    public Sprite[] Type_Image;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Change_Type(int num)
    {
        Deck_Character_TypeSlot cs = this.gameObject.GetComponent<Deck_Character_TypeSlot>();

        cs.Character_Type = num;

        switch(cs.Character_Type)
        {
            case 1:
                this.gameObject.GetComponent<Image>().sprite= Type_Image[0];
                break;
            case 2:
                this.gameObject.GetComponent<Image>().sprite = Type_Image[1];
                break;
            case 3:
                this.gameObject.GetComponent<Image>().sprite = Type_Image[2];
                break;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("µå·ÓµÊ");
    }

}
