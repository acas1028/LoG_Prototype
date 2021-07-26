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
    private string _playFabPlayerIdCache;

    private void Awake()
    {
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
    }

    public void SendData()
    {
        //var data = new Dictionary<string, object>() { { "Hello", "World" } };
        //var flags = new WebFlags(WebFlags.HttpForwardConst);
        //var result = PhotonNetwork.RaiseEvent(0, data, new RaiseEventOptions()
        //{
        //    Flags = flags
        //}, new SendOptions());
        //LogMessage("Data(Dictionary Type) Transmition: " + result);

        //var properties = new Hashtable() {
        //    {"CustomProperty", "It's Value"}};
        //var expectedProperties = new Hashtable();
        //PhotonNetwork.CurrentRoom.SetCustomProperties(properties, expectedProperties, flags);
        //LogMessage("New Room Properties Set");

        var data = new Dictionary<string, string>() { { "name", PhotonNetwork.NickName } };
        var request = new UpdateUserDataRequest() { Data = data, Permission = UserDataPermission.Public };
        PlayFabClientAPI.UpdateUserData(request,
            result => {
                foreach (var item in request.Data)
                {
                    Debug.LogFormat("�÷��̾� ������ ���� ����: {0} / {1}", item.Key, item.Value);
                }
            }, error => Debug.LogWarningFormat("�÷��̾� ������ ���� ����: {0}", error.ErrorMessage)
        );
    }

    public void GetData()
    {
        var request = new GetUserDataRequest() { PlayFabId = _playFabPlayerIdCache };
        PlayFabClientAPI.GetUserData(request,
            result => {
                foreach (var item in result.Data)
                {
                    Debug.LogFormat("�ҷ��� ������: {0} / {1}", item.Key, item.Value);
                }
            }, error => Debug.LogWarningFormat("������ �ҷ����� ����: {0}", error.ErrorMessage)
        );
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