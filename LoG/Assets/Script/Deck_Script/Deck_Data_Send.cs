using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck_Data_Send : MonoBehaviour
{
    static public Deck_Data_Send instance;
    public GameObject[] Save_Data;
    // Start is called before the first frame update
    private void Awake()
    {
        // Singleton
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);
    }
}
