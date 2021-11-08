using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pve_Ui_Manager : MonoBehaviour
{
    public int stage_Numer; // �� �������� �ѹ�
    public GameObject[] StageButtons; //�������� ��ư ����
    public Text StageNumber_text; //�������� �ѹ� �ؽ�Ʈ
    public GameObject returnButton;

    new GameObject camera; //ī�޶�
    float cameraMove_Distance; // ī�޶��� �̵��Ÿ�

    private void Start()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera");
        cameraMove_Distance = 43.6f;
        stage_Numer = 1;
    }

    public void Map_Next_Pass() //���� ������
    {
        Camera_Move(true);
        StageButtons[stage_Numer-1].SetActive(false);
        stage_Numer++;
        StageButtons[stage_Numer-1].SetActive(true);
        StageNumber_text.text = stage_Numer.ToString();
        ReturnButton_active();
    }

    public void Map_Returen_Pass() // ���� ������
    {
        Camera_Move(false);
        StageButtons[stage_Numer-1].SetActive(false);
        stage_Numer--;
        StageButtons[stage_Numer-1].SetActive(true);
        StageNumber_text.text = stage_Numer.ToString();
        ReturnButton_active();
    }

    

    void Camera_Move(bool isRight) //ī�޶� ������
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
