using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using Photon.Pun;

public class UI_Manager : MonoBehaviourPunCallbacks
{
    [Header("Match Result Panel")]
    public GameObject matchResultPanel;

    public GameObject matchReward;

    public void ShowMatchResult(bool isWin)
    {
        
        Text matchResultText = Instantiate(matchResultPanel, GameObject.Find("Canvas").transform).GetComponentInChildren<Text>();
        Instantiate(matchReward, GameObject.Find("Canvas").transform);
        matchResultText.text = isWin ? "�¸�" : "�й�";

        if (isWin && !(PhotonNetwork.OfflineMode))
        {

            matchReward.GetComponent<MatchReward>().RewardValue.text = "100 Credit";
        }

        else if(!isWin && !(PhotonNetwork.OfflineMode))
        {
            matchReward.GetComponent<MatchReward>().RewardValue.text = "50 Credit";
        }
        Invoke("LeaveRoom", 4.0f);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        Debug.Log("<color=yellow>OnLeftRoom() ȣ��\n���� �����ϴ�. �κ�� �̵��մϴ�.</color>");

        ExitGames.Client.Photon.Hashtable table = new ExitGames.Client.Photon.Hashtable { { "IsPreemptive", null }, { "RoundWinCount", null }, { "Stack_Survivor", null } };
        PhotonNetwork.SetPlayerCustomProperties(table);
        PhotonNetwork.Disconnect();

        SceneManager.LoadScene("MainLobbyScene");
    }
}
