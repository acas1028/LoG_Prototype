using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pve_Ui_Manager : MonoBehaviour
{
    public int stage_Number; // 현 스테이지 넘버
    public GameObject[] StageButtons; //스테이지 버튼 모음
    public GameObject[] Stages;
    public Text StageNumber_text; //스테이지 넘버 텍스트
    public GameObject returnButton;
    public GameObject nextButton;

    new GameObject camera; //카메라
    float cameraMove_Distance; // 카메라의 이동거리

    private void Start()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera");
        cameraMove_Distance = 41.1f;
        stage_Number = 1;
    }

    public void Map_Next_Pass() //다음 맵으로
    {
        Camera_Move(true);
        StageButtons[stage_Number-1].SetActive(false);
        stage_Number++;
        StageButtons[stage_Number-1].SetActive(true);
        StageNumber_text.text = stage_Number.ToString();
        ReturnButton_active();
        NextButton_active();
    }

    public void Map_Returen_Pass() // 이전 맵으로
    {
        Camera_Move(false);
        StageButtons[stage_Number-1].SetActive(false);
        stage_Number--;
        StageButtons[stage_Number-1].SetActive(true);
        StageNumber_text.text = stage_Number.ToString();
        ReturnButton_active();
        NextButton_active();
    }

    

    void Camera_Move(bool isRight) //카메라 움직임
    {
        if (isRight == true)
        {
            for(int i =0; i< Stages.Length;i++)
            {
                if(Stages[i].activeSelf == true)
                {
                    Stages[i].SetActive(false);
                    Stages[i + 1].SetActive(true);
                    return;
                }
            }
        }
        else
        {
            for (int i = 0; i < Stages.Length; i++)
            {
                if (Stages[i].activeSelf == true)
                {
                    Stages[i].SetActive(false);
                    Stages[i-1].SetActive(true);
                    return;
                }
            }
        }
    }

    void ReturnButton_active()
    {
        if (stage_Number == 1)
        {
            returnButton.SetActive(false);
        }
        else
        {
            returnButton.SetActive(true);
        }
    }

    void NextButton_active()
    {
        if (stage_Number == 4)
        {
            nextButton.SetActive(false);
        }
        else
        {
            nextButton.SetActive(true);
        }
    }
}
