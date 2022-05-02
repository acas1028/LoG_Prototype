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
            Debug.LogError("로그인이 필요합니다.");

        isGetAllData = false;

        GetData();
    }

    public void SetData(int clearStage)
    {
        //서버에 저장되는 key 값 양식

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
                        Debug.Log("클리어한 스테이지 번호 불러오기: " + clearStage);
                    }

                }
                isGetAllData = true;
            }, error => Debug.LogWarningFormat("데이터 불러오기 실패: {0}", error.ErrorMessage)
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
                        Debug.LogFormat("클리어 스테이지 번호 저장 성공: {0} / {1}", item.Key, item.Value);
                    }
                }, error => Debug.LogWarningFormat("클리어 스테이지 번호 저장 실패: {0}", error.ErrorMessage)
            );
    }

    public bool IsGetAllData()
    {
        return isGetAllData;
    }
}


