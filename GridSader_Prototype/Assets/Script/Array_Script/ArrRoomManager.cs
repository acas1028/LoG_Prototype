
using UnityEngine;
using UnityEngine.UI;

using Photon.Realtime;
using Photon.Pun;
using ExitGames.Client.Photon;

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

    public Text preemptiveCheck;
    public Text playerName;
    public Text roomStatusText;
    public Text joinedPlayerList;
    public Text isEnemyReadyText;
    public Text timeText;
    public Button readyButton;
    public Button arrCompleteButton;

    Player firstPlayer;
    Player secondPlayer;
    bool isPreemptivePlayerSet;

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
        preemptiveCheck.text = " ";

        firstPlayer = PhotonNetwork.LocalPlayer;
        secondPlayer = PhotonNetwork.LocalPlayer;
        isPreemptivePlayerSet = false;

        arrayPhase = (int)ArrayPhase.STANDBY;

        RenewPlayerList();
    }

    #region 외부에서 호출되는 public 함수
    public bool IsPlayerPreemptive()
    {
        return firstPlayer == PhotonNetwork.LocalPlayer;
    }

    public int GetArrayPhase()
    {
        return arrayPhase;
    }

    public bool IsAllPlayersJoined()
    {
        return PhotonNetwork.CurrentRoom.PlayerCount >= 2;
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

    public void SetPreemptivePlayer()
    {
        Debug.Log("SetPreemptivePlayer() 호출");

        bool result = false;
        result = IsAllPlayersJoined();
        if (!result)
        {
            Debug.LogError("플레이어 한 명이 나간 상태입니다. 선/후공 결정을 취소합니다. 게임을 진행할 수 없습니다.");
            return;
        }

        bool isPreemptive = Random.Range(0, 2) == 0 ? false : true;
        Hashtable player_1_table = new Hashtable() { { "IsPreemptive", isPreemptive } };
        Hashtable player_2_table = new Hashtable() { { "IsPreemptive", !isPreemptive } };

        result = PhotonNetwork.PlayerList[0].SetCustomProperties(player_1_table);
        if (!result)
            Debug.LogWarning("플레이어 1 선공여부 동기화 실패");
        result = PhotonNetwork.PlayerList[1].SetCustomProperties(player_2_table);
        if (!result)
            Debug.LogWarning("플레이어 2 선공여부 동기화 실패");
    }

    public void StartArrayPhase()
    {
        bool result = IsAllPlayersJoined();
        if (!result && !PhotonNetwork.OfflineMode)
        {
            Debug.LogError("플레이어 한 명이 나간 상태입니다. 교차 선택을 취소합니다. 게임을 진행할 수 없습니다.");
            return;
        }

        photonView.RPC("NextArrayPhase", RpcTarget.All);
    }
    #endregion

    [PunRPC]
    private void NextArrayPhase()
    {
        if (readyButton.gameObject.activeSelf)
            readyButton.gameObject.SetActive(false);
        if (isEnemyReadyText.gameObject.activeSelf)
            isEnemyReadyText.gameObject.SetActive(false);
        if (timeText.gameObject.activeSelf)
            timeText.gameObject.SetActive(false);

        timeText.gameObject.SetActive(true);

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
            if (IsPlayerPreemptive())
                arrCompleteButton.gameObject.SetActive(true);
            else
                arrCompleteButton.gameObject.SetActive(false);
        }
        else if (arrayPhase % 2 == 1)
        {
            roomStatusText.text = "#" + (arrayPhase + 1) + " 후공 " + secondPlayer.NickName + ", " + (arrayPhase == (int)ArrayPhase.SECOND5 ? 1 : 2) + "개의 캐릭터를 배치하십시오.";
            if (IsPlayerPreemptive())
                arrCompleteButton.gameObject.SetActive(false);
            else
                arrCompleteButton.gameObject.SetActive(true);
        }

        Debug.Log("<color=yellow>RPC Success with ArrayPhase: </color><color=lightblue>" + (ArrayPhase)arrayPhase + "</color>");
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

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        // 선, 후공 결정에 관한 데이터이면서 '나'의 선, 후공 데이터인 경우에만 아래 과정을 거친다.
        if (changedProps.ContainsKey("IsPreemptive") && targetPlayer == PhotonNetwork.LocalPlayer && !isPreemptivePlayerSet)
        {
            Debug.LogFormat("Player <color=lightblue>#{0} {1}</color> Properties Updated due to <color=green>{2}</color>", targetPlayer.ActorNumber, targetPlayer.NickName, changedProps.ToString());

            object o_is_preemptive;
            bool is_preemptive;
            targetPlayer.CustomProperties.TryGetValue("IsPreemptive", out o_is_preemptive);
            is_preemptive = (bool)o_is_preemptive;

            if (is_preemptive)
            {
                foreach (var player in PhotonNetwork.PlayerListOthers)
                {
                    secondPlayer = player;
                }
                preemptiveCheck.text = "선공";
            }
            else
            {
                foreach (var player in PhotonNetwork.PlayerListOthers)
                {
                    firstPlayer = player;
                }
                preemptiveCheck.text = "후공";
            }

            isPreemptivePlayerSet = true;

            if (PhotonNetwork.IsMasterClient)
                StartArrayPhase();
        }
    }
    #endregion
}
