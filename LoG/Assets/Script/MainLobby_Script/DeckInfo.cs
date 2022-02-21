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
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        selectedDeckImage.sprite = selectedDeckSprites[selectedDeckNumber - 1];

        attackText.text = attackNumber.ToString();
        balancedText.text = balancedNumber.ToString();
        defenderText.text = defenderNumber.ToString();

        if (DeckPanel.activeSelf) return;

        if (attackNumber + balancedNumber + defenderNumber != 7) DeckPanel.SetActive(true);
    }

    public void SelectDeck()
    {
        if (attackNumber + balancedNumber + defenderNumber != 7) return;

        GameObject.FindWithTag("SelectedDeck").GetComponent<DeckSelectionController>().selectedDeckNumber = selectedDeckNumber;
    }
}
