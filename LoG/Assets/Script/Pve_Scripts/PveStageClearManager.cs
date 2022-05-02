using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PveStageClearManager : MonoBehaviour
{
    public static PveStageClearManager instance;

    public int clearstage;

    public GameObject[] stageButtons;

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
                //lock 사진으로 변경
            }

            else
            {
                stageButtons[i].GetComponent<Button>().enabled = true;
            }
        }
    }
    //clear 스테이지 넘버 갱신만 하면 됨. 그러기 위해선 battlemanager에서 게임피니쉬에 이를 변경시키도록 해야함.

}
