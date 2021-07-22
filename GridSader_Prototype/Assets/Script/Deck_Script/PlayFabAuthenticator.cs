using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

// PlayFab�� �α����� �ϴ� Ŭ����
// PlayFab�� ���� �����͸� �����ϱ� ���� ����ϴ� ���̸�, �̿ܿ� ������ Ʈ���� ��Ȳ�̳� ��û Ƚ��, �÷��̾� ���� �� �� �ִ�.
public class PlayFabAuthenticator : MonoBehaviour
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
            PhotonApplicationId = PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime
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