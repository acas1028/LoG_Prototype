using System.Collections.Generic;
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
    int currentStage;
    List<Dictionary<string, object>> reward_data;

    private void Start() {
        reward_data = CSVReader.Read("PVE_Reward/PVE_Rewards");
        currentStage = CSVManager.StageNumber;
    }

    public void ShowMatchResult(bool isWin, bool isPVE, bool isMatchOver, bool onEnemyQuit)
    {
        GameObject result = Instantiate(matchResultPanel, GameObject.Find("Canvas").transform);
        this.isPVE = isPVE;
        this.isMatchOver = isMatchOver;

        if (!isPVE) { // PVP�϶�
            if (isMatchOver) {
                if (isWin) {
                    Debug.Log("PVP Win");
                    result.GetComponent<MatchReward>().LoseTitle.SetActive(false);

                    if (!onEnemyQuit) {
                        result.GetComponent<MatchReward>().RewardValue.text = "100 credit";

                        var request = new AddUserVirtualCurrencyRequest() { VirtualCurrency = "CO", Amount = 100 };
                        PlayFabClientAPI.AddUserVirtualCurrency(request,
                            (result) => {
                                Debug.Log(result.BalanceChange + " ���� ȹ��");
                            },
                            (error) => Debug.Log("���� ȹ�� ����"));
                    }
                    else {
                        result.GetComponent<MatchReward>().RewardValue.gameObject.SetActive(false);
                    }
                }

                else if (!isWin) {
                    Debug.Log("PVP Lose");
                    result.GetComponent<MatchReward>().WinTitle.SetActive(false);

                    if (!onEnemyQuit) {
                        result.GetComponent<MatchReward>().RewardValue.text = "50 credit";

                        var request = new AddUserVirtualCurrencyRequest() { VirtualCurrency = "CO", Amount = 50 };
                        PlayFabClientAPI.AddUserVirtualCurrency(request,
                            (result) => {
                                Debug.Log(result.BalanceChange + " ���� ȹ��");
                            },
                            (error) => Debug.Log("���� ȹ�� ����"));
                    }
                    else {
                        result.GetComponent<MatchReward>().RewardValue.gameObject.SetActive(false);
                    }
                }
            }
        }
        else {
            if(isWin)
            {
                Debug.Log("Pve_Win");
                PveDataSync.instance.SetData(CSVManager.StageNumber);
                PveDataSync.instance.SendClearStage(CSVManager.StageNumber);

                result.GetComponent<MatchReward>().LoseTitle.SetActive(false);

                if ((string)reward_data[currentStage - 1]["Reward"] == "Gold") {
                    int rewardCredit = (int)reward_data[currentStage - 1]["Value"];

                    result.GetComponent<MatchReward>().RewardValue.text = rewardCredit.ToString() + " credit";

                    var request = new AddUserVirtualCurrencyRequest() { VirtualCurrency = "CO", Amount = rewardCredit };
                    PlayFabClientAPI.AddUserVirtualCurrency(request,
                        (result) => {
                            Debug.Log(result.BalanceChange + " ���� ȹ��");
                        },
                        (error) => Debug.Log("���� ȹ�� ����"));
                }
                else if ((string)reward_data[currentStage - 1]["Reward"] == "Mastery") {
                    string rewardSkill = (string)reward_data[currentStage - 1]["Value"];

                    result.GetComponent<MatchReward>().RewardValue.text = rewardSkill;

                    CharacterStats.CharacterSkill skill = CharacterStats.CharacterSkill.Null;

                    switch (rewardSkill) {
                        case "��������":
                            skill = CharacterStats.CharacterSkill.Defense_Disarm;
                            break;
                        case "��ƴϸ鵵":
                            skill = CharacterStats.CharacterSkill.Balance_GBGH;
                            break;
                        case "�߾�":
                            skill = CharacterStats.CharacterSkill.Attack_Struggle;
                            break;
                        case "�ݷ�":
                            skill = CharacterStats.CharacterSkill.Defense_Encourage;
                            break;
                        case "������":
                            skill = CharacterStats.CharacterSkill.Balance_Survivor;
                            break;
                        case "������":
                            skill = CharacterStats.CharacterSkill.Defense_Coward;
                            break;
                        case "����":
                            skill = CharacterStats.CharacterSkill.Balance_Curse;
                            break;
                        case "��ȣ��":
                            skill = CharacterStats.CharacterSkill.Attack_DivineShield;
                            break;
                        case "ó����":
                            skill = CharacterStats.CharacterSkill.Attack_Executioner;
                            break;
                        default:
                            break;
                    }

                    if (UserDataSynchronizer.unlockedSkillList.Contains(skill)) {
                        Debug.Log("���� ���� Ư���Դϴ�.");
                        return;
                    }

                    var request = new GrantCharacterToUserRequest() { CatalogVersion = "Skill", ItemId = "SKILL_" + ((int)skill).ToString() };
                    PlayFabClientAPI.GrantCharacterToUser(request,
                        (result) => {
                            Debug.Log($"Ư�� ȹ�� ����");
                        },
                        (error) => Debug.Log($"Ư�� ȹ�� ����"));
                }
            }
            else {
                Debug.Log("Pve_Lose");

                result.GetComponent<MatchReward>().WinTitle.SetActive(false);
                result.GetComponent<MatchReward>().RewardValue.gameObject.SetActive(false);
            }
        }

        if (isMatchOver)
            Invoke("LeaveRoom", 5f);
        else
            Invoke("BackToArrayment", 5f);
    }

    private void BackToArrayment() {
        if (!isPVE.HasValue) {
            object o_isPVE;
            isPVE = PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("IsPVE", out o_isPVE);
        }

        if (!isPVE.Value)
            PhotonNetwork.LoadLevel((int)Move_Scene.ENUM_SCENE.ARRAYMENT_SCENE);
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
                PhotonNetwork.LoadLevel((int)Move_Scene.ENUM_SCENE.PVE_SCENE);
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
