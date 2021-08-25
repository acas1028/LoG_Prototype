using System.Collections.Generic;

using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

// PlayFab�� �α����� �ϴ� Ŭ����
// PlayFab�� ���� �����͸� �����ϱ� ���� ����ϴ� ���̸�, �̿ܿ� ������ Ʈ���� ��Ȳ�̳� ��û Ƚ��, �÷��̾� ���� �� �� �ִ�.
public class PlayFabAuthenticator : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private DeckDataSync deckDataSync;

    /// <summary>
    /// ������ �����Ϳ� �����ϱ� ���� ID
    /// </summary>
    [Tooltip("������ �����Ϳ� �����ϱ� ���� ID")]
    [SerializeField]
    private string _playFabPlayerIdCache;

    private void Awake()
    {
        // �����ϰ��� �ϴ� PlayFab ������ ID
        PlayFabSettings.TitleId = "7CCDF";
        AuthenticateWithPlayFab();
    }

    // �ӽ� ID(��ġ�� ����� ID)�� PlayFab�� ����
    private void AuthenticateWithPlayFab()
    {
        LogMessage("PlayFab authenticating using Custom ID...");

        PlayFabClientAPI.LoginWithCustomID(new LoginWithCustomIDRequest()
        {
            CreateAccount = true,
            CustomId = PlayFabSettings.DeviceUniqueIdentifier
        }, RequestPhotonToken, OnPlayFabError);
    }

    // PlayFab�� Photon Token�� ��û
    private void RequestPhotonToken(LoginResult obj)
    {
        LogMessage("PlayFab authenticated. Requesting photon token...");

        _playFabPlayerIdCache = obj.PlayFabId;

        PlayFabClientAPI.GetPhotonAuthenticationToken(new GetPhotonAuthenticationTokenRequest()
        {
            PhotonApplicationId = "996cb4a8-225b-4185-8f5e-80396335746f" // Photon Realtime AppID
        }, AuthenticateWithPhoton, OnPlayFabError);
    }

    // PlayFab���� ���� Photon Token�� ������� ������(Authentication Value) �Ҵ�, �̴� �α����� �ϱ� ���� �Է��ϴ� ���̵�� ��й�ȣ ������ ����ϴ�.
    private void AuthenticateWithPhoton(GetPhotonAuthenticationTokenResult obj)
    {
        LogMessage("Photon token acquired: " + obj.PhotonCustomAuthenticationToken + " Authentication complete.");

        var customAuth = new AuthenticationValues { AuthType = CustomAuthenticationType.Custom };
        customAuth.AddAuthParameter("username", _playFabPlayerIdCache);
        customAuth.AddAuthParameter("token", obj.PhotonCustomAuthenticationToken);
        PhotonNetwork.AuthValues = customAuth;

        PlayerPrefs.SetString("PlayFabId", _playFabPlayerIdCache);
        GetDeckData();
    }

    private void GetDeckData()
    {
        int num = deckDataSync.GetLastPageNum();
        //deckDataSync.GetData(0);
        deckDataSync.GetData(num);
    }

    // ���� �߻��� �ݹ�Ǵ� �Լ�
    private void OnPlayFabError(PlayFabError obj)
    {
        Debug.LogError("PlayFab Authentication Failed: " + obj.GenerateErrorReport());
    }

    private void LogMessage(string message)
    {
        Debug.Log("PlayFab: " + message);
    }
}