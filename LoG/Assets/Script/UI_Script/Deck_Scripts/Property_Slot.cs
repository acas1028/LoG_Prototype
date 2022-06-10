using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CharacterStats;

public class Property_Slot : MonoBehaviour
{
    //public CharacterSkill character_Skill;
    //public CharacterType characterType;

    //public Sprite[] attacker_Sprites;
    //public Sprite[] balance_Sprites;
    //public Sprite[] Defense_Sprites;

    public Property_images_collection property_Images_Collection;

    public string property_Name;


    private void Update()
    {
        NullisTransparent();
    }

    public void Change_property(string name)
    {
        Property_Slot cs = this.gameObject.GetComponent<Property_Slot>();

        cs.property_Name = name;

        switch(cs.property_Name)
        {
            case "Null":
                this.GetComponent<Image>().sprite = null;
                break;

            //공격형 
            case "Attack_ArmorPiercer":
                this.GetComponent<Image>().sprite = property_Images_Collection.attack_Sprites[0];
                break;
            case "Attack_Confidence":
                this.GetComponent<Image>().sprite = property_Images_Collection.attack_Sprites[1];
                break;
            case "Attack_DivineShield":
                this.GetComponent<Image>().sprite = property_Images_Collection.attack_Sprites[2];
                break;
            case "Attack_Executioner":
                this.GetComponent<Image>().sprite = property_Images_Collection.attack_Sprites[3];
                break;
            case "Attack_Ranger":
                this.GetComponent<Image>().sprite = property_Images_Collection.attack_Sprites[4];
                break;
            case "Attack_Struggle":
                this.GetComponent<Image>().sprite = property_Images_Collection.attack_Sprites[5];
                break;
            case "Attack_Sturdy":
                this.GetComponent<Image>().sprite = property_Images_Collection.attack_Sprites[6];
                break;
            case "Attack_Null":
                this.GetComponent<Image>().sprite = property_Images_Collection.attack_Sprites[7];
                break;
            //밸런스형
            case "Balance_Blessing":
                this.GetComponent<Image>().sprite = property_Images_Collection.balance_Sprites[0];
                break;
            case "Balance_Curse":
                this.GetComponent<Image>().sprite = property_Images_Collection.balance_Sprites[1];
                break;
            case "Balance_DestinyBond":
                this.GetComponent<Image>().sprite = property_Images_Collection.balance_Sprites[2];
                break;
            case "Balance_GBGH":
                this.GetComponent<Image>().sprite = property_Images_Collection.balance_Sprites[3];
                break;
            case "Balance_Smoke":
                this.GetComponent<Image>().sprite = property_Images_Collection.balance_Sprites[4];
                break;
            case "Balance_Survivor":
                this.GetComponent<Image>().sprite = property_Images_Collection.balance_Sprites[5];
                break;
            case "Balance_WideCounter":
                this.GetComponent<Image>().sprite = property_Images_Collection.balance_Sprites[6];
                break;
            case "Balance_Null":
                this.GetComponent<Image>().sprite = property_Images_Collection.attack_Sprites[7];
                break;
            //방어형
            case "Defense_Barrier":
                this.GetComponent<Image>().sprite = property_Images_Collection.defender_Sprites[0];
                break;
            case "Defense_Coward":
                this.GetComponent<Image>().sprite = property_Images_Collection.defender_Sprites[1];
                break;
            case "Defense_Disarm":
                this.GetComponent<Image>().sprite = property_Images_Collection.defender_Sprites[2];
                break;
            case "Defense_Encourage":
                this.GetComponent<Image>().sprite = property_Images_Collection.defender_Sprites[3];
                break;
            case "Defense_Patience":
                this.GetComponent<Image>().sprite = property_Images_Collection.defender_Sprites[4];
                break;
            case "Defense_Responsibility":
                this.GetComponent<Image>().sprite = property_Images_Collection.defender_Sprites[5];
                break;
            case "Defense_Thronmail":
                this.GetComponent<Image>().sprite = property_Images_Collection.defender_Sprites[6];
                break;
            case "Defense_Null":
                this.GetComponent<Image>().sprite = property_Images_Collection.attack_Sprites[7];
                break;

        }


    }

    void NullisTransparent()
    {
        if (gameObject.GetComponent<Image>().sprite == null)
        {
            gameObject.GetComponent<Image>().color = Color.clear;
        }

        else
        {
            gameObject.GetComponent<Image>().color = Color.white;
        }
    }




}
