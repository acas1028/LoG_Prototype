using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PveStageClearManager : MonoBehaviour
{
    public static PveStageClearManager instance;

    public int clearstage;

    public GameObject[] stageButtons;

    public Sprite[] pve_Button_Images;

    private void Awake()
    {
        if (!instance)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        StageLock();
    }

    void StageLock()
    {
        for(int i=0; i< stageButtons.Length;i++)
        {
            if(i>=clearstage+1)
            {
                stageButtons[i].GetComponent<Button>().enabled = false;
                stageButtons[i].GetComponent<Image>().sprite = pve_Button_Images[0];
            }

            else if(i == clearstage)
            {
                stageButtons[i].GetComponent<Button>().enabled = true;
                stageButtons[i].GetComponent<Image>().sprite = pve_Button_Images[1];
            }

            else
            {
                stageButtons[i].GetComponent<Button>().enabled = true;
                stageButtons[i].GetComponent<Image>().sprite = pve_Button_Images[2];
            }
        }
    }

}
