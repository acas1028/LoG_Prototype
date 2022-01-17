using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeShopPage : MonoBehaviour
{
    public GameObject currentPage;
    public GameObject nextPage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangePage()
    {
        GameObject description = GameObject.FindWithTag("Description_Popup");
        if (description != null)
            description.SetActive(false);
        nextPage.SetActive(true);
        currentPage.SetActive(false);
    }
}
