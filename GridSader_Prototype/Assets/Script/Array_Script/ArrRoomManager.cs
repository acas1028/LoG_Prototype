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
            roomStatusText.text = "<�������� ���>";
        else if (!PhotonNetwork.IsConnected)
            roomStatusText.text = "�α����� �ʿ��մϴ�";
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
        Player secondPlayer = PhotonNetwork.LocalPlayer; // �ʱ�ȭ

        foreach (var player in PhotonNetwork.PlayerListOthers)
        {
            secondPlayer = player; // LocalPlayer �� �����̹Ƿ� ������ ������ �÷��̾ secondPlayer �� �ȴ�.
        }

        arrayPhase = (int)ArrayPhase.FIRST1;
        roomStatusText.text = "#1 ���� " + PhotonNetwork.MasterClient.NickName + " 1���� ĳ���͸� ��ġ�Ͻʽÿ�.";
        
        yield return new WaitUntil(() => isArrCompleteButtonClicked);

        isArrCompleteButtonClicked = false;
        arrayPhase = (int)ArrayPhase.SECOND12;
        roomStatusText.text = "#2 �İ� " + secondPlayer.NickName + " 2���� ĳ���͸� ��ġ�Ͻʽÿ�.";

        yield return new WaitUntil(() => isArrCompleteButtonClicked);

        isArrCompleteButtonClicked = false;
        arrayPhase = (int)ArrayPhase.FIRST23;
        roomStatusText.text = "#3 ���� " + PhotonNetwork.MasterClient.NickName + " 2���� ĳ���͸� ��ġ�Ͻʽÿ�.";

        yield return new WaitUntil(() => isArrCompleteButtonClicked);

        isArrCompleteButtonClicked = false;
        arrayPhase = (int)ArrayPhase.SECOND34;
        roomStatusText.text = "#4 �İ� " + secondPlayer.NickName + " 2���� ĳ���͸� ��ġ�Ͻʽÿ�.";

        yield return new WaitUntil(() => isArrCompleteButtonClicked);

        isArrCompleteButtonClicked = false;
        arrayPhase = (int)ArrayPhase.FIRST45;
        roomStatusText.text = "#5 ���� " + PhotonNetwork.MasterClient.NickName + " 2���� ĳ���͸� ��ġ�Ͻʽÿ�.";

        yield return new WaitUntil(() => isArrCompleteButtonClicked);

        isArrCompleteButtonClicked = false;
        arrayPhase = (int)ArrayPhase.SECOND5;
        roomStatusText.text = "#6 �İ� " + secondPlayer.NickName + " 2���� ĳ���͸� ��ġ�Ͻʽÿ�.";

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

    public int GetArrayPhase()
    {
        return arrayPhase;
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
