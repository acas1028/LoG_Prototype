using System.Collections.Generic;

using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

// PlayFab에 로그인을 하는 클래스
// PlayFab은 웹에 데이터를 저장하기 위해 사용하는 것이며, 이외에 서버의 트래픽 현황이나 요청 횟수, 플레이어 등을 볼 수 있다.
public class PlayFabAuthenticator : MonoBehaviourPunCallbacks
{
    private string _playFabPlayerIdCache;

    private void Awake()
    {
        PlayFabSettings.TitleId = "7CCDF";
        AuthenticateWithPlayFab();
    }

    // 임시 ID(장치에 기반한 ID)로 PlayFab에 연결
    private void AuthenticateWithPlayFab()
    {
        LogMessage("PlayFab authenticating using Custom ID...");

        PlayFabClientAPI.LoginWithCustomID(new LoginWithCustomIDRequest()
        {
            CreateAccount = true,
            CustomId = PlayFabSettings.DeviceUniqueIdentifier
        }, RequestPhotonToken, OnPlayFabError);
    }

    // PlayFab에 Photon Token을 요청
    private void RequestPhotonToken(LoginResult obj)
    {
        LogMessage("PlayFab authenticated. Requesting photon token...");

        _playFabPlayerIdCache = obj.PlayFabId;

        PlayFabClientAPI.GetPhotonAuthenticationToken(new GetPhotonAuthenticationTokenRequest()
        {
            PhotonApplicationId = "996cb4a8-225b-4185-8f5e-80396335746f" // Photon Realtime AppID
        }, AuthenticateWithPhoton, OnPlayFabError);
    }

    // PlayFab에서 받은 Photon Token을 기반으로 인증값(Authentication Value) 할당, 이는 로그인을 하기 위해 입력하는 아이디와 비밀번호 묶음과 비슷하다.
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
                    Debug.LogFormat("플레이어 데이터 저장 성공: {0} / {1}", item.Key, item.Value);
                }
            }, error => Debug.LogWarningFormat("플레이어 데이터 저장 실패: {0}", error.ErrorMessage)
        );
    }

    public void GetData()
    {
        var request = new GetUserDataRequest() { PlayFabId = _playFabPlayerIdCache };
        PlayFabClientAPI.GetUserData(request,
            result => {
                foreach (var item in result.Data)
                {
                    Debug.LogFormat("불러온 데이터: {0} / {1}", item.Key, item.Value);
                }
            }, error => Debug.LogWarningFormat("데이터 불러오기 실패: {0}", error.ErrorMessage)
        );
    }

    // 에러 발생시 콜백되는 함수
    private void OnPlayFabError(PlayFabError obj)
    {
        Debug.LogError("PlayFab Authentication Failed: " + obj.GenerateErrorReport());
    }

    private void LogMessage(string message)
    {
        Debug.Log("PlayFab: " + message);
    }
}