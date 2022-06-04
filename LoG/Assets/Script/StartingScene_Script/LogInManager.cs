using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LogInManager : MonoBehaviour
{
    [Header("Log In Panel")]
    [SerializeField] InputField emailInput;
    [SerializeField] InputField passwordInput;
    [SerializeField] Button logInButton;

    [Header("Register Panel")]
    [SerializeField] InputField usernameInputRegister;
    [SerializeField] InputField emailInputRegister;
    [SerializeField] InputField passwordInputRegister;
    [SerializeField] Button registerButton;

    [Header("Notice Text")]
    [SerializeField] Text noticeText;

    // Start is called before the first frame update
    void Start()
    {
        if (string.IsNullOrEmpty(PlayFabSettings.TitleId)) PlayFabSettings.TitleId = "7CCDF"; // 우리 게임의 PlayFab ID는 7CCDF 입니다.

        emailInput.text = PlayerPrefs.GetString("userID");
        passwordInput.text = PlayerPrefs.GetString("userPassword");

        logInButton.onClick.AddListener(LogIn);
        registerButton.onClick.AddListener(Register);
        noticeText.text = string.Empty;
    }

    void LogIn() {
        var request = new LoginWithEmailAddressRequest { Email = emailInput.text, Password = passwordInput.text };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLogInSuccess, OnLogInFailed);
        noticeText.text = "로그인 시도 중..";
    }

    void Register() {
        var request = new RegisterPlayFabUserRequest { Username = usernameInputRegister.text, Email = emailInputRegister.text, Password = passwordInputRegister.text };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnRegisterFailed);
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

    void OnRegisterSuccess(RegisterPlayFabUserResult result) {
        Debug.Log("Registration Success : 회원가입 성공");

        usernameInputRegister.text = string.Empty;
        emailInputRegister.text = string.Empty;
        passwordInputRegister.text = string.Empty;

        noticeText.text = "회원가입에 성공하였습니다.";

        AddUserVirtualCurrencyRequest request = new AddUserVirtualCurrencyRequest() { VirtualCurrency = "CO", Amount = 1000 };
        PlayFabClientAPI.AddUserVirtualCurrency(request,
            (result) => print("회원가입 이벤트! 1000 코인을 획득하였습니다."),
            (error)=>print("알 수 없는 오류로 인해 1000 코인을 획득하지 못하였습니다."));
    }

    void OnRegisterFailed(PlayFabError error) {
        noticeText.text = "회원가입에 실패하였습니다.";
        Debug.Log("Sign-in Failed : " + noticeText.text);
    }
}
