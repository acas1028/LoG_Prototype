using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// 게임에 참여한 플레이어A, B가 서로 상대방의 그리드 정보를 가져오는 스크립트
// BattleManager를 통해 서버로 넘겨진 플레이어A의 그리드 정보를 플레이어B에서 받아와서 인스턴스화하는 방식
public class GridSync : MonoBehaviourPunCallbacks, IPunObservable
{
    BattleManager battleManager = BattleManager.Instance;
    [Tooltip("상대편 Team_Character 오브젝트")]
    public GameObject[] bM_Character_Team_Enemy;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region 서버와 데이터 동기화

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // We own this player: send the others our data
            // 내 팀의 그리드 정보를 서버에 전송
            // photonView.IsMine == true 일 때
            stream.SendNext(battleManager.bM_Character_Team1);
        }
        else
        {
            // Network player, receive data
            // 다른 클라이언트가 전송한 그리드 정보를 받아옴
            // photonView.IsMine == false 일 때
            bM_Character_Team_Enemy = (GameObject[])stream.ReceiveNext();
        }
    }

    #endregion
}
