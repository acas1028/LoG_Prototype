using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CharacterStats;

public class GoodsDataController : MonoBehaviour
{
    public CharacterSkill Goods_Data;

    public List<Sprite> imageList;
    public List<string> nameList;
    public List<string> descriptionList;
    public List<int> ValueList;

    public Image GoodsImage;
    public Text GoodsPrice;
    public string GoodsName;
    public string GoodsDescription;

    // Start is called before the first frame update
    void Start()
    {
        GoodsImage.sprite = imageList[(int)Goods_Data];
        GoodsPrice.text = ValueList[(int)Goods_Data].ToString();
        GoodsName = nameList[(int)Goods_Data];
        GoodsDescription = descriptionList[(int)Goods_Data];
    }

    // Update is called once per frame
    void Update()
    {
        GoodsImage.sprite = imageList[(int)Goods_Data];
        GoodsPrice.text = ValueList[(int)Goods_Data].ToString();
        GoodsName = nameList[(int)Goods_Data];
        GoodsDescription = descriptionList[(int)Goods_Data];
    }
}
