using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckInfo : MonoBehaviour
{
    public int selectedDeckNumber;

    public Sprite[] selectedDeckSprites;
    public Image selectedDeckImage;

    public int attackNumber;
    public int balancedNumber;
    public int defenderNumber;

    public Text attackText;
    public Text balancedText;
    public Text defenderText;

    public GameObject DeckPanel;
    public void initSelectDeck()
    {
        selectedDeckImage.sprite = selectedDeckSprites[selectedDeckNumber - 1];

        attackText.text = attackNumber.ToString();
        balancedText.text = balancedNumber.ToString();
        defenderText.text = defenderNumber.ToString();

        if (DeckPanel.activeSelf) return;

        if (attackNumber + balancedNumber + defenderNumber != 7) DeckPanel.SetActive(true);

    }
    public void EmptyPage()
    {
        attackNumber = 0;
        balancedNumber = 0;
        defenderNumber = 0;
    }

    public void CopyPage(GameObject Page)
    {
        DeckInfo Data = Page.GetComponent<DeckInfo>();
        attackNumber = Data.attackNumber;
        balancedNumber = Data.balancedNumber;
        defenderNumber = Data.defenderNumber;
        selectedDeckNumber = Data.selectedDeckNumber;
    }
}
