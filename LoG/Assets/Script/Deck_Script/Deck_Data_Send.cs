using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck_Data_Send : MonoBehaviour
{
    static public Deck_Data_Send instance = null;
    public GameObject characterPrefab;
    public GameObject[,] Save_Data;

    public int lastPageNum;

    // Start is called before the first frame update
    private void Awake()
    {
        // Singleton
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        Save_Data = new GameObject[5, 7];

        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                Save_Data[i, j] = Instantiate(characterPrefab, transform.Find("Page " + (i + 1)));
            }
        }
    }
}
