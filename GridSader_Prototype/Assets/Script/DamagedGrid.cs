using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagedGrid : MonoBehaviour
{
    public GameObject Black_Grid;   // ���� �׸���
    public GameObject Red_Grid;     // ���� �׸���

    // Start is called before the first frame update
    void Start()
    {
        Create_Grid(); // ���� �� ���� �׸��� ���� <= �ʼ��ƴ�
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void Create_Grid() // ���� �׸��� ���� <= �ʼ��ƴ�
    {
        for (float x = -6.7f; x <= -2f;)
        {
            for (float y = -2f; y <= 3;)
            {
                Instantiate(Black_Grid, new Vector3(x, y, 0), Quaternion.identity);
                y += 2.25f;
            }

            x += 2.25f;
        }
        // (grid-1)%3 %3->�� ����
        for (float x = 2.2f; x <= 6.7f;)
        {

            for (float y = -2f; y <= 3;)
            {
                Instantiate(Black_Grid, new Vector3(x, y, 0), Quaternion.identity);
                y += 2.25f;
            }
            x += 2.25f;
        }
    }

    // �����߿� �ǰݴ��� ������ ���� �׸��� ���� (1��)
    // ���߿� �� �׷����� ���� �� ���� ���� �ʿ�
    public void Create_Damaged_Grid_Team1(int Damaged_Grid_Num)
    {
        switch (Damaged_Grid_Num)
        {
            case 1: // �׸���1�� �ǰݹ����� �� �ش� ��ġ�� ���� �׸��� ����
                Vector3 num1 = new Vector3(-6.7f, 2.5f, 0.0f);
                Instantiate(Red_Grid, num1, Quaternion.identity);
                break;
            case 2:
                Vector3 num2 = new Vector3(-4.45f, 2.5f, 0f);
                Instantiate(Red_Grid, num2, Quaternion.identity);
                break;
            case 3:
                Vector3 num3 = new Vector3(-2.2f, 2.5f, 0.0f);
                Instantiate(Red_Grid, num3, Quaternion.identity);
                break;
            case 4:
                Vector3 num4 = new Vector3(-6.7f, 0.25f, 0.0f);
                Instantiate(Red_Grid, num4, Quaternion.identity);
                break;
            case 5:
                Vector3 num5 = new Vector3(-4.45f, 0.25f, 0.0f);
                Instantiate(Red_Grid, num5, Quaternion.identity);
                break;
            case 6:
                Vector3 num6 = new Vector3(-2.2f, 0.25f, 0.0f);
                Instantiate(Red_Grid, num6, Quaternion.identity);
                break;
            case 7:
                Vector3 num7 = new Vector3(-6.7f, -2.5f, 0.0f);
                Instantiate(Red_Grid, num7, Quaternion.identity);
                break;
            case 8:
                Vector3 num8 = new Vector3(-4.45f, -2.5f, 0.0f);
                Instantiate(Red_Grid, num8, Quaternion.identity);
                break;
            case 9:
                Vector3 num9 = new Vector3(-2.2f, -2.5f, 0.0f);
                Instantiate(Red_Grid, num9, Quaternion.identity);
                break;
        }
    }

    //�����߿� �ǰݴ��� ������ ���� �׸��� ���� (2��)
    public void Create_Damaged_Grid_Team2(int Damaged_Grid_Num)
    {
        switch (Damaged_Grid_Num)
        {
            case 1:
                Vector3 num1 = new Vector3(2.2f, 2.5f, 0.0f);
                Instantiate(Red_Grid, num1, Quaternion.identity);
                break;
            case 2:
                Vector3 num2 = new Vector3(4.45f, 2.5f, 0f);
                Instantiate(Red_Grid, num2, Quaternion.identity);
                break;
            case 3:
                Vector3 num3 = new Vector3(6.7f, 2.5f, 0.0f);
                Instantiate(Red_Grid, num3, Quaternion.identity);
                break;
            case 4:
                Vector3 num4 = new Vector3(2.2f, 0.25f, 0.0f);
                Instantiate(Red_Grid, num4, Quaternion.identity);
                break;
            case 5:
                Vector3 num5 = new Vector3(4.45f, 0.25f, 0.0f);
                Instantiate(Red_Grid, num5, Quaternion.identity);
                break;
            case 6:
                Vector3 num6 = new Vector3(6.7f, 0.25f, 0.0f);
                Instantiate(Red_Grid, num6, Quaternion.identity);
                break;
            case 7:
                Vector3 num7 = new Vector3(2.2f, -2.5f, 0.0f);
                Instantiate(Red_Grid, num7, Quaternion.identity);
                break;
            case 8:
                Vector3 num8 = new Vector3(4.45f, -2.5f, 0.0f);
                Instantiate(Red_Grid, num8, Quaternion.identity);
                break;
            case 9:
                Vector3 num9 = new Vector3(6.7f, -2.5f, 0.0f);
                Instantiate(Red_Grid, num9, Quaternion.identity);
                break;
        }
    }
}
 