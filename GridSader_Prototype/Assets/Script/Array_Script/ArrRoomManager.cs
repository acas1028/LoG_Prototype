using System.Collections;

using UnityEngine;
using UnityEngine.UI;

using Photon.Realtime;
using Photon.Pun;

public enum ArrayPhase
{
    FIRST1,
    SECOND12,
    FIRST23,
    SECOND34,
    FIRST45,
    SECOND5
}

public class ArrRoomManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private int arrayPhase;

    private bool isArrCompleteButtonClicked;

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
        isArrCompleteButtonClicked = false;

        arrCompleteButton.onClick.AddListener(() => isArrCompleteButtonClicked = true);

        StartArrayPhase();
        RenewPlayerList();
    }

    public void StartArrayPhase()
    {
        StartCoroutine(SetArrayPhase());
    }

    IEnumerator SetArrayPhase()
    {
        Player secondPlayer = PhotonNetwork.LocalPlayer; // 초기화

        foreach (var player in PhotonNetwork.PlayerListOthers)
        {
            secondPlayer = player; // LocalPlayer 는 방장이므로 방장을 제외한 플레이어가 secondPlayer 가 된다.
        }

        arrayPhase = (int)ArrayPhase.FIRST1;
        roomStatusText.text = "#1 선공 " + PhotonNetwork.MasterClient.NickName + " 1개의 캐릭터를 배치하십시오.";
        
        yield return new WaitUntil(() => isArrCompleteButtonClicked);

        isArrCompleteButtonClicked = false;
        arrayPhase = (int)ArrayPhase.SECOND12;
        roomStatusText.text = "#2 후공 " + secondPlayer.NickName + " 2개의 캐릭터를 배치하십시오.";

        yield return new WaitUntil(() => isArrCompleteButtonClicked);

        isArrCompleteButtonClicked = false;
        arrayPhase = (int)ArrayPhase.FIRST23;
        roomStatusText.text = "#3 선공 " + PhotonNetwork.MasterClient.NickName + " 2개의 캐릭터를 배치하십시오.";

        yield return new WaitUntil(() => isArrCompleteButtonClicked);

        isArrCompleteButtonClicked = false;
        arrayPhase = (int)ArrayPhase.SECOND34;
        roomStatusText.text = "#4 후공 " + secondPlayer.NickName + " 2개의 캐릭터를 배치하십시오.";

        yield return new WaitUntil(() => isArrCompleteButtonClicked);

        isArrCompleteButtonClicked = false;
        arrayPhase = (int)ArrayPhase.FIRST45;
        roomStatusText.text = "#5 선공 " + PhotonNetwork.MasterClient.NickName + " 2개의 캐릭터를 배치하십시오.";

        yield return new WaitUntil(() => isArrCompleteButtonClicked);

        isArrCompleteButtonClicked = false;
        arrayPhase = (int)ArrayPhase.SECOND5;
        roomStatusText.text = "#6 후공 " + secondPlayer.NickName + " 2개의 캐릭터를 배치하십시오.";

        yield return new WaitUntil(() => isArrCompleteButtonClicked);

        isArrCompleteButtonClicked = false;
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

    public int GetArrayPhase()
    {
        return arrayPhase;
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
