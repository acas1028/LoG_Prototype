using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;

#pragma warning disable 649

	public class Lobby : MonoBehaviourPunCallbacks
	{

		#region Private Serializable Fields

		[Tooltip("플레이어 이름 입력 패널")]
		[SerializeField]
		private GameObject loginPanel;

		[Tooltip("연결 상태 출력 텍스트")]
		[SerializeField]
		private Text feedbackText;

		[Tooltip("룸에 입장할 수 있는 최대 인원 수")]
		[SerializeField]
		private byte maxPlayersPerRoom = 2;

		[Tooltip("로딩 효과")]
		[SerializeField]
		private LoadingEffect loadingEffect;

		#endregion

		#region Private 변수
		/// <summary>
		/// Keep track of the current process. Since connection is asynchronous and is based on several callbacks from Photon, 
		/// we need to keep track of this to properly adjust the behavior when we receive call back by Photon.
		/// Typically this is used for the OnConnectedToMaster() callback.
		/// </summary>
		bool isConnecting;

		/// <summary>
		/// This client's version number. Users are separated from each other by gameVersion (which allows you to make breaking changes).
		/// </summary>
		string gameVersion = "1";

		#endregion

		#region MonoBehaviour 콜백 함수

		/// <summary>
		/// MonoBehaviour method called on GameObject by Unity during early initialization phase.
		/// </summary>
		void Awake()
		{
			if (loadingEffect == null)
			{
				Debug.LogError("<Color=Red><b>Missing</b></Color> 로딩 이펙트 찾을 수 없음.", this);
			}

			// #Critical
			// this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
			PhotonNetwork.AutomaticallySyncScene = true;

		}

		#endregion


		#region Public 함수

		/// <summary>
		/// Start the connection process. 
		/// - If already connected, we attempt joining a random room
		/// - if not yet connected, Connect this application instance to Photon Cloud Network
		/// </summary>
		public void Connect()
		{
			// we want to make sure the log is clear everytime we connect, we might have several failed attempted if connection failed.
			feedbackText.text = "";

			// keep track of the will to join a room, because when we come back from the game we will get a callback that we are connected, so we need to know what to do then
			isConnecting = true;

			// 메인 패널 숨기기
			loginPanel.SetActive(false);

			// start the loader animation for visual effect.
			if (loadingEffect != null)
			{
				loadingEffect.StartLoaderAnimation();
			}

			// we check if we are connected or not, we join if we are , else we initiate the connection to the server.
			if (PhotonNetwork.IsConnected)
			{
				LogFeedback("룸에 입장 중...");
				// #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
				PhotonNetwork.JoinRandomRoom();
			}
			else
			{
				LogFeedback("연결 중...");
				// #Critical, we must first and foremost connect to Photon Online Server.
				PhotonNetwork.ConnectUsingSettings();
				PhotonNetwork.GameVersion = this.gameVersion;
			}
		}

		/// <summary>
		/// Logs the feedback in the UI view for the player, as opposed to inside the Unity Editor for the developer.
		/// </summary>
		/// <param name="message">Message.</param>
		void LogFeedback(string message)
		{
			// we do not assume there is a feedbackText defined.
			if (feedbackText == null)
			{
				return;
			}

			// add new messages as a new line and at the bottom of the log.
			feedbackText.text += System.Environment.NewLine + message;
		}

		#endregion


		#region 포톤 콜백 함수
		// below, we implement some callbacks of PUN
		// you can find PUN's callbacks in the class MonoBehaviourPunCallbacks


		/// <summary>
		/// Called after the connection to the master is established and authenticated
		/// </summary>
		public override void OnConnectedToMaster()
		{
			// we don't want to do anything if we are not attempting to join a room. 
			// this case where isConnecting is false is typically when you lost or quit the game, when this level is loaded, OnConnectedToMaster will be called, in that case
			// we don't want to do anything.
			if (isConnecting)
			{
				LogFeedback("OnConnectedToMaster: 무작위 룸에 참가합니다.");
				Debug.Log("OnConnectedToMaster() 호출\n당신은 서버와 연결되었고 룸에 참가할 수 있습니다.");

				// #Critical: The first we try to do is to join a potential existing room. If there is, good, else, we'll be called back with OnJoinRandomFailed()
				PhotonNetwork.JoinRandomRoom();
			}
		}

		/// <summary>
		/// Called when a JoinRandom() call failed. The parameter provides ErrorCode and message.
		/// </summary>
		/// <remarks>
		/// Most likely all rooms are full or no rooms are available. <br/>
		/// </remarks>
		public override void OnJoinRandomFailed(short returnCode, string message)
		{
			LogFeedback("<Color=Red>OnJoinRandomFailed</Color>: 입장 가능한 룸이 없어 새 룸을 만듭니다.");
			Debug.Log("OnJoinRandomFailed() 호출\n입장 가능한 룸이 없어 새 룸을 만듭니다.");

			// #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
			PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = this.maxPlayersPerRoom });
		}


		/// <summary>
		/// Called after disconnecting from the Photon server.
		/// </summary>
		public override void OnDisconnected(DisconnectCause cause)
		{
			LogFeedback("<Color=Red>OnDisconnected</Color> " + cause);
			Debug.LogError("연결 해제됨");

			// #Critical: we failed to connect or got disconnected. There is not much we can do. Typically, a UI system should be in place to let the user attemp to connect again.
			loadingEffect.StopLoaderAnimation();

			isConnecting = false;
			loginPanel.SetActive(true);

		}

		/// <summary>
		/// Called when entering a room (by creating or joining it). Called on all clients (including the Master Client).
		/// </summary>
		/// <remarks>
		/// This method is commonly used to instantiate player characters.
		/// If a match has to be started "actively", you can call an [PunRPC](@ref PhotonView.RPC) triggered by a user's button-press or a timer.
		///
		/// When this is called, you can usually already access the existing players in the room via PhotonNetwork.PlayerList.
		/// Also, all custom properties should be already available as Room.customProperties. Check Room..PlayerCount to find out if
		/// enough players are in the room to start playing.
		/// </remarks>
		public override void OnJoinedRoom()
		{
			LogFeedback("<Color=Green>OnJoinedRoom</Color> " + PhotonNetwork.CurrentRoom.PlayerCount + "명이 있는 룸에 입장합니다.");
			Debug.Log("OnJoinedRoom() 호출\n이제 당신은 룸에 있습니다. 여기서 당신의 게임이 시작됩니다.");

			PhotonNetwork.LoadLevel("Arrayment_Scene");
		}

		#endregion

	}