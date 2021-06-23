using UnityEngine;
using UnityEngine.UI;

using Photon.Realtime;
using Photon.Pun;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public Text playerName;
    public Text roomStatusText;
    public Text joinedPlayerList;
    public Text isEnemyReadyText;
    public Button readyButton;

    private void Start()
    {
        playerName.text = PhotonNetwork.LocalPlayer.NickName;
        if (PhotonNetwork.OfflineMode)
            roomStatusText.text = "<오프라인 모드>";
        else if (!PhotonNetwork.IsConnected)
            roomStatusText.text = "로그인이 필요합니다";
        else
            roomStatusText.text = " ";
        isEnemyReadyText.text = " ";

        RenewPlayerList();
    }

    private void RenewPlayerList()
    {
        string playerList = string.Empty;

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            playerList += System.Environment.NewLine + player.NickName;
        }

        joinedPlayerList.text = "룸에 있는 플레이어 <color=#912900>" + playerList + "</color>";
    }

    public void SetReadyButtonStatus(bool isReady)
    {
        ColorBlock cb = readyButton.colors;
        cb.normalColor = new Color(isReady ? 1.0f : 0.0f, 0.0f, 0.0f);
        cb.selectedColor = new Color(isReady ? 1.0f : 0.0f, 0.0f, 0.0f);
        readyButton.colors = cb;
        readyButton.GetComponentInChildren<Text>().text = isReady ? "준비 완료!" : "준비!";
    }

    public void SetIsEnemyReadyText(bool isEnemyReady)
    {
        if (isEnemyReady)
            isEnemyReadyText.text = "상대 준비 완료";
        else
            isEnemyReadyText.text = "상대 준비 미완료";
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    #region 포톤 콜백 함수
    public override void OnPlayerEnteredRoom(Player other)
    {
        roomStatusText.text = "상대 플레이어 " + other.NickName + " 입장";
        Debug.Log("<color=yellow>플레이어 " + other.NickName + " 입장</color>");
        RenewPlayerList();
    }

    public override void OnPlayerLeftRoom(Player other)
    {
        roomStatusText.text = "상대 플레이어 " + other.NickName + " 퇴장";
        Debug.Log("<color=yellow>플레이어 " + other.NickName + " 퇴장</color>");
        RenewPlayerList();
    }

    public override void OnLeftRoom()
    {
        Debug.Log("<color=yellow>OnLeftRoom() 호출\n룸을 나갑니다. 로비로 이동합니다.</color>");
        if (PhotonNetwork.IsConnected)
            PhotonNetwork.Disconnect();
        Debug.Log("<color=yellow>Disconnected\n연결 해제됨</color>");
        PhotonNetwork.LoadLevel("LobbyScene");
    }
    #endregion
}
