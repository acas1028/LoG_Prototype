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
                //lock �������� ����
            }

            else
            {
                stageButtons[i].GetComponent<Button>().enabled = true;
            }
        }
    }
    //clear �������� �ѹ� ���Ÿ� �ϸ� ��. �׷��� ���ؼ� battlemanager���� �����ǴϽ��� �̸� �����Ű���� �ؾ���.

}
