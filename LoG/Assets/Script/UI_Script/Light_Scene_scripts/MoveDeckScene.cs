using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveDeckScene : MonoBehaviour
{
    public void MoveDeck()
    {
        SceneManager.LoadScene("Deck_Scene");
    }
}
