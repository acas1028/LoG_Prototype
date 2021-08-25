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
    [SerializeField]
    private DeckDataSync deckDataSync;

    /// <summary>
    /// 개인의 데이터에 접근하기 위한 ID
    /// </summary>
    [Tooltip("개인의 데이터에 접근하기 위한 ID")]
    [SerializeField]
    private string _playFabPlayerIdCache;

    private void Awake()
    {
        // 접근하고자 하는 PlayFab 서버의 ID
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

        PlayerPrefs.SetString("PlayFabId", _playFabPlayerIdCache);
        GetDeckData();
    }

    private void GetDeckData()
    {
        int num = deckDataSync.GetLastPageNum();
        //deckDataSync.GetData(0);
        deckDataSync.GetData(num);
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