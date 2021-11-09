
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
    public Text enemyPlayerName;
    public Text winStateText;
    public Text roomStatusText;
    public Text isEnemyJoinedText;
    public Text timeText;
    public Button readyButton;

    Player firstPlayer;
    Player secondPlayer;
    bool isPreemptivePlayerSet;

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

        playerName.text = PhotonNetwork.LocalPlayer.NickName;
        isEnemyJoinedText.text = "����� ������ ��ٸ��� ���Դϴ�...";
        preemptiveCheck.text = " ";

        firstPlayer = PhotonNetwork.LocalPlayer;
        secondPlayer = PhotonNetwork.LocalPlayer;
        isPreemptivePlayerSet = false;

        arrayPhase = (int)ArrayPhase.STANDBY;

        RenewEnemyPlayer();

        if (PhotonNetwork.OfflineMode)
        {
            roomStatusText.text = "<�������� ���>";
            preemptiveCheck.text = "����";
            StartArrayPhase();
        }
        else if (!PhotonNetwork.IsConnected)
            roomStatusText.text = "�α����� �ʿ��մϴ�";
        else
            roomStatusText.text = " ";

        result = GetCustomProperties();
        if (!result)
        {
            result = InitCustomProperties();
            if (!result)
            {
                Debug.LogError("CustomProperties �ʱ�ȭ ����");
            }
        }

        winStateText.text = winCount + " : " + enemyWinCount;

        if (PhotonNetwork.IsMasterClient && IsAllPlayersJoined())
            SetPreemptivePlayer();
    }

    private bool InitCustomProperties()
    {
        bool result = false;

        Hashtable table = new Hashtable() { { "IsPreemptive", true }, { "RoundWinCount", 0 } };
        result = PhotonNetwork.SetPlayerCustomProperties(table);
        if (!result)
        {
            Debug.LogError("PlayerCustomProperties ����ȭ ����");
            return false;
        }

        if (PhotonNetwork.IsMasterClient)
        {
            table = new Hashtable() { { "RoundCount", 0 } };
            result = PhotonNetwork.CurrentRoom.SetCustomProperties(table);
            if (!result)
            {
                Debug.LogError("RoundCount ����ȭ ����");
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
            Debug.LogWarning("isPreemptive ���� �������µ� �����߽��ϴ�. ���� ���� �Ҵ��մϴ�.");
            return false;
        }
        isPreemptive = (bool)o_isPreemptive;

        object o_winCount;
        PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("RoundWinCount", out o_winCount);
        if (o_winCount == null)
        {
            Debug.LogWarning("RoundWinCount ���� �������µ� �����߽��ϴ�. ���� ���� �Ҵ��մϴ�.");
            return false;
        }
        winCount = (int)o_winCount;

        object o_enemyWinCount;
        foreach (Player player in PhotonNetwork.PlayerListOthers)
        {
            player.CustomProperties.TryGetValue("RoundWinCount", out o_enemyWinCount);
            if (o_enemyWinCount == null)
            {
                Debug.LogWarning("RoundWinCount ���� �������µ� �����߽��ϴ�. ���� ���� �Ҵ��մϴ�.");
                return false;
            }
            enemyWinCount = (int)o_enemyWinCount;
        }

        object o_roundCount;
        PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("RoundCount", out o_roundCount);
        if (o_roundCount == null)
        {
            Debug.LogWarning("RoundCount ���� �������µ� �����߽��ϴ�. ���� ���� �Ҵ��մϴ�.");
            return false;
        }
        roundCount = (int)o_roundCount;

        return true;
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
        return PhotonNetwork.CurrentRoom.PlayerCount >= 2;
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

        if (roundCount == 1)
            _isPreemptive = !isPreemptive;
        else
        {
            Debug.Log("���� ���尡 2 �̻��̹Ƿ� �������� ������ ������մϴ�.");
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
        if (isEnemyJoinedText.gameObject.activeSelf)
            isEnemyJoinedText.gameObject.SetActive(false);
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
            roomStatusText.text = "#" + (arrayPhase + 1) + " ���� " + firstPlayer.NickName + ", " + (arrayPhase == (int)ArrayPhase.FIRST1 ? 1 : 2) + "���� ĳ���͸� ��ġ�Ͻʽÿ�.";
            if (IsPlayerPreemptive())
                readyButton.gameObject.SetActive(true);
            else
                readyButton.gameObject.SetActive(false);
        }
        else if (arrayPhase % 2 == 1)
        {
            roomStatusText.text = "#" + (arrayPhase + 1) + " �İ� " + secondPlayer.NickName + ", " + (arrayPhase == (int)ArrayPhase.SECOND5 ? 1 : 2) + "���� ĳ���͸� ��ġ�Ͻʽÿ�.";
            if (IsPlayerPreemptive())
                readyButton.gameObject.SetActive(false);
            else
                readyButton.gameObject.SetActive(true);
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
            SetPreemptivePlayer();
    }

    public override void OnPlayerLeftRoom(Player other)
    {
        roomStatusText.text = "��� �÷��̾� " + other.NickName + " ����";
        Debug.Log("<color=yellow>�÷��̾� " + other.NickName + " ����</color>");
        RenewEnemyPlayer();
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        // ��, �İ� ������ ���� �������̸鼭 '��'�� ��, �İ� �������� ��쿡�� �Ʒ� ������ ��ģ��. ���� �� �÷��̾ �濡 ���� �� ó�� �� ���� �� ������ ��ģ��.
        if (targetPlayer == PhotonNetwork.LocalPlayer)
        {
            if (changedProps.ContainsKey("IsPreemptive")
                && !PhotonNetwork.OfflineMode
                && IsAllPlayersJoined()
                && !isPreemptivePlayerSet)
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
                        preemptiveCheck.text = "����";
                    }
                    else
                    {
                        firstPlayer = player;
                        preemptiveCheck.text = "�İ�";
                    }
                }

                isPreemptivePlayerSet = true;
                roomStatusText.text = "��, �İ��� �����մϴ�...";

                if (PhotonNetwork.IsMasterClient)
                {
                    Invoke("StartArrayPhase", 1.0f);
                }
            }
        }
    }
    #endregion
}
