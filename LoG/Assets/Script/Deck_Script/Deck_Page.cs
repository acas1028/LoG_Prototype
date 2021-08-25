using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck_Page : MonoBehaviour
{
    public DeckDataSync deckDataSync;
    public int P_num;

    public void Click_Deck()
    {
        Deck_Manager DM = Deck_Manager.instance;

        P_num = this.gameObject.GetComponent<Deck_Page>().P_num;
        DM.Page_Num = P_num;

        //deckDataSync.GetData(P_num);

        DM.StartCoroutine("Switch_Page");
    }
}
