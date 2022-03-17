using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class BattleEnemyPveSprite : MonoBehaviour
{

    private void Update()
    {
        PveSpriteInset();
    }

    void PveSpriteInset()
    {
        object o_isPVE;
        PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("IsPVE", out o_isPVE);

        if((bool)o_isPVE == true)
        {
            for(int i=0;i <5;i++)
            {
                Character Team2CS = BattleManager.Instance.bM_Character_Team2[i].GetComponent<Character>();
                Team2CS.spriteManager.SetPveCharacterSprite();
                //현재 tag 문제로 작동 안함.
            }
            
        }
    }

}
