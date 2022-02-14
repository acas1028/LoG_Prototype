
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
            roomStatusText.text = "<�������� ���>";
            preemptiveCheck.text = "����";

            Hashtable table = new Hashtable() { { "IsPreemptive", true } };
            PhotonNetwork.SetPlayerCustomProperties(table);

        }
        else if (!PhotonNetwork.IsConnected)
            roomStatusText.text = "�α����� �ʿ��մϴ�";
        else if (IsAllPlayersJoined())
            roomStatusText.text = "��� �� ������ ���۵˴ϴ�.";
        else
            roomStatusText.text = "����� ������ ��ٸ��� ���Դϴ�...";

        result = GetCustomProperties();
        if (!result)
        {
            result = InitCustomProperties();
            if (!result)
            {
                Debug.LogError("CustomProperties �ʱ�ȭ ����");
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
            Debug.LogError("PlayerCustomProperties ����ȭ ����");
            return false;
        }

        if (PhotonNetwork.IsMasterClient)
        {
            table = new Hashtable() { { "RoundCount", 1 } };
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
            Debug.LogWarning("isPreemptive ���� �������� ���߽��ϴ�. ���� ���� �Ҵ��մϴ�.");
            return false;
        }
        isPreemptive = (bool)o_isPreemptive;
        return true;
    }

    #region �ܺο��� ȣ��Ǵ� public �Լ�
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

    #region ���� �ݹ� �Լ�
    //public override void OnPlayerEnteredRoom(Player other)
    //{
    //    roomStatusText.text = "��� �÷��̾� " + other.NickName + " ����";
    //    Debug.Log("<color=yellow>�÷��̾� " + other.NickName + " ����</color>");
    //    RenewEnemyPlayer();

    //    if (PhotonNetwork.IsMasterClient && IsAllPlayersJoined())
    //    {
    //        roomStatusText.text = "��� �� ������ ���۵˴ϴ�.";
    //        Invoke("SetPreemptivePlayer", 4.0f);
    //    }
    //}

    //public override void OnPlayerLeftRoom(Player other)
    //{
    //    roomStatusText.text = "��� �÷��̾� " + other.NickName + " ����";
    //    Debug.Log("<color=yellow>�÷��̾� " + other.NickName + " ����</color>");
    //    RenewEnemyPlayer();

    //    timeText.gameObject.SetActive(false);
    //    uiManager.ShowMatchResult(true);
    //}

    //public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    //{
    //    // ��, �İ� ������ ���� �������̸鼭 '��'�� ��, �İ� �������� ��쿡�� �Ʒ� ������ ��ģ��. ���� �� �÷��̾ �濡 ���� �� ó�� �� ���� �� ������ ��ģ��.
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
    //                    preemptiveCheck.text = "����";
    //                }
    //                else
    //                {
    //                    firstPlayer = player;
    //                    preemptiveCheck.text = "�İ�";
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
