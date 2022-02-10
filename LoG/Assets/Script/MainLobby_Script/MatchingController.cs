using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class MatchingController : MonoBehaviourPunCallbacks
{
    public void EnterRoom() {
		StartCoroutine(ConnectCoroutine());
	}

    public void EnterOfflineMode() {
		PhotonNetwork.OfflineMode = true;

		StartCoroutine(ConnectCoroutine());
	}

	IEnumerator ConnectCoroutine() {
		yield return new WaitUntil(DeckDataSync.Instance.IsGetAllData);

		// we check if we are connected or not, we join if we are , else we initiate the connection to the server.
		if (PhotonNetwork.IsConnected) {
			// offline mode = true �� ��� ��� PhotonNetwork.IsConnected = true �� �ȴ�.
			Debug.Log("<color=lightblue>���� ������ ����Ǿ��ְų� �������� ����Դϴ�. �뿡 �����մϴ�.</color>");
			// #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
			PhotonNetwork.JoinRandomRoom();
		}
		else {
			Debug.Log("<color=lightblue>���� ������ ����Ǿ����� �ʾ� ���� ������ �õ��մϴ�.</color>");
			// #Critical, we must first and foremost connect to Photon Online Server.

			PhotonNetwork.ConnectUsingSettings();
		}
	}

	#region ���� �ݹ� �Լ�
	public override void OnConnectedToMaster() {
		// ���ʷ� ������ ������ ������� �� �ݹ�Ǵ� �Լ�
		// we don't want to do anything if we are not attempting to join a room. 
		// this case where isConnecting is false is typically when you lost or quit the game, when this level is loaded, OnConnectedToMaster will be called, in that case
		// we don't want to do anything.

		Debug.Log("<color=yellow>OnConnectedToMaster() ȣ��\n������ ���� �����</color>");

		PhotonNetwork.JoinRandomRoom();
	}

	public override void OnJoinRandomFailed(short returnCode, string message) {
		Debug.Log("<color=yellow>OnJoinRandomFailed() ȣ��\n���� ������ ���� ���� �� ���� ����ϴ�.</color>");

		string roomName = "Room " + Random.Range(1, 100);

		PhotonNetwork.CreateRoom(roomName, new RoomOptions { MaxPlayers = 2 }, null);
	}

	public override void OnDisconnected(DisconnectCause cause) {
		Debug.LogWarning("<color=yellow>Disconnected\n���� ������</color>");
	}

	public override void OnJoinedRoom() {
		Debug.Log("<color=yellow>OnJoinedRoom() ȣ��\n���� ����� �뿡 �ֽ��ϴ�. ���⼭ ����� ������ ���۵˴ϴ�.</color>");

		PhotonNetwork.LoadLevel("Arrayment_Scene");
	}
	#endregion
}