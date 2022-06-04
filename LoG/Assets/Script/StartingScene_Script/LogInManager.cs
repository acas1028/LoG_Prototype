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
        if (string.IsNullOrEmpty(PlayFabSettings.TitleId)) PlayFabSettings.TitleId = "7CCDF"; // �츮 ������ PlayFab ID�� 7CCDF �Դϴ�.

        emailInput.text = PlayerPrefs.GetString("userID");
        passwordInput.text = PlayerPrefs.GetString("userPassword");

        logInButton.onClick.AddListener(LogIn);
        registerButton.onClick.AddListener(Register);
        noticeText.text = string.Empty;
    }

    void LogIn() {
        var request = new LoginWithEmailAddressRequest { Email = emailInput.text, Password = passwordInput.text };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLogInSuccess, OnLogInFailed);
        noticeText.text = "�α��� �õ� ��..";
    }

    void Register() {
        var request = new RegisterPlayFabUserRequest { Username = usernameInputRegister.text, Email = emailInputRegister.text, Password = passwordInputRegister.text };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnRegisterFailed);
        noticeText.text = "ȸ������ �õ� ��..";
    }

    void OnLogInSuccess(LoginResult result) {
        Debug.Log("Log-in Success : �α��� ����");
        noticeText.text = "�α��ο� �����Ͽ����ϴ�.\n�κ�� �̵� ��..";
        PlayerPrefs.SetString("userID", emailInput.text);
        PlayerPrefs.SetString("userPassword", passwordInput.text);
        SceneManager.LoadSceneAsync((int)Move_Scene.ENUM_SCENE.MAINLOBBY_SCENE);
    }

    void OnLogInFailed(PlayFabError error) {
        switch (error.Error) {
            case PlayFabErrorCode.AccountNotFound:
                noticeText.text = "������ ã�� �� �����ϴ�.\n�̸��� �ּҸ� Ȯ�����ּ���.";
                break;
            case PlayFabErrorCode.InvalidAccount:
                noticeText.text = "�߸��� ���� �����Դϴ�.";
                break;
            case PlayFabErrorCode.InvalidEmailAddress:
                noticeText.text = "�߸��� �̸��� �ּ��Դϴ�.";
                break;
            case PlayFabErrorCode.InvalidEmailOrPassword:
                noticeText.text = "�߸��� ��й�ȣ �Դϴ�.";
                break;
            default:
                noticeText.text = "�α��� ����";
                break;
        }
        Debug.Log($"{error.ErrorMessage}");
    }

    void OnRegisterSuccess(RegisterPlayFabUserResult result) {
        Debug.Log("Registration Success : ȸ������ ����");

        usernameInputRegister.text = string.Empty;
        emailInputRegister.text = string.Empty;
        passwordInputRegister.text = string.Empty;

        noticeText.text = "ȸ�����Կ� �����Ͽ����ϴ�.";

        AddUserVirtualCurrencyRequest request = new AddUserVirtualCurrencyRequest() { VirtualCurrency = "CO", Amount = 1000 };
        PlayFabClientAPI.AddUserVirtualCurrency(request,
            (result) => print("ȸ������ �̺�Ʈ! 1000 ������ ȹ���Ͽ����ϴ�."),
            (error)=>print("�� �� ���� ������ ���� 1000 ������ ȹ������ ���Ͽ����ϴ�."));
    }

    void OnRegisterFailed(PlayFabError error) {
        noticeText.text = "ȸ�����Կ� �����Ͽ����ϴ�.";
        Debug.Log("Sign-in Failed : " + noticeText.text);
    }
}
