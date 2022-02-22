using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckSelectionController : MonoBehaviour
{
    public GameObject DeckList;

    public GameObject SelectedDeckInfo;

    void Start()
    {
        
    }

    public void ActivatingDeckList()
    {
        if (DeckList.activeSelf)
            DeckList.SetActive(false);
        else
            DeckList.SetActive(true);
    }
}
