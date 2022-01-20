using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PVE_InventoryID : MonoBehaviour
{
    [SerializeField]
    private GameObject MyTeamData;
    // Start is called before the first frame update
    void Start()
    {
        for(int i=0;i<7;i++)
        {
            Debug.Log(MyTeamData.transform.GetChild(i).GetComponent<Character>().character_ID);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
