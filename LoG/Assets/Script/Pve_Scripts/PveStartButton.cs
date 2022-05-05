using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PveStartButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SettingStage(int stageNumber)
    {
        CSVManager.Instance.SettingStage(stageNumber);
    }
}
