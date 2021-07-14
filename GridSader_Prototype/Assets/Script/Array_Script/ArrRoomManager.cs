using System.Collections;

using UnityEngine;
using UnityEngine.UI;

using Photon.Realtime;
using Photon.Pun;

public enum ArrayPhase
{
    STANDBY = -1,
    FIRST1,
    SECOND12,
    FIRST23,
    SECOND34,
    FIRST45,
    SECOND5,
    END
}

public class ArrRoomManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private int arrayPhase;

    public Text playerName;
    public Text roomStatusText;
    public Text joinedPlayerList;
    public Text isEnemyReadyText;
    public Button readyButton;
    public Button arrCompleteButton;

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

        arrayPhase = (int)ArrayPhase.STANDBY;

        RenewPlayerList();
    }

    public void StartArrayPhase()
    {
        photonView.RPC("NextArrayPhase", RpcTarget.All);
    }

    [PunRPC]
    private void NextArrayPhase()
    {
        Player firstPlayer = PhotonNetwork.MasterClient;
        Player secondPlayer = PhotonNetwork.LocalPlayer;

        if (PhotonNetwork.IsMasterClient)
            foreach (var player in PhotonNetwork.PlayerListOthers)
                secondPlayer = player;

        if (readyButton.gameObject.activeSelf)
            readyButton.gameObject.SetActive(false);
        if (isEnemyReadyText.gameObject.activeSelf)
            isEnemyReadyText.gameObject.SetActive(false);

        arrayPhase++;
        if (arrayPhase == (int)ArrayPhase.END)
        {
            roomStatusText.text = "모든 배치가 완료되었습니다.";
            arrCompleteButton.gameObject.SetActive(false);
            PhotonNetwork.LoadLevel("BattleScene");
        }
        else if (arrayPhase % 2 == 0)
        {
            roomStatusText.text = "#" + (arrayPhase + 1) + " 선공 " + firstPlayer.NickName + ", " + (arrayPhase == (int)ArrayPhase.FIRST1 ? 1 : 2) + "개의 캐릭터를 배치하십시오.";
            if (PhotonNetwork.LocalPlayer == firstPlayer)
                arrCompleteButton.gameObject.SetActive(true);
            else if (PhotonNetwork.LocalPlayer == secondPlayer)
                arrCompleteButton.gameObject.SetActive(false);
        }
        else if (arrayPhase % 2 == 1)
        {
            roomStatusText.text = "#" + (arrayPhase + 1) + " 후공 " + secondPlayer.NickName + ", " + (arrayPhase == (int)ArrayPhase.SECOND5 ? 1 : 2) + "개의 캐릭터를 배치하십시오.";
            if (PhotonNetwork.LocalPlayer == firstPlayer)
                arrCompleteButton.gameObject.SetActive(false);
            else if (PhotonNetwork.LocalPlayer == secondPlayer)
                arrCompleteButton.gameObject.SetActive(true);
        }

        var ap = (ArrayPhase)arrayPhase;
        Debug.Log("<color=yellow>RPC Success with ArrayPhase: </color><color=lightblue>" + ap + "</color>");
    }

    public int GetArrayPhase()
    {
        return arrayPhase;
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

        PhotonNetwork.LoadLevel("LobbyScene");
    }
    #endregion
}
