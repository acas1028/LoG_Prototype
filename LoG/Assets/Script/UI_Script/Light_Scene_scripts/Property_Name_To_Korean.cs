using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Property_Name_To_Korean : MonoBehaviour
{
    public string PropertyNameTOKorean(string property_Name)
    {
        switch(property_Name)
        {
            //���� Ư��
            case "Attack_Confidence":
                return "�ڽŰ�";
            case "Attack_Executioner":
                return "ó����";
            case "Attack_Struggle":
                return "�߾�";
            case "Attack_Ranger":
                return "����";
            case "Attack_ArmorPiercer":
                return "ö��ź";
            case "Attack_DivineShield":
                return "õ���Ǻ�ȣ��";
            case "Attack_Sturdy":
                return "����";

            //�뷱�� Ư��
            case "Balance_Blessing":
                return "�ູ";
            case "Balance_GBGH":
                return "��ƴϸ� ��";
            case "Balance_Smoke":
                return "����ź";
            case "Balance_Survivor":
                return "������";
            case "Balance_Curse":
                return "����";
            case "Balance_WideCounter":
                return "�����ݰ�";
            case "Balance_DestinyBond":
                return "�浿��";

            // ��� Ư��
            case "Defense_Disarm":
                return "��������";
            case "Defense_Coward":
                return "������";
            case "Defense_Patience":
                return "�γ���";
            case "Defense_Responsibility":
                return "å�Ӱ�";
            case "Defense_Barrier":
                return "�溮";
            case "Defense_Encourage":
                return "�ݷ�";
            case "Defense_Thronmail":
                return "���ð���";
        }

        return default;

    }


    
}
