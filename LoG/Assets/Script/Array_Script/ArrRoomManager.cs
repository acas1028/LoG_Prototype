
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
    private UI_Manager uiManager;
    public Arrayment_Manager arraymentManager;

    [SerializeField]
    private int arrayPhase;

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
    private int winCount;
    public int WinCount
    {
        get { return winCount; }
    }
    private int enemyWinCount;
    public int EnemyWinCount
    {
        get { return enemyWinCount; }
    }
    private int roundCount;
    public int RoundCount
    {
        get { return roundCount; }
    }

    private void Start()
    {
        bool result = false;

        uiManager = FindObjectOfType<UI_Manager>();
        playerName.text = PhotonNetwork.LocalPlayer.NickName;
        preemptiveCheck.text = " ";

        firstPlayer = PhotonNetwork.LocalPlayer;
        secondPlayer = PhotonNetwork.LocalPlayer;

        arrayPhase = (int)ArrayPhase.STANDBY;

        RenewEnemyPlayer();

        if (PhotonNetwork.OfflineMode)
        {
            roomStatusText.text = "<오프라인 모드>";
            preemptiveCheck.text = "선공";

            Hashtable table = new Hashtable() { { "IsPreemptive", true } };
            PhotonNetwork.SetPlayerCustomProperties(table);

            StartArrayPhase();
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

        winStateText.text = winCount + " : " + enemyWinCount;

        if (PhotonNetwork.IsMasterClient && IsAllPlayersJoined())
            SetPreemptivePlayer();
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

            table = new Hashtable() { { "IsPVE", false } };
            result = PhotonNetwork.CurrentRoom.SetCustomProperties(table);
            if (!result) {
                Debug.LogError("IsPVE 동기화 실패");
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

        object o_winCount;
        PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("RoundWinCount", out o_winCount);
        if (o_winCount == null)
        {
            Debug.LogWarning("RoundWinCount 값을 가져오지 못했습니다. 값을 새로 할당합니다.");
            return false;
        }
        winCount = (int)o_winCount;

        object o_enemyWinCount;
        foreach (Player player in PhotonNetwork.PlayerListOthers)
        {
            player.CustomProperties.TryGetValue("RoundWinCount", out o_enemyWinCount);
            if (o_enemyWinCount == null)
            {
                Debug.LogWarning("적 RoundWinCount 값을 가져오지 못했습니다. 값을 새로 할당합니다.");
                return false;
            }
            enemyWinCount = (int)o_enemyWinCount;
        }

        object o_roundCount;
        PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("RoundCount", out o_roundCount);
        if (o_roundCount == null)
        {
            Debug.LogWarning("RoundCount 값을 가져오지 못했습니다. 값을 새로 할당합니다.");
            return false;
        }
        roundCount = (int)o_roundCount;

        return true;
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

    public void SetPreemptivePlayer()
    {
        bool result = false;
        result = IsAllPlayersJoined();
        if (!result)
        {
            Debug.LogError("플레이어 한 명이 나간 상태입니다. 선/후공 결정을 취소합니다. 게임을 진행할 수 없습니다.");
            return;
        }

        bool _isPreemptive = false;

        if (roundCount == 2)
            _isPreemptive = !isPreemptive;
        else
        {
            Debug.Log("현재 라운드가 첫 라운드 혹은 3 이상이므로 무작위로 선공을 재결정합니다.");
            _isPreemptive = Random.Range(0, 2) == 0 ? false : true;
        }

        Hashtable player_1_table = new Hashtable() { { "IsPreemptive", _isPreemptive } };
        Hashtable player_2_table = new Hashtable() { { "IsPreemptive", !_isPreemptive } };

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
        if (timeText.gameObject.activeSelf)
            timeText.gameObject.SetActive(false);

        timeText.gameObject.SetActive(true);

        arrayPhase++;
        if (arrayPhase == (int)ArrayPhase.END)
        {
            roomStatusText.text = "모든 배치가 완료되었습니다.";
            readyButton.gameObject.SetActive(false);
            PhotonNetwork.LoadLevel("BattleScene");
        }
        else if (arrayPhase % 2 == 0)
        {
            roomStatusText.text = "<color=#FFE439>" + firstPlayer.NickName + "</color>, " + (arrayPhase == (int)ArrayPhase.FIRST1 ? 1 : 2) + "개의 캐릭터를 배치하십시오.";
            if (IsPlayerPreemptive())
            {
                arraymentManager.InventoryUnblock();
                readyButton.gameObject.SetActive(true);
            }
            else
            {
                arraymentManager.InventoryBlock();
                readyButton.gameObject.SetActive(false);
            }
        }
        else if (arrayPhase % 2 == 1)
        {
            roomStatusText.text = "<color=#FFE439>" + secondPlayer.NickName + "</color>, " + (arrayPhase == (int)ArrayPhase.SECOND5 ? 1 : 2) + "개의 캐릭터를 배치하십시오.";
            if (IsPlayerPreemptive())
            {
                arraymentManager.InventoryBlock();
                readyButton.gameObject.SetActive(false);
            }
            else
            {
                arraymentManager.InventoryUnblock();
                readyButton.gameObject.SetActive(true);
            }
        }

        Debug.Log("<color=yellow>RPC Success with ArrayPhase: </color><color=lightblue>" + (ArrayPhase)arrayPhase + "</color>");
    }

    private void RenewEnemyPlayer()
    {
        enemyPlayerName.text = " ";

        foreach (Player player in PhotonNetwork.PlayerListOthers)
            enemyPlayerName.text = player.NickName;
    }

    #region 포톤 콜백 함수
    public override void OnPlayerEnteredRoom(Player other)
    {
        roomStatusText.text = "상대 플레이어 " + other.NickName + " 입장";
        Debug.Log("<color=yellow>플레이어 " + other.NickName + " 입장</color>");
        RenewEnemyPlayer();

        if (PhotonNetwork.IsMasterClient && IsAllPlayersJoined())
        {
            roomStatusText.text = "잠시 후 게임이 시작됩니다.";
            Invoke("SetPreemptivePlayer", 4.0f);
        }
    }

    public override void OnPlayerLeftRoom(Player other)
    {
        roomStatusText.text = "상대 플레이어 " + other.NickName + " 퇴장";
        Debug.Log("<color=yellow>플레이어 " + other.NickName + " 퇴장</color>");
        RenewEnemyPlayer();

        timeText.gameObject.SetActive(false);
        uiManager.ShowMatchResult(true);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        // 선, 후공 결정에 관한 데이터이면서 '나'의 선, 후공 데이터인 경우에만 아래 과정을 거친다. 또한 두 플레이어가 방에 입장 후 처음 한 번만 이 과정을 거친다.
        if (targetPlayer == PhotonNetwork.LocalPlayer)
        {
            if (changedProps.ContainsKey("IsPreemptive")
                && !PhotonNetwork.OfflineMode
                && IsAllPlayersJoined())
            {
                Debug.LogFormat("Player <color=lightblue>#{0} {1}</color> Properties Updated due to <color=green>{2}</color>", targetPlayer.ActorNumber, targetPlayer.NickName, changedProps.ToString());

                object o_is_preemptive;
                bool is_preemptive;
                targetPlayer.CustomProperties.TryGetValue("IsPreemptive", out o_is_preemptive);
                is_preemptive = (bool)o_is_preemptive;

                foreach (var player in PhotonNetwork.PlayerListOthers)
                {
                    if (is_preemptive)
                    {
                        secondPlayer = player;
                        preemptiveCheck.text = "선공";
                    }
                    else
                    {
                        firstPlayer = player;
                        preemptiveCheck.text = "후공";
                    }
                }

                if (PhotonNetwork.IsMasterClient)
                {
                    Invoke("StartArrayPhase", 1.0f);
                }
            }
        }
    }
    #endregion
}
