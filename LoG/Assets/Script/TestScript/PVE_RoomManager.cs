
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
    }
}