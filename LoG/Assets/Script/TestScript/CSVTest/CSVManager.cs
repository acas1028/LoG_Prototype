using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class CSVManager : MonoBehaviourPunCallbacks
{
    private static CSVManager _instance;
    // 인스턴스에 접근하기 위한 프로퍼티
    public static CSVManager Instance
    {
        get
        {
            // 인스턴스가 없는 경우에 접근하려 하면 인스턴스를 할당해준다.
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(CSVManager)) as CSVManager;

                if (_instance == null)
                    Debug.Log("no Singleton obj");
            }
            return _instance;
        }
    }
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        // 인스턴스가 존재하는 경우 새로생기는 인스턴스를 삭제한다.
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    public int StageNumber;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        StageNumber = 0;
    }

    public void SettingStage(int stageNum)
    {
        StageNumber = stageNum;
        PhotonNetwork.OfflineMode = true;
        Connect();
    }

    private void Connect() {
        // we check if we are connected or not, we join if we are , else we initiate the connection to the server.
        if (PhotonNetwork.IsConnected) {
            // offline mode = true 인 경우 즉시 PhotonNetwork.IsConnected = true 가 된다.
            Debug.Log("<color=lightblue>현재 서버와 연결되어있거나 오프라인 모드입니다. 룸에 입장합니다.</color>");

            PhotonNetwork.JoinRandomRoom();
        }
        else {
            Debug.Log("<color=lightblue>현재 서버와 연결되어있지 않아 새로 연결을 시도합니다.</color>");
            // #Critical, we must first and foremost connect to Photon Online Server.

            PhotonNetwork.ConnectUsingSettings();
        }
    }

    #region 포톤 콜백
    // 최초로 마스터 서버에 연결됐을 때 콜백되는 함수
    public override void OnJoinedRoom() {
        Debug.Log("<color=yellow>OnJoinedRoom() 호출\n이제 당신은 룸에 있습니다. 여기서 당신의 게임이 시작됩니다.</color>");

        PhotonNetwork.LoadLevel((int)Move_Scene.ENUM_SCENE.PVE_CSVTESTSCENE2);
    }
    #endregion

}
