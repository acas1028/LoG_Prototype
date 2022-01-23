using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateShop : MonoBehaviour
{
    public GameObject Shop;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenShop()
    {
        Shop.SetActive(true);
    }

    public void CloseShop()
    {
        GameObject description = GameObject.FindWithTag("Description_Popup");
        if(description != null)
            description.SetActive(false);

        Shop.SetActive(false);
    }
}