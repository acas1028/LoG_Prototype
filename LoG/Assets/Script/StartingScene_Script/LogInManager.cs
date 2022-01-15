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
        if (string.IsNullOrEmpty(PlayFabSettings.TitleId)) PlayFabSettings.TitleId = "7CCDF"; // �츮 ������ PlayFab ID�� 7CCDF �Դϴ�.

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
        Debug.Log("Log-in Success : �α��� ����");
        SceneManager.LoadScene("MainLobbyScene");
    }

    void OnLogInFailed(PlayFabError error) {
        noticeText.text = "������ ã�� �� �����ϴ�. �Է��� ������ �߸��Ǿ� �ְų� ȸ�������� �ʿ��մϴ�.";
        Debug.Log("Log-in Failed : " + noticeText.text);
    }

    void OnSignInSuccess(RegisterPlayFabUserResult result) {
        Debug.Log("Sign-in Success : ȸ������ ����");

        usernameInputSignIn.text = string.Empty;
        emailInputSignIn.text = string.Empty;
        passwordInputSignIn.text = string.Empty;

        noticeText.text = "ȸ�����Կ� �����Ͽ����ϴ�.";
    }

    void OnSignInFailed(PlayFabError error) {
        noticeText.text = "ȸ�����Կ� �����Ͽ����ϴ�.";
        Debug.Log("Sign-in Failed : " + noticeText.text);
    }
}
