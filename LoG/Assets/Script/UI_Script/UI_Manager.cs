using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using Photon.Pun;

public class UI_Manager : MonoBehaviourPunCallbacks
{
    Text matchResultText;

    [Header("Match Result Panel")]
    public GameObject matchResultPanel;

    private void Start()
    {
        matchResultText = matchResultPanel.GetComponentInChildren<Text>();
    }

    public void ShowMatchResult(bool isWin)
    {
        Instantiate(matchResultPanel, GameObject.Find("Canvas").transform);
        matchResultText.text = isWin ? "�¸�" : "�й�";
        Invoke("LeaveRoom", 3.0f);
    }

    public void BackToLobby()
    {
        SceneManager.LoadScene("LobbyScene");
    }

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
