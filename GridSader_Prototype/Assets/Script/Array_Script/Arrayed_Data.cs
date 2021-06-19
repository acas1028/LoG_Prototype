using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrayed_Data : MonoBehaviour
{
    public static GameObject instance;
    public GameObject[] team1;
    public GameObject[] team2;

    private void Awake()
    {
        if (instance)
            Destroy(instance);
        instance = this.gameObject;

        DontDestroyOnLoad(this.gameObject);
    }
}
