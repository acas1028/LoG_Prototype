using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sprite_Image_flipx : MonoBehaviour
{
    private int Change_point;

    private void Start()
    {
        Change_point = 0;
    }

    private void Update()
    {
        if (BattleManager.Instance.bM_Character_Team1.Count == 5 && BattleManager.Instance.bM_Character_Team2.Count == 5) //예외 처리
        {
            Change_Team2_flip();
        }
    }


    private void Change_Team2_flip()
    {
        if (BattleManager.Instance.bM_Character_Team2[0] == null)
            return;
        if (Change_point == 1)
            return;

        for(int i=0; i< BattleManager.Instance.bM_Character_Team2.Count; i++)
        {
            BattleManager.Instance.bM_Character_Team2[i].GetComponent<SpriteRenderer>().flipX = true;

        }

        Change_point = 1;

        
        
    }

}
