using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pve_Ui_Manager : MonoBehaviour
{
    public int stage_Number; // �� �������� �ѹ�
    public GameObject[] StageButtons; //�������� ��ư ����
    public GameObject[] Stages;
    public Text StageNumber_text; //�������� �ѹ� �ؽ�Ʈ
    public GameObject returnButton;
    public GameObject nextButton;

    new GameObject camera; //ī�޶�
    float cameraMove_Distance; // ī�޶��� �̵��Ÿ�

    private void Start()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera");
        cameraMove_Distance = 41.1f;
        stage_Number = 1;
    }

    public void Map_Next_Pass() //���� ������
    {
        Camera_Move(true);
        StageButtons[stage_Number-1].SetActive(false);
        stage_Number++;
        StageButtons[stage_Number-1].SetActive(true);
        StageNumber_text.text = stage_Number.ToString();
        ReturnButton_active();
        NextButton_active();
    }

    public void Map_Returen_Pass() // ���� ������
    {
        Camera_Move(false);
        StageButtons[stage_Number-1].SetActive(false);
        stage_Number--;
        StageButtons[stage_Number-1].SetActive(true);
        StageNumber_text.text = stage_Number.ToString();
        ReturnButton_active();
        NextButton_active();
    }

    

    void Camera_Move(bool isRight) //ī�޶� ������
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
