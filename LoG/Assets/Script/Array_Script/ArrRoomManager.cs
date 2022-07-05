
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
    private MatchResultManager uiManager;
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

    int callbackCount;

    private void Start()
    {
        uiManager = FindObjectOfType<MatchResultManager>();
        playerName.text = PhotonNetwork.LocalPlayer.NickName;
        preemptiveCheck.text = " ";

        firstPlayer = PhotonNetwork.LocalPlayer;
        secondPlayer = PhotonNetwork.LocalPlayer;

        arrayPhase = (int)ArrayPhase.STANDBY;

        RenewEnemyPlayer();

        if (PhotonNetwork.OfflineMode)
        {
            roomStatusText.text = "<�������� ���>";
            preemptiveCheck.text = "����";

            Hashtable table = new Hashtable() { { "IsPreemptive", true } };
            PhotonNetwork.SetPlayerCustomProperties(table);

            StartArrayPhase();
        }
        else if (!PhotonNetwork.IsConnected)
            roomStatusText.text = "�α����� �ʿ��մϴ�";
        else if (IsAllPlayersJoined())
            roomStatusText.text = "��� �� ������ ���۵˴ϴ�.";
        else
            roomStatusText.text = "����� ������ ��ٸ��� ���Դϴ�...";

        GetCustomProperties();

        winStateText.text = winCount + " : " + enemyWinCount;

        if (PhotonNetwork.IsMasterClient && IsAllPlayersJoined())
            SetPreemptivePlayer();

        callbackCount = 0;
    }

    private void GetCustomProperties()
    {
        object o_isPreemptive;
        PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("IsPreemptive", out o_isPreemptive);
        if (o_isPreemptive == null)
        {
            Debug.LogWarning("isPreemptive ���� �������� ���߽��ϴ�. ���� ���� �Ҵ��մϴ�.");
        }
        else
            isPreemptive = (bool)o_isPreemptive;

        object o_winCount;
        PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("RoundWinCount", out o_winCount);
        if (o_winCount == null)
        {
            Debug.LogWarning("RoundWinCount ���� �������� ���߽��ϴ�. ���� ���� �Ҵ��մϴ�.");

            var table = new Hashtable() { { "RoundWinCount", 0 } };
            if (!PhotonNetwork.SetPlayerCustomProperties(table)) {
                Debug.LogError("PlayerCustomProperties ����ȭ ����");
            }
        }
        else
            winCount = (int)o_winCount;

        object o_enemyWinCount;
        foreach (Player player in PhotonNetwork.PlayerListOthers)
        {
            player.CustomProperties.TryGetValue("RoundWinCount", out o_enemyWinCount);
            if (o_enemyWinCount == null) {
                Debug.LogWarning("�� RoundWinCount ���� �������� ���߽��ϴ�.");
            }
            else
                enemyWinCount = (int)o_enemyWinCount;
        }

        object o_roundCount;
        PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("RoundCount", out o_roundCount);
        if (o_roundCount == null)
        {
            Debug.LogWarning("RoundCount ���� �������� ���߽��ϴ�. ���� ���� �Ҵ��մϴ�.");

            if (PhotonNetwork.IsMasterClient) {
                var table = new Hashtable() { { "RoundCount", 1 } };
                if (!PhotonNetwork.CurrentRoom.SetCustomProperties(table)) {
                    Debug.LogError("RoundCount ����ȭ ����");
                }
            }
        }
        else
            roundCount = (int)o_roundCount;

        object o_isPVE;
        PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("IsPVE", out o_isPVE);
        if (o_isPVE == null) {
            Debug.LogWarning("isPVE ���� �������� ���߽��ϴ�. ���� ���� �Ҵ��մϴ�.");

            if (PhotonNetwork.IsMasterClient) {
                var table = new Hashtable() { { "IsPVE", false } };
                if (!PhotonNetwork.CurrentRoom.SetCustomProperties(table)) {
                    Debug.LogError("IsPVE ����ȭ ����");
                }
            }
        }
    }

    #region �ܺο��� ȣ��Ǵ� public �Լ�
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
        if (PhotonNetwork.InRoom)
            return PhotonNetwork.CurrentRoom.PlayerCount >= 2;
        else
            return false;
    }

    public void SetPreemptivePlayer()
    {
        bool result = false;
        result = IsAllPlayersJoined();
        if (!result)
        {
            Debug.LogError("�÷��̾� �� ���� ���� �����Դϴ�. ��/�İ� ������ ����մϴ�. ������ ������ �� �����ϴ�.");
            return;
        }

        bool _isPreemptive = false;

        if (roundCount == 2)
            _isPreemptive = !isPreemptive;
        else
        {
            Debug.Log("���� ���尡 ù ���� Ȥ�� 3 �̻��̹Ƿ� �������� ������ ������մϴ�.");
            _isPreemptive = Random.Range(0, 2) == 0 ? false : true;
        }

        Hashtable player_1_table = new Hashtable() { { "IsPreemptive", _isPreemptive } };
        Hashtable player_2_table = new Hashtable() { { "IsPreemptive", !_isPreemptive } };

        result = PhotonNetwork.PlayerList[0].SetCustomProperties(player_1_table);
        if (!result)
            Debug.LogWarning("�÷��̾� 1 �������� ����ȭ ����");
        result = PhotonNetwork.PlayerList[1].SetCustomProperties(player_2_table);
        if (!result)
            Debug.LogWarning("�÷��̾� 2 �������� ����ȭ ����");
    }

    public void StartArrayPhase()
    {
        bool result = IsAllPlayersJoined();
        if (!result && !PhotonNetwork.OfflineMode)
        {
            Debug.LogError("�÷��̾� �� ���� ���� �����Դϴ�. ���� ������ ����մϴ�. ������ ������ �� �����ϴ�.");
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
            roomStatusText.text = "��� ��ġ�� �Ϸ�Ǿ����ϴ�.";
            readyButton.gameObject.SetActive(false);
            PhotonNetwork.LoadLevel("BattleScene");
        }
        else if (arrayPhase % 2 == 0)
        {
            roomStatusText.text = "<color=#FFE439>" + firstPlayer.NickName + "</color>, " + (arrayPhase == (int)ArrayPhase.FIRST1 ? 1 : 2) + "���� ĳ���͸� ��ġ�Ͻʽÿ�.";
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
            roomStatusText.text = "<color=#FFE439>" + secondPlayer.NickName + "</color>, " + (arrayPhase == (int)ArrayPhase.SECOND5 ? 1 : 2) + "���� ĳ���͸� ��ġ�Ͻʽÿ�.";
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

    #region ���� �ݹ� �Լ�
    public override void OnPlayerEnteredRoom(Player other)
    {
        roomStatusText.text = "��� �÷��̾� " + other.NickName + " ����";
        Debug.Log("<color=yellow>�÷��̾� " + other.NickName + " ����</color>");
        RenewEnemyPlayer();

        if (PhotonNetwork.IsMasterClient && IsAllPlayersJoined())
        {
            roomStatusText.text = "��� �� ������ ���۵˴ϴ�.";
            Invoke("SetPreemptivePlayer", 4.0f);
        }
    }

    public override void OnPlayerLeftRoom(Player other)
    {
        roomStatusText.text = "��� �÷��̾� " + other.NickName + " ����";
        Debug.Log("<color=yellow>�÷��̾� " + other.NickName + " ����</color>");
        RenewEnemyPlayer();

        timeText.gameObject.SetActive(false);
        uiManager.ShowMatchResult(isWin: true, isPVE: false, isMatchOver: true, onEnemyQuit: true);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        // ��, �İ� ������ ���� �������̸鼭 '��'�� ��, �İ� �������� ��쿡�� �Ʒ� ������ ��ģ��. ���� �� �÷��̾ �濡 ���� �� ó�� �� ���� �� ������ ��ģ��.
        if (changedProps.ContainsKey("IsPreemptive")
                && !PhotonNetwork.OfflineMode
                && IsAllPlayersJoined()) {
            callbackCount++;

            if (targetPlayer == PhotonNetwork.LocalPlayer) {
                Debug.LogFormat("Player <color=lightblue>#{0} {1}</color> Properties Updated due to <color=green>{2}</color>", targetPlayer.ActorNumber, targetPlayer.NickName, changedProps.ToString());

                object o_is_preemptive;
                bool is_preemptive;
                targetPlayer.CustomProperties.TryGetValue("IsPreemptive", out o_is_preemptive);
                is_preemptive = (bool)o_is_preemptive;

                foreach (var player in PhotonNetwork.PlayerListOthers) {
                    if (is_preemptive) {
                        secondPlayer = player;
                        preemptiveCheck.text = "����";
                    }
                    else {
                        firstPlayer = player;
                        preemptiveCheck.text = "�İ�";
                    }
                }
            }

            if (PhotonNetwork.IsMasterClient && callbackCount >= 2)
                Invoke("StartArrayPhase", 2f);
        }
    }
    #endregion
}
