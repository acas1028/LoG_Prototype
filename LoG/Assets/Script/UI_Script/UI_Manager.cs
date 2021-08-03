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
        Debug.Log("<color=yellow>OnLeftRoom() 호출\n룸을 나갑니다. 로비로 이동합니다.</color>");

        SceneManager.LoadScene("LobbyScene");
    }
}
