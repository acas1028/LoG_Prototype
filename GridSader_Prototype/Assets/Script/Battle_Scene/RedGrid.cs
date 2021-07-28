using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedGrid : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, BattleManager.Instance.bM_Timegap);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
