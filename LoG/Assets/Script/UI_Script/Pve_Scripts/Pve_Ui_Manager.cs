using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pve_Ui_Manager : MonoBehaviour
{
    public int stage_Numer; // 현 스테이지 넘버
    public GameObject[] StageButtons; //스테이지 버튼 모음
    public Text StageNumber_text; //스테이지 넘버 텍스트
    public GameObject returnButton;

    new GameObject camera; //카메라
    float cameraMove_Distance; // 카메라의 이동거리

    private void Start()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera");
        cameraMove_Distance = 43.6f;
        stage_Numer = 1;
    }

    public void Map_Next_Pass() //다음 맵으로
    {
        Camera_Move(true);
        StageButtons[stage_Numer-1].SetActive(false);
        stage_Numer++;
        StageButtons[stage_Numer-1].SetActive(true);
        StageNumber_text.text = stage_Numer.ToString();
        ReturnButton_active();
    }

    public void Map_Returen_Pass() // 이전 맵으로
    {
        Camera_Move(false);
        StageButtons[stage_Numer-1].SetActive(false);
        stage_Numer--;
        StageButtons[stage_Numer-1].SetActive(true);
        StageNumber_text.text = stage_Numer.ToString();
        ReturnButton_active();
    }

    

    void Camera_Move(bool isRight) //카메라 움직임
    {
        if (isRight == true)
        {
            camera.transform.Translate(cameraMove_Distance, 0, 0);
        }
        else
        {
            camera.transform.Translate(-cameraMove_Distance, 0, 0);
        }
    }

    void ReturnButton_active()
    {
        if (stage_Numer == 1)
        {
            returnButton.SetActive(false);
        }
        else
        {
            returnButton.SetActive(true);
        }
    }
}
