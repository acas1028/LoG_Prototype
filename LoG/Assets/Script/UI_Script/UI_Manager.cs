using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using CharacterStats;

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

            var request = new AddUserVirtualCurrencyRequest() { VirtualCurrency = "CO", Amount = 100 };
            PlayFabClientAPI.AddUserVirtualCurrency(request,
                (result) => {
                },
                (error) => Debug.Log("ƒ⁄¿Œ »πµÊ Ω«∆–"));
        }

        else if(!isWin)
        {
            Debug.Log("Lose");
            result.GetComponent<MatchReward>().WinTitle.SetActive(false);
            result.GetComponent<MatchReward>().RewardValue.text = "50 credit";

            var request = new AddUserVirtualCurrencyRequest() { VirtualCurrency = "CO", Amount = 50 };
            PlayFabClientAPI.AddUserVirtualCurrency(request,
                (result) => {
                },
                (error) => Debug.Log("ƒ⁄¿Œ »πµÊ Ω«∆–"));
        }
        Invoke("LeaveRoom", 6.0f);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        Debug.Log("<color=yellow>OnLeftRoom() »£√‚\n∑Î¿ª ≥™∞©¥œ¥Ÿ. ∑Œ∫Ò∑Œ ¿Ãµø«’¥œ¥Ÿ.</color>");

        ExitGames.Client.Photon.Hashtable table = new ExitGames.Client.Photon.Hashtable { { "IsPreemptive", null }, { "RoundWinCount", null }, { "Stack_Survivor", null } };
        PhotonNetwork.SetPlayerCustomProperties(table);
        PhotonNetwork.Disconnect();

        SceneManager.LoadScene("MainLobbyScene");
    }
}
