using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class CSVManager : MonoBehaviourPunCallbacks
{
    public static int StageNumber;
    // Start is called before the first frame update
    void Start()
    {
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
