using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PurchasePopupDataChange : MonoBehaviour
{
    public Image MasteryImage;
    public Text MasteryName;
    public Text MasteryPrice;


    public Sprite dataSprite;
    public string dataName;
    public string dataPrice;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MasteryImage.sprite = dataSprite;
        MasteryName.text = dataName;
        MasteryPrice.text = dataPrice;
    }
}
