using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using PlayFab;
using PlayFab.ClientModels;

using Photon.Pun;

public class LogInManager : MonoBehaviour
{
    [Header("Log In Panel")]
    [SerializeField] InputField emailInput;
    [SerializeField] InputField passwordInput;
    [SerializeField] Button logInButton;

    [Header("Sign In Panel")]
    [SerializeField] InputField usernameInputSignIn;
    [SerializeField] InputField emailInputSignIn;
    [SerializeField] InputField passwordInputSignIn;
    [SerializeField] Button signInButton;

    [Header("Notice Text")]
    [SerializeField] Text noticeText;

    // Start is called before the first frame update
    void Start()
    {
        if (string.IsNullOrEmpty(PlayFabSettings.TitleId)) PlayFabSettings.TitleId = "7CCDF"; // 우리 게임의 PlayFab ID는 7CCDF 입니다.

        emailInput.text = PlayerPrefs.GetString("userID");
        passwordInput.text = PlayerPrefs.GetString("userPassword");

        logInButton.onClick.AddListener(LogIn);
        signInButton.onClick.AddListener(SignIn);
        noticeText.text = string.Empty;
    }

    void LogIn() {
        var request = new LoginWithEmailAddressRequest { Email = emailInput.text, Password = passwordInput.text };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLogInSuccess, OnLogInFailed);
        noticeText.text = "로그인 시도 중..";
    }

    void SignIn() {
        var request = new RegisterPlayFabUserRequest { Username = usernameInputSignIn.text, Email = emailInputSignIn.text, Password = passwordInputSignIn.text };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnSignInSuccess, OnSignInFailed);
        noticeText.text = "회원가입 시도 중..";
    }

    void OnLogInSuccess(LoginResult result) {
        Debug.Log("Log-in Success : 로그인 성공");
        noticeText.text = "로그인에 성공하였습니다.\n로비로 이동 중..";
        PlayerPrefs.SetString("userID", emailInput.text);
        PlayerPrefs.SetString("userPassword", passwordInput.text);
        SceneManager.LoadSceneAsync((int)Move_Scene.ENUM_SCENE.MAINLOBBY_SCENE);
    }

    void OnLogInFailed(PlayFabError error) {
        switch (error.Error) {
            case PlayFabErrorCode.AccountNotFound:
                noticeText.text = "계정을 찾을 수 없습니다.\n이메일 주소를 확인해주세요.";
                break;
            case PlayFabErrorCode.InvalidAccount:
                noticeText.text = "잘못된 계정 정보입니다.";
                break;
            case PlayFabErrorCode.InvalidEmailAddress:
                noticeText.text = "잘못된 이메일 주소입니다.";
                break;
            case PlayFabErrorCode.InvalidEmailOrPassword:
                noticeText.text = "잘못된 비밀번호 입니다.";
                break;
            default:
                noticeText.text = "로그인 실패";
                break;
        }
        Debug.Log($"{error.ErrorMessage}");
    }

    void OnSignInSuccess(RegisterPlayFabUserResult result) {
        Debug.Log("Sign-in Success : 회원가입 성공");

        usernameInputSignIn.text = string.Empty;
        emailInputSignIn.text = string.Empty;
        passwordInputSignIn.text = string.Empty;

        noticeText.text = "회원가입에 성공하였습니다.";
    }

    void OnSignInFailed(PlayFabError error) {
        noticeText.text = "회원가입에 실패하였습니다.";
        Debug.Log("Sign-in Failed : " + noticeText.text);
    }
}
