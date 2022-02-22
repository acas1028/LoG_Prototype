using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckListData : MonoBehaviour
{
    private Deck_Data_Send DeckData;
    [SerializeField]
    private GameObject[] infoDeck;
    [SerializeField]
    private GameObject DeckSelection;
    private void Start()
    {
        DeckData = Deck_Data_Send.instance;
        ResetListData();
        StartCoroutine("InitListData");
    }

    private void ResetListData()
    {
        for(int i=0;i<5;i++)
        {
            infoDeck[i].GetComponent<DeckInfo>().EmptyPage();
        }
    }
    IEnumerator InitListData()
    {
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < 5; i++)
        {
            DeckInfo DI = infoDeck[i].GetComponent<DeckInfo>();
            for (int j = 0; j < 7; j++)
            {
                Character cs = DeckData.DeckPage[i].transform.GetChild(j).GetComponent<Character>();
                if (cs.character_ID == 0)
                {
                    DI.EmptyPage();
                    break;
                }
                else
                {
                    if (cs.character_Type == CharacterStats.CharacterType.Attacker)
                        DI.attackNumber++;
                    if (cs.character_Type == CharacterStats.CharacterType.Balance)
                        DI.balancedNumber++;
                    if (cs.character_Type == CharacterStats.CharacterType.Defender)
                        DI.defenderNumber++;
                }
            }
            DI.initSelectDeck();
        }
        DeckInfo Lastinfo = infoDeck[DeckData.lastPageNum].GetComponent<DeckInfo>();
        DeckInfo DS = DeckSelection.GetComponent<DeckInfo>();
        DS.CopyPage(infoDeck[DeckData.lastPageNum]);
        DS.initSelectDeck();
    }

    public void ChangeDeckPage(GameObject DeckPage)
    {
        DeckInfo DI = DeckPage.GetComponent<DeckInfo>();
        DeckData.lastPageNum = DI.selectedDeckNumber;
        DeckSelection.GetComponent<DeckInfo>().CopyPage(DeckPage);
        DeckSelection.GetComponent<DeckInfo>().initSelectDeck();
    }
}
