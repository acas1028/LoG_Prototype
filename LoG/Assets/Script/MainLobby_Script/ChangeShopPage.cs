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
        nextPage.SetActive(true);
        currentPage.SetActive(false);
    }
}
