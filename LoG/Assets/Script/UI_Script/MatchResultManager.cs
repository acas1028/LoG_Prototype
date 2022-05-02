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

        if (!isPVE) { // PVP�϶�
            if (isWin) {
                Debug.Log("Win");
                result.GetComponent<MatchReward>().LoseTitle.SetActive(false);
                result.GetComponent<MatchReward>().RewardValue.text = "100 credit";

                var request = new AddUserVirtualCurrencyRequest() { VirtualCurrency = "CO", Amount = 100 };
                PlayFabClientAPI.AddUserVirtualCurrency(request,
                    (result) => {
                    },
                    (error) => Debug.Log("���� ȹ�� ����"));
            }

            else if (!isWin) {
                Debug.Log("Lose");
                result.GetComponent<MatchReward>().WinTitle.SetActive(false);
                result.GetComponent<MatchReward>().RewardValue.text = "50 credit";

                var request = new AddUserVirtualCurrencyRequest() { VirtualCurrency = "CO", Amount = 50 };
                PlayFabClientAPI.AddUserVirtualCurrency(request,
                    (result) => {
                    },
                    (error) => Debug.Log("���� ȹ�� ����"));
            }
        }
        else {
            if(isWin)
            {
                Debug.Log("Pve_VIctory");
                PveDataSync.instance.SetData(CSVManager.Instance.StageNumber);
                PveDataSync.instance.SendClearStage(CSVManager.Instance.StageNumber);
            }
            // PVE�϶� ����
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

    #region ���� �ݹ� �Լ�
    public override void OnLeftRoom() {
        Debug.Log("<color=yellow>OnLeftRoom() ȣ��\n���� �����ϴ�. �κ�� �̵��մϴ�.</color>");

        var currentScene = (Move_Scene.ENUM_SCENE)SceneManager.GetActiveScene().buildIndex;
        if (currentScene == Move_Scene.ENUM_SCENE.BATTLE_SCENE) {
            if (isPVE.Value)
                PhotonNetwork.LoadLevel((int)Move_Scene.ENUM_SCENE.PVE_CSVTESTSCENE2);
            else {
                if (!isMatchOver.HasValue) {
                    Debug.LogError("��ġ�� �������� ���θ� Ȯ���� �� �����ϴ�.");
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
