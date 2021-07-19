
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
            roomStatusText.text = "<�������� ���>";
        else if (!PhotonNetwork.IsConnected)
            roomStatusText.text = "�α����� �ʿ��մϴ�";
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

    public void SetReadyButtonStatus(bool isReady)
    {
        ColorBlock cb = readyButton.colors;
        cb.normalColor = new Color(isReady ? 1.0f : 0.0f, 0.0f, 0.0f);
        cb.selectedColor = new Color(isReady ? 1.0f : 0.0f, 0.0f, 0.0f);
        readyButton.colors = cb;
        readyButton.GetComponentInChildren<Text>().text = isReady ? "�غ� �Ϸ�!" : "�غ�!";
    }

    public void SetIsEnemyReadyText(bool isEnemyReady)
    {
        if (isEnemyReady)
            isEnemyReadyText.text = "��� �غ� �Ϸ�";
        else
            isEnemyReadyText.text = "��� �غ� �̿Ϸ�";
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void SetPreemptivePlayer()
    {
        Debug.Log("SetPreemptivePlayer() ȣ��");

        bool result = false;
        result = IsAllPlayersJoined();
        if (!result)
        {
            Debug.LogError("�÷��̾� �� ���� ���� �����Դϴ�. ��/�İ� ������ ����մϴ�. ������ ������ �� �����ϴ�.");
            return;
        }

        bool isPreemptive = Random.Range(0, 2) == 0 ? false : true;
        Hashtable player_1_table = new Hashtable() { { "IsPreemptive", isPreemptive } };
        Hashtable player_2_table = new Hashtable() { { "IsPreemptive", !isPreemptive } };

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
            roomStatusText.text = "��� ��ġ�� �Ϸ�Ǿ����ϴ�.";
            arrCompleteButton.gameObject.SetActive(false);
            PhotonNetwork.LoadLevel("BattleScene");
        }
        else if (arrayPhase % 2 == 0)
        {
            roomStatusText.text = "#" + (arrayPhase + 1) + " ���� " + firstPlayer.NickName + ", " + (arrayPhase == (int)ArrayPhase.FIRST1 ? 1 : 2) + "���� ĳ���͸� ��ġ�Ͻʽÿ�.";
            if (IsPlayerPreemptive())
                arrCompleteButton.gameObject.SetActive(true);
            else
                arrCompleteButton.gameObject.SetActive(false);
        }
        else if (arrayPhase % 2 == 1)
        {
            roomStatusText.text = "#" + (arrayPhase + 1) + " �İ� " + secondPlayer.NickName + ", " + (arrayPhase == (int)ArrayPhase.SECOND5 ? 1 : 2) + "���� ĳ���͸� ��ġ�Ͻʽÿ�.";
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

        joinedPlayerList.text = "�뿡 �ִ� �÷��̾� <color=#912900>" + playerList + "</color>";
    }

    #region ���� �ݹ� �Լ�
    public override void OnPlayerEnteredRoom(Player other)
    {
        roomStatusText.text = "��� �÷��̾� " + other.NickName + " ����";
        Debug.Log("<color=yellow>�÷��̾� " + other.NickName + " ����</color>");
        RenewPlayerList();
    }

    public override void OnPlayerLeftRoom(Player other)
    {
        roomStatusText.text = "��� �÷��̾� " + other.NickName + " ����";
        Debug.Log("<color=yellow>�÷��̾� " + other.NickName + " ����</color>");
        RenewPlayerList();
    }

    public override void OnLeftRoom()
    {
        Debug.Log("<color=yellow>OnLeftRoom() ȣ��\n���� �����ϴ�. �κ�� �̵��մϴ�.</color>");

        PhotonNetwork.LoadLevel("LobbyScene");
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        // ��, �İ� ������ ���� �������̸鼭 '��'�� ��, �İ� �������� ��쿡�� �Ʒ� ������ ��ģ��.
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
                preemptiveCheck.text = "����";
            }
            else
            {
                foreach (var player in PhotonNetwork.PlayerListOthers)
                {
                    firstPlayer = player;
                }
                preemptiveCheck.text = "�İ�";
            }

            isPreemptivePlayerSet = true;

            if (PhotonNetwork.IsMasterClient)
                StartArrayPhase();
        }
    }
    #endregion
}
