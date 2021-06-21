using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Photon.Realtime;
using Photon.Pun;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public Text playerName;
    public Text roomStatusText;
    public Text joinedPlayerList;
    public Text isEnemyReadyText;
    public Button readyButton;

    private void Start()
    {
        playerName.text = PhotonNetwork.LocalPlayer.NickName;
        if (PhotonNetwork.OfflineMode)
            roomStatusText.text = "<�������� ���>";
        else
            roomStatusText.text = " ";
        isEnemyReadyText.text = " ";

        RenewPlayerList();
    }

    private void RenewPlayerList()
    {
        string playerList = string.Empty;

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            playerList += System.Environment.NewLine + player.NickName;
        }

        joinedPlayerList.text = "�뿡 �ִ� �÷��̾�" + playerList;
    }

    public void SetReadyButtonColor(bool isReady)
    {
        ColorBlock cb = readyButton.colors;
        cb.normalColor = new Color(isReady ? 1.0f : 0.0f, 0.0f, 0.0f);
        cb.selectedColor = new Color(isReady ? 1.0f : 0.0f, 0.0f, 0.0f);
        readyButton.colors = cb;
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
        Debug.Log("�÷��̾� " + other.NickName + " ����");
        RenewPlayerList();
    }

    public override void OnPlayerLeftRoom(Player other)
    {
        roomStatusText.text = "��� �÷��̾� " + other.NickName + " ����";
        Debug.Log("�÷��̾� " + other.NickName + " ����");
        RenewPlayerList();
    }

    public override void OnLeftRoom()
    {
        Debug.Log("���� �����ϴ�. �κ�� �̵��մϴ�.");
        PhotonNetwork.LoadLevel("LobbyScene");
    }
    #endregion
}
