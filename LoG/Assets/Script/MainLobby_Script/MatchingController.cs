using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class MatchingController : MonoBehaviourPunCallbacks
{
	bool offlineMode;

	public enum ROOM_TYPE {
		PVE,
		PVP
    }

	[SerializeField] Button pvpButton;

    IEnumerator Start() {
		pvpButton.onClick.AddListener(EnterRoom);

		PhotonNetwork.AutomaticallySyncScene = false;
		if (!PhotonNetwork.IsConnected)
			PhotonNetwork.ConnectUsingSettings();

		yield return new WaitUntil(() => DeckDataSync.Instance.IsGetAllData());
		pvpButton.interactable = true;
	}

    public void EnterRoom() {
		offlineMode = false;
		PhotonNetwork.OfflineMode = false;
		JoinRoom();
	}

    public void EnterOfflineMode() {
		offlineMode = true;

		if (PhotonNetwork.IsConnected)
			PhotonNetwork.Disconnect();
		else {
			PhotonNetwork.OfflineMode = true;
			JoinRoom();
		}
	}

	void JoinRoom() {
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
			PhotonNetwork.JoinRandomRoom();
		}
	}

	#region ���� �ݹ� �Լ�
	public override void OnJoinRandomFailed(short returnCode, string message) {
		Debug.Log("<color=yellow>OnJoinRandomFailed() ȣ��\n���� ������ ���� ���� �� ���� ����ϴ�.</color>");

		string roomName = "Room " + Random.Range(1, 100);

		PhotonNetwork.CreateRoom(roomName, new RoomOptions { MaxPlayers = 2 }, null);
	}

	public override void OnDisconnected(DisconnectCause cause) {
		Debug.LogWarning("<color=yellow>Disconnected\n���� ������</color>");

		if (offlineMode) {
			PhotonNetwork.OfflineMode = true;
			JoinRoom();
		}
	}

	public override void OnJoinedRoom() {
		Debug.Log("<color=yellow>OnJoinedRoom() ȣ��\n���� ����� �뿡 �ֽ��ϴ�. ���⼭ ����� ������ ���۵˴ϴ�.</color>");

		PhotonNetwork.LoadLevel((int)Move_Scene.ENUM_SCENE.ARRAYMENT_SCENE);
	}
	#endregion
}
