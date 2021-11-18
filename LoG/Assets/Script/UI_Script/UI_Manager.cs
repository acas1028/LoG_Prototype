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
        if (matchResultPanel != null)
        {
            matchResultText = matchResultPanel.GetComponentInChildren<Text>();
        }
    }

    public void ShowMatchResult(bool isWin)
    {
        Instantiate(matchResultPanel, GameObject.Find("Canvas").transform);
        matchResultText.text = isWin ? "승리" : "패배";
        Invoke("LeaveRoom", 4.0f);
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
        Debug.Log("<color=yellow>OnLeftRoom() 호출\n룸을 나갑니다. 로비로 이동합니다.</color>");

        ExitGames.Client.Photon.Hashtable table = new ExitGames.Client.Photon.Hashtable { { "IsPreemptive", null }, { "RoundWinCount", null } };
        PhotonNetwork.SetPlayerCustomProperties(table);

        SceneManager.LoadScene("LobbyScene");
    }
}
