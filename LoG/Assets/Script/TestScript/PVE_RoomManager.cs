
using UnityEngine;
using UnityEngine.UI;

using Photon.Realtime;
using Photon.Pun;
using ExitGames.Client.Photon;

public class PVE_RoomManager : MonoBehaviourPunCallbacks {
    public Text preemptiveCheck;
    public Text playerName;
    public Text roomStatusText;

    private void Start() {
        playerName.text = PhotonNetwork.LocalPlayer.NickName;
        preemptiveCheck.text = " ";

        if (!PhotonNetwork.IsConnected)
            roomStatusText.text = "������ ������� �ʾҽ��ϴ�.";
        else {
            roomStatusText.text = "PVE";
            preemptiveCheck.text = "����";
        }

        InitCustomProperties();
    }

    private void InitCustomProperties() {
        bool result = false;

        Hashtable table = new Hashtable() { { "RoundWinCount", 0 } };
        result = PhotonNetwork.SetPlayerCustomProperties(table);
        if (!result) {
            Debug.LogError("PlayerCustomProperties ����ȭ ����");
        }

        if (PhotonNetwork.IsMasterClient) {
            table = new Hashtable() { { "RoundCount", 1 } };
            result = PhotonNetwork.CurrentRoom.SetCustomProperties(table);
            if (!result) {
                Debug.LogError("RoundCount ����ȭ ����");
            }

            table = new Hashtable() { { "IsPVE", true } };
            result = PhotonNetwork.CurrentRoom.SetCustomProperties(table);
            if (!result) {
                Debug.LogError("IsPVE ����ȭ ����");
            }
        }
    }
}