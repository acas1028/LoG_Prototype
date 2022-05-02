using Photon.Pun;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MatchResultManager : MonoBehaviourPunCallbacks
{
    [Header("Match Result Panel")]
    public GameObject matchResultPanel;

    bool? isPVE;
    bool? isMatchOver;

    public void ShowMatchResult(bool isWin, bool isPVE, bool isMatchOver = false)
    {
        GameObject result = Instantiate(matchResultPanel, GameObject.Find("Canvas").transform);
        this.isPVE = isPVE;
        this.isMatchOver = isMatchOver;

        if (!isPVE) { // PVP일때
            if (isWin) {
                Debug.Log("Win");
                result.GetComponent<MatchReward>().LoseTitle.SetActive(false);
                result.GetComponent<MatchReward>().RewardValue.text = "100 credit";

                var request = new AddUserVirtualCurrencyRequest() { VirtualCurrency = "CO", Amount = 100 };
                PlayFabClientAPI.AddUserVirtualCurrency(request,
                    (result) => {
                    },
                    (error) => Debug.Log("코인 획득 실패"));
            }

            else if (!isWin) {
                Debug.Log("Lose");
                result.GetComponent<MatchReward>().WinTitle.SetActive(false);
                result.GetComponent<MatchReward>().RewardValue.text = "50 credit";

                var request = new AddUserVirtualCurrencyRequest() { VirtualCurrency = "CO", Amount = 50 };
                PlayFabClientAPI.AddUserVirtualCurrency(request,
                    (result) => {
                    },
                    (error) => Debug.Log("코인 획득 실패"));
            }
        }
        else {
            if(isWin)
            {
                Debug.Log("Pve_VIctory");
                PveDataSync.instance.SetData(CSVManager.Instance.StageNumber);
                PveDataSync.instance.SendClearStage(CSVManager.Instance.StageNumber);
            }
            // PVE일때 보상
        }

        Invoke("LeaveRoom", 5f);
    }

    public void LeaveRoom()
    {
        if (!isPVE.HasValue) {
            object o_isPVE;
            isPVE = PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("IsPVE", out o_isPVE);
        }

        ExitGames.Client.Photon.Hashtable table = new ExitGames.Client.Photon.Hashtable { { "IsPreemptive", null }, { "RoundWinCount", null } };
        PhotonNetwork.SetPlayerCustomProperties(table);
        PhotonNetwork.LeaveRoom();
    }

    #region 포톤 콜백 함수
    public override void OnLeftRoom() {
        Debug.Log("<color=yellow>OnLeftRoom() 호출\n룸을 나갑니다. 로비로 이동합니다.</color>");

        var currentScene = (Move_Scene.ENUM_SCENE)SceneManager.GetActiveScene().buildIndex;
        if (currentScene == Move_Scene.ENUM_SCENE.BATTLE_SCENE) {
            if (isPVE.Value)
                PhotonNetwork.LoadLevel((int)Move_Scene.ENUM_SCENE.PVE_CSVTESTSCENE2);
            else {
                if (!isMatchOver.HasValue) {
                    Debug.LogError("매치가 끝났는지 여부를 확인할 수 없습니다.");
                    return;
                }
                if (isMatchOver.Value)
                    PhotonNetwork.LoadLevel((int)Move_Scene.ENUM_SCENE.MAINLOBBY_SCENE);
                else
                    PhotonNetwork.LoadLevel((int)Move_Scene.ENUM_SCENE.ARRAYMENT_SCENE);
            }
        }
        else if (currentScene == Move_Scene.ENUM_SCENE.PVE_CSVTESTSCENE2)
            PhotonNetwork.LoadLevel((int)Move_Scene.ENUM_SCENE.PVE_SCENE);
        else if (currentScene == Move_Scene.ENUM_SCENE.ARRAYMENT_SCENE)
            PhotonNetwork.LoadLevel((int)Move_Scene.ENUM_SCENE.MAINLOBBY_SCENE);
    }
    #endregion
}
