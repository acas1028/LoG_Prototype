using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillMessage_Explanation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Skill_Message_explanation(string skillname)
    {
        switch (skillname)
        {
            case "ó����":
                this.GetComponent<Text>().text = "���� ������ �ѹ��� ����";
                break;
            case "�߾�":
                this.GetComponent<Text>().text = "���ظ� ������, ���ݷ� ����";
                break;
            case "����":
                this.GetComponent<Text>().text = "���� ĳ���� ����";
                break;
            case "õ���� ��ȣ��":
                this.GetComponent<Text>().text = "���� 1ȸ ����";
                break;
            case "�˰���":
                this.GetComponent<Text>().text = "1ȸ ���� �� �ݰ�";
                break;
            case "�� �ƴϸ� ��":
                this.GetComponent<Text>().text = "ó���� ���ݷ� ����, �������� ���� ����";
                break;
            case "�ູ":
                this.GetComponent<Text>().text = "�Ĺ� ���ݷ� ����, ���� ���� ����";
                break;
            case "����ź":
                this.GetComponent<Text>().text = "��� ���ݷ� ����";
                break;
            case "����":
                this.GetComponent<Text>().text = "���� ���";
                break;
            case "�����ݰ�":
                this.GetComponent<Text>().text = "������ ���� 4ĭ �ݰ�";
                break;

                //���Ŀ� �߰� 


        }
    }
}
