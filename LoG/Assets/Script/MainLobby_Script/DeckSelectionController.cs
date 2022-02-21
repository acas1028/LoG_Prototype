using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckSelectionController : MonoBehaviour
{
    public GameObject DeckList;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
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
