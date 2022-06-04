using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class MatchingController : MonoBehaviourPunCallbacks
{
	public enum ROOM_TYPE {
		PVE,
		PVP
    }

	[SerializeField] Button pvpButton;

    IEnumerator Start() {
		pvpButton.onClick.AddListener(EnterRoom);

		yield return new WaitUntil(() => DeckDataSync.Instance.IsGetAllData());
		pvpButton.interactable = true;
	}

    public void EnterRoom() {
		PhotonNetwork.OfflineMode = false;
		Connect();
	}

    public void EnterOfflineMode() {
		PhotonNetwork.OfflineMode = true;
		Connect();
	}

	void Connect() {
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

	#region 포톤 콜백 함수
	public override void OnConnectedToMaster() {
		// 최초로 마스터 서버에 연결됐을 때 콜백되는 함수
		// we don't want to do anything if we are not attempting to join a room. 
		// this case where isConnecting is false is typically when you lost or quit the game, when this level is loaded, OnConnectedToMaster will be called, in that case
		// we don't want to do anything.

		Debug.Log("<color=yellow>OnConnectedToMaster() 호출\n마스터 서버 연결됨</color>");
		PhotonNetwork.JoinRandomRoom();
	}

	public override void OnJoinRandomFailed(short returnCode, string message) {
		Debug.Log("<color=yellow>OnJoinRandomFailed() 호출\n입장 가능한 룸이 없어 새 룸을 만듭니다.</color>");

		string roomName = "Room " + Random.Range(1, 100);

		PhotonNetwork.CreateRoom(roomName, new RoomOptions { MaxPlayers = 2 }, null);
	}

	public override void OnDisconnected(DisconnectCause cause) {
		Debug.LogWarning("<color=yellow>Disconnected\n연결 해제됨</color>");
	}

	public override void OnJoinedRoom() {
		Debug.Log("<color=yellow>OnJoinedRoom() 호출\n이제 당신은 룸에 있습니다. 여기서 당신의 게임이 시작됩니다.</color>");

		PhotonNetwork.LoadLevel((int)Move_Scene.ENUM_SCENE.ARRAYMENT_SCENE);
	}
	#endregion
}
