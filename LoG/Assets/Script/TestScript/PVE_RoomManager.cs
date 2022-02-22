
using UnityEngine;
using UnityEngine.UI;

using Photon.Realtime;
using Photon.Pun;
using ExitGames.Client.Photon;

public class PVE_RoomManager : MonoBehaviourPunCallbacks {
    public Text preemptiveCheck;
    public Text playerName;
    public Text roomStatusText;
    public Button readyButton;

    private void Start() {
        playerName.text = PhotonNetwork.LocalPlayer.NickName;
        preemptiveCheck.text = " ";

        if (!PhotonNetwork.IsConnected)
            roomStatusText.text = "로그인이 필요합니다";
        else {
            roomStatusText.text = "PVE";
            preemptiveCheck.text = "선공";
        }
    }

    public void LoadBattleScene() {
        PhotonNetwork.LoadLevel("BattleScene");
    }
}