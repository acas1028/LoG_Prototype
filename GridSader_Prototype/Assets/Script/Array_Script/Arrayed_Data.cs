using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrayed_Data : MonoBehaviour
{
    public static Arrayed_Data instance;
    public GameObject[] team1;
    public GameObject[] team2;

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
