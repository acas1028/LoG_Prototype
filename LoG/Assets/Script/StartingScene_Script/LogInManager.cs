using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using PlayFab;
using PlayFab.ClientModels;

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

        logInButton.onClick.AddListener(LogIn);
        signInButton.onClick.AddListener(SignIn);
        noticeText.text = string.Empty;
    }

    void LogIn() {
        var request = new LoginWithEmailAddressRequest { Email = emailInput.text, Password = passwordInput.text };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLogInSuccess, OnLogInFailed);
    }

    void SignIn() {
        var request = new RegisterPlayFabUserRequest { Username = usernameInputSignIn.text, Email = emailInputSignIn.text, Password = passwordInputSignIn.text };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnSignInSuccess, OnSignInFailed);
    }

    void OnLogInSuccess(LoginResult result) {
        Debug.Log("Log-in Success : 로그인 성공");
        SceneManager.LoadScene("MainLobbyScene");
    }

    void OnLogInFailed(PlayFabError error) {
        noticeText.text = "계정을 찾을 수 없습니다. 입력한 정보가 잘못되어 있거나 회원가입이 필요합니다.";
        Debug.Log("Log-in Failed : " + noticeText.text);
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
