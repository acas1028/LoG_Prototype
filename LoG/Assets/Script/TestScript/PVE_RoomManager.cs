
using UnityEngine;
using UnityEngine.UI;

using Photon.Realtime;
using Photon.Pun;
using ExitGames.Client.Photon;

public class PVE_RoomManager : MonoBehaviour
{
    private UI_Manager uiManager;

    public Text preemptiveCheck;
    public Text playerName;
    public Text enemyPlayerName;
    public Text winStateText;
    public Text roomStatusText;
    public Text timeText;
    public Button readyButton;

    Player firstPlayer;
    Player secondPlayer;

    private bool isPreemptive;
    public bool IsPreemptive
    {
        get { return isPreemptive; }
    }

    private void Start()
    {
        bool result = false;

        uiManager = FindObjectOfType<UI_Manager>();
        playerName.text = PhotonNetwork.LocalPlayer.NickName;
        preemptiveCheck.text = " ";

        firstPlayer = PhotonNetwork.LocalPlayer;
        secondPlayer = PhotonNetwork.LocalPlayer;

        RenewEnemyPlayer();

        if (PhotonNetwork.OfflineMode)
        {
            roomStatusText.text = "<오프라인 모드>";
            preemptiveCheck.text = "선공";

            Hashtable table = new Hashtable() { { "IsPreemptive", true } };
            PhotonNetwork.SetPlayerCustomProperties(table);

        }
        else if (!PhotonNetwork.IsConnected)
            roomStatusText.text = "로그인이 필요합니다";
        else if (IsAllPlayersJoined())
            roomStatusText.text = "잠시 후 게임이 시작됩니다.";
        else
            roomStatusText.text = "상대의 입장을 기다리는 중입니다...";

        result = GetCustomProperties();
        if (!result)
        {
            result = InitCustomProperties();
            if (!result)
            {
                Debug.LogError("CustomProperties 초기화 실패");
            }
        }

        if (PhotonNetwork.IsMasterClient && IsAllPlayersJoined())
            ;
    }

    private bool InitCustomProperties()
    {
        bool result = false;

        Hashtable table = new Hashtable() { { "RoundWinCount", 0 } };
        result = PhotonNetwork.SetPlayerCustomProperties(table);
        if (!result)
        {
            Debug.LogError("PlayerCustomProperties 동기화 실패");
            return false;
        }

        if (PhotonNetwork.IsMasterClient)
        {
            table = new Hashtable() { { "RoundCount", 1 } };
            result = PhotonNetwork.CurrentRoom.SetCustomProperties(table);
            if (!result)
            {
                Debug.LogError("RoundCount 동기화 실패");
                return false;
            }
        }

        return true;
    }

    private bool GetCustomProperties()
    {
        object o_isPreemptive;
        PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("IsPreemptive", out o_isPreemptive);
        if (o_isPreemptive == null)
        {
            Debug.LogWarning("isPreemptive 값을 가져오지 못했습니다. 값을 새로 할당합니다.");
            return false;
        }
        isPreemptive = (bool)o_isPreemptive;
        return true;
    }

    #region 외부에서 호출되는 public 함수
    public bool IsPlayerPreemptive()
    {
        return firstPlayer == PhotonNetwork.LocalPlayer;
    }

    public bool IsAllPlayersJoined()
    {
        return PhotonNetwork.CurrentRoom.PlayerCount >= 2;
    }

    #endregion

    private void RenewEnemyPlayer()
    {
        enemyPlayerName.text = " ";

        foreach (Player player in PhotonNetwork.PlayerListOthers)
            enemyPlayerName.text = player.NickName;
    }

    public void GetReady()
    {
        PhotonNetwork.LoadLevel("BattleScene");
    }

    #region 포톤 콜백 함수
    //public override void OnPlayerEnteredRoom(Player other)
    //{
    //    roomStatusText.text = "상대 플레이어 " + other.NickName + " 입장";
    //    Debug.Log("<color=yellow>플레이어 " + other.NickName + " 입장</color>");
    //    RenewEnemyPlayer();

    //    if (PhotonNetwork.IsMasterClient && IsAllPlayersJoined())
    //    {
    //        roomStatusText.text = "잠시 후 게임이 시작됩니다.";
    //        Invoke("SetPreemptivePlayer", 4.0f);
    //    }
    //}

    //public override void OnPlayerLeftRoom(Player other)
    //{
    //    roomStatusText.text = "상대 플레이어 " + other.NickName + " 퇴장";
    //    Debug.Log("<color=yellow>플레이어 " + other.NickName + " 퇴장</color>");
    //    RenewEnemyPlayer();

    //    timeText.gameObject.SetActive(false);
    //    uiManager.ShowMatchResult(true);
    //}

    //public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    //{
    //    // 선, 후공 결정에 관한 데이터이면서 '나'의 선, 후공 데이터인 경우에만 아래 과정을 거친다. 또한 두 플레이어가 방에 입장 후 처음 한 번만 이 과정을 거친다.
    //    if (targetPlayer == PhotonNetwork.LocalPlayer)
    //    {
    //        if (changedProps.ContainsKey("IsPreemptive")
    //            && !PhotonNetwork.OfflineMode
    //            && IsAllPlayersJoined())
    //        {
    //            Debug.LogFormat("Player <color=lightblue>#{0} {1}</color> Properties Updated due to <color=green>{2}</color>", targetPlayer.ActorNumber, targetPlayer.NickName, changedProps.ToString());

    //            object o_is_preemptive;
    //            bool is_preemptive;
    //            targetPlayer.CustomProperties.TryGetValue("IsPreemptive", out o_is_preemptive);
    //            is_preemptive = (bool)o_is_preemptive;

    //            foreach (var player in PhotonNetwork.PlayerListOthers)
    //            {
    //                if (is_preemptive)
    //                {
    //                    secondPlayer = player;
    //                    preemptiveCheck.text = "선공";
    //                }
    //                else
    //                {
    //                    firstPlayer = player;
    //                    preemptiveCheck.text = "후공";
    //                }
    //            }

    //            if (PhotonNetwork.IsMasterClient)
    //            {
    //                Invoke("StartArrayPhase", 1.0f);
    //            }
    //        }
    //    }
    //}
    #endregion
}
