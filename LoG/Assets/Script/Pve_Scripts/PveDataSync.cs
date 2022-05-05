using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

using PlayFab;
using PlayFab.ClientModels;

public class PveDataSync : MonoBehaviour
{
    public static PveDataSync instance;
    bool isGetAllData;

    private void Awake()
    {
        if (!instance)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        if (!PlayFabClientAPI.IsClientLoggedIn())
            Debug.LogError("�α����� �ʿ��մϴ�.");

        isGetAllData = false;

        GetData();
    }

    public void SetData(int clearStage)
    {
        //������ ����Ǵ� key �� ���

        string clearStage_Num = clearStage.ToString();


        
    }

    public void GetData()
    {
        int clearStage = 0; // default

        var request = new GetUserDataRequest();

        PlayFabClientAPI.GetUserData(request,
            result =>
            {
                foreach (var item in result.Data)
                {
                    if (item.Key == "clearStage")
                    {
                        clearStage = int.Parse(item.Value.Value);
                        PveStageClearManager.instance.clearstage = clearStage;
                        Debug.Log("Ŭ������ �������� ��ȣ �ҷ�����: " + clearStage);
                    }

                }
                isGetAllData = true;
            }, error => Debug.LogWarningFormat("������ �ҷ����� ����: {0}", error.ErrorMessage)
        );
    }

    public void SendClearStage(int clearstage_Number)
    {
        var request = new UpdateUserDataRequest() { Data = new Dictionary<string, string>() { { "clearStage", clearstage_Number.ToString() } }, Permission = UserDataPermission.Private };
        PlayFabClientAPI.UpdateUserData(request,
                result =>
                {
                    foreach (var item in request.Data)
                    {
                        Debug.LogFormat("Ŭ���� �������� ��ȣ ���� ����: {0} / {1}", item.Key, item.Value);
                    }
                }, error => Debug.LogWarningFormat("Ŭ���� �������� ��ȣ ���� ����: {0}", error.ErrorMessage)
            );
    }

    public bool IsGetAllData()
    {
        return isGetAllData;
    }
}


