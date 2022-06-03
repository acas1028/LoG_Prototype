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
            // offline mode = true �� ��� ��� PhotonNetwork.IsConnected = true �� �ȴ�.
            Debug.Log("<color=lightblue>���� ������ ����Ǿ��ְų� �������� ����Դϴ�. �뿡 �����մϴ�.</color>");

            PhotonNetwork.JoinRandomRoom();
        }
        else {
            Debug.Log("<color=lightblue>���� ������ ����Ǿ����� �ʾ� ���� ������ �õ��մϴ�.</color>");
            // #Critical, we must first and foremost connect to Photon Online Server.

            PhotonNetwork.ConnectUsingSettings();
        }
    }

    #region ���� �ݹ�
    // ���ʷ� ������ ������ ������� �� �ݹ�Ǵ� �Լ�
    public override void OnJoinedRoom() {
        Debug.Log("<color=yellow>OnJoinedRoom() ȣ��\n���� ����� �뿡 �ֽ��ϴ�. ���⼭ ����� ������ ���۵˴ϴ�.</color>");

        PhotonNetwork.LoadLevel((int)Move_Scene.ENUM_SCENE.PVE_CSVTESTSCENE2);
    }
    #endregion

}
