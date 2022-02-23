using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using Photon.Pun;

public class UI_Manager : MonoBehaviourPunCallbacks
{
    [Header("Match Result Panel")]
    public GameObject matchResultPanel;


    public void ShowMatchResult(bool isWin)
    {

        GameObject result = Instantiate(matchResultPanel, GameObject.Find("Canvas").transform);

        if (isWin)
        {
            Debug.Log("Win");
            result.GetComponent<MatchReward>().LoseTitle.SetActive(false);
            result.GetComponent<MatchReward>().RewardValue.text = "100 credit";
        }

        else if(!isWin)
        {
            Debug.Log("Lose");
            result.GetComponent<MatchReward>().WinTitle.SetActive(false);
            result.GetComponent<MatchReward>().RewardValue.text = "50 credit";
        }
        Invoke("LeaveRoom", 6.0f);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        Debug.Log("<color=yellow>OnLeftRoom() 호출\n룸을 나갑니다. 로비로 이동합니다.</color>");

        ExitGames.Client.Photon.Hashtable table = new ExitGames.Client.Photon.Hashtable { { "IsPreemptive", null }, { "RoundWinCount", null }, { "Stack_Survivor", null } };
        PhotonNetwork.SetPlayerCustomProperties(table);
        PhotonNetwork.Disconnect();

        SceneManager.LoadScene("MainLobbyScene");
    }
}
