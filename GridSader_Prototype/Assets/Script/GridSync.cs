using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// ���ӿ� ������ �÷��̾�A, B�� ���� ������ �׸��� ������ �������� ��ũ��Ʈ
// BattleManager�� ���� ������ �Ѱ��� �÷��̾�A�� �׸��� ������ �÷��̾�B���� �޾ƿͼ� �ν��Ͻ�ȭ�ϴ� ���
public class GridSync : MonoBehaviourPunCallbacks, IPunObservable
{
    BattleManager battleManager = BattleManager.Instance;
    [Tooltip("����� Team_Character ������Ʈ")]
    public GameObject[] bM_Character_Team_Enemy;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region ������ ������ ����ȭ

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // We own this player: send the others our data
            // �� ���� �׸��� ������ ������ ����
            // photonView.IsMine == true �� ��
            stream.SendNext(battleManager.bM_Character_Team1);
        }
        else
        {
            // Network player, receive data
            // �ٸ� Ŭ���̾�Ʈ�� ������ �׸��� ������ �޾ƿ�
            // photonView.IsMine == false �� ��
            bM_Character_Team_Enemy = (GameObject[])stream.ReceiveNext();
        }
    }

    #endregion
}
