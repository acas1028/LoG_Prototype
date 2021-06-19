using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Photon.Realtime;
using Photon.Pun;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public Text playerName;
    public Text roomStatusText;
    public Text isEnemyReadyText;
    public Button readyButton;

    private void Start()
    {
        playerName.text = PhotonNetwork.LocalPlayer.NickName;
        roomStatusText.text = " ";
        isEnemyReadyText.text = " ";
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
            isEnemyReadyText.text = "상대 준비 완료";
        else
            isEnemyReadyText.text = "상대 준비 미완료";
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    #region 포톤 콜백 함수
    public override void OnPlayerEnteredRoom(Player other)
    {
        roomStatusText.text = "상대 플레이어 " + other.NickName + " 입장";
        Debug.Log("플레이어 " + other.NickName + " 입장");
    }

    public override void OnPlayerLeftRoom(Player other)
    {
        roomStatusText.text = "상대 플레이어 " + other.NickName + " 퇴장";
        Debug.Log("플레이어 " + other.NickName + " 퇴장");
    }

    public override void OnLeftRoom()
    {
        Debug.Log("룸을 나갑니다. 로비로 이동합니다.");
        SceneManager.LoadScene("LobbyScene");
    }
    #endregion
}
