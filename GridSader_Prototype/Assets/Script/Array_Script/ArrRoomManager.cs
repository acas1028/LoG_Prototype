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
            roomStatusText.text = "<�������� ���>";
        else if (!PhotonNetwork.IsConnected)
            roomStatusText.text = "�α����� �ʿ��մϴ�";
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
            roomStatusText.text = "��� ��ġ�� �Ϸ�Ǿ����ϴ�.";
            arrCompleteButton.gameObject.SetActive(false);
            PhotonNetwork.LoadLevel("BattleScene");
        }
        else if (arrayPhase % 2 == 0)
        {
            roomStatusText.text = "#" + (arrayPhase + 1) + " ���� " + firstPlayer.NickName + ", " + (arrayPhase == (int)ArrayPhase.FIRST1 ? 1 : 2) + "���� ĳ���͸� ��ġ�Ͻʽÿ�.";
            if (PhotonNetwork.LocalPlayer == firstPlayer)
                arrCompleteButton.gameObject.SetActive(true);
            else if (PhotonNetwork.LocalPlayer == secondPlayer)
                arrCompleteButton.gameObject.SetActive(false);
        }
        else if (arrayPhase % 2 == 1)
        {
            roomStatusText.text = "#" + (arrayPhase + 1) + " �İ� " + secondPlayer.NickName + ", " + (arrayPhase == (int)ArrayPhase.SECOND5 ? 1 : 2) + "���� ĳ���͸� ��ġ�Ͻʽÿ�.";
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

        joinedPlayerList.text = "�뿡 �ִ� �÷��̾� <color=#912900>" + playerList + "</color>";
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
    #endregion
}
