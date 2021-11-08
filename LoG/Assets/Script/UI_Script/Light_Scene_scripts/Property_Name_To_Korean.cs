using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Property_Name_To_Korean : MonoBehaviour
{
    public string PropertyNameTOKorean(string property_Name)
    {
        switch(property_Name)
        {
            //공격 특성
            case "Attack_Confidence":
                return "자신감";
            case "Attack_Executioner":
                return "처형자";
            case "Attack_Struggle":
                return "발악";
            case "Attack_Ranger":
                return "명사수";
            case "Attack_ArmorPiercer":
                return "철갑탄";
            case "Attack_DivineShield":
                return "천상의보호막";
            case "Attack_Sturdy":
                return "기합";

            //밸런스 특성
            case "Balance_Blessing":
                return "축복";
            case "Balance_GBGH":
                return "모아니면 도";
            case "Balance_Smoke":
                return "연막탄";
            case "Balance_Survivor":
                return "생존자";
            case "Balance_Curse":
                return "저주";
            case "Balance_WideCounter":
                return "광역반격";
            case "Balance_DestinyBond":
                return "길동무";

            // 방어 특성
            case "Defense_Disarm":
                return "무장해제";
            case "Defense_Coward":
                return "겁쟁이";
            case "Defense_Patience":
                return "인내심";
            case "Defense_Responsibility":
                return "책임감";
            case "Defense_Barrier":
                return "방벽";
            case "Defense_Encourage":
                return "격려";
            case "Defense_Thronmail":
                return "가시갑옷";
        }

        return default;

    }


    
}
