using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrayed_Data : MonoBehaviour
{
    public static Arrayed_Data instance;
    public List<GameObject> team1 = new List<GameObject>();
    public List<GameObject> team2 = new List<GameObject>();
    private void Awake()
    {
        // Singleton
        if (instance == null)
        {
            instance = this;
            Debug.Log("instance ArrayData");
        }
        else
        {
            Debug.Log("Delete ArrayData");
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }
    private void Update()
    {
    }
    public void infoTeam2()
    {
        for(int i=0;i<5;i++)
        {
            Debug.Log("Array Enemy Data "+team2[i].GetComponent<Character>().character_ID);
        }
    }
}
