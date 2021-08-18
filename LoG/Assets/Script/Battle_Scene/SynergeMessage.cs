using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SynergeMessage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Message(int teamNumber, string synergeName)
    {
        this.GetComponent<Text>().text = teamNumber + "팀 " + synergeName + "시너지 발동!";

        Invoke("Disable", BattleManager.Instance.bM_Timegap);
    }
}
