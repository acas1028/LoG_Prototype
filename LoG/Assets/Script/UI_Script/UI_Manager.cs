using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;

public class UI_Manager : MonoBehaviourPunCallbacks
{
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        Debug.Log("<color=yellow>OnLeftRoom() ȣ��\n���� �����ϴ�. �κ�� �̵��մϴ�.</color>");

        SceneManager.LoadScene("LobbyScene");
    }
}
