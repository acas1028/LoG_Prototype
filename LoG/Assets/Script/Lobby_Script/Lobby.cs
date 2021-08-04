using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;

#pragma warning disable 649

public class Lobby : MonoBehaviourPunCallbacks
{

	#region Private Serializable Fields

	[Header("PlayFab 로그인 관리자")]
	public GameObject PlayFabAuth;

	[Header("Web Sync Panel")]
	[Tooltip("웹 동기화 패널")]
	[SerializeField]
	private GameObject WebSyncPanel;

	[Header("Login Panel")]
	[Tooltip("플레이어 이름 입력 패널")]
	[SerializeField]
	private GameObject LoginPanel;

	[Header("Selection Panel")]
	[Tooltip("선택 패널")]
	public GameObject SelectionPanel;

	[Header("Create Room Panel")]
	[Tooltip("룸 생성 패널")]
	public GameObject CreateRoomPanel;

	public InputField RoomNameInputField;

	[Header("Join Random Room Panel")]
	[Tooltip("무작위 룸 입장 패널")]
	public GameObject JoinRandomRoomPanel;

	[Header("Room List Panel")]
	[Tooltip("룸 목록 보기 패널")]
	public GameObject RoomListPanel;

	public GameObject RoomListContent;
	public GameObject RoomListEntryPrefab;

	[Tooltip("연결 상태 텍스트 수직스크롤바")]
	public Scrollbar statusVerticalBar;

	[Tooltip("연결 상태 출력 텍스트")]
	[SerializeField]
	private Text feedbackText;

	[Tooltip("룸에 입장할 수 있는 최대 인원 수")]
	[SerializeField]
	private byte maxPlayersPerRoom = 2;

	[Tooltip("로딩 효과")]
	[SerializeField]
	private LoadingEffect loadingEffect;

	private Dictionary<string, RoomInfo> cachedRoomList;
	private Dictionary<string, GameObject> roomListEntries;

	#endregion

	#region Private 변수

	/// <summary>
	/// This client's version number. Users are separated from each other by gameVersion (which allows you to make breaking changes).
	/// </summary>
	string gameVersion = "1";

	#endregion

	#region MonoBehaviour 콜백 함수

	private void Awake()
	{
		cachedRoomList = new Dictionary<string, RoomInfo>();
		roomListEntries = new Dictionary<string, GameObject>();

		// this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
		PhotonNetwork.AutomaticallySyncScene = true;

		if (loadingEffect == null)
		{
			Debug.LogError("<Color=Red><b>Missing</b></Color> 로딩 이펙트 찾을 수 없음.", this);
		}
	}

    private void Start()
    {
		// 캐릭터 배치 데이터 지우기
		if (Arrayed_Data.instance)
		{
			Destroy(Arrayed_Data.instance.gameObject);
			Arrayed_Data.instance = null;
		}

		// 룸에서 퇴장한 경우 로그인이 아닌 선택 패널이 바로 나오도록 한다.
		if (PhotonNetwork.OfflineMode)
		{
			PhotonNetwork.Disconnect();
			SetActivePanel(LoginPanel.name);
		}
		else if (PhotonNetwork.IsConnected)
			SetActivePanel(SelectionPanel.name);
    }

    #endregion


    #region Public 함수

    public void Connect()
	{
		// start the loader animation for visual effect.
		if (loadingEffect != null)
		{
			loadingEffect.StartLoaderAnimation();
		}

		// we check if we are connected or not, we join if we are , else we initiate the connection to the server.
		if (PhotonNetwork.IsConnected)
		{
			SetActivePanel(JoinRandomRoomPanel.name);

			// offline mode = true 인 경우 즉시 PhotonNetwork.IsConnected = true 가 된다.
			LogFeedback("룸에 입장 중...");
			Debug.Log("<color=lightblue>현재 서버와 연결되어있거나 오프라인 모드입니다. 룸에 입장합니다.</color>");
			// #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
			PhotonNetwork.JoinRandomRoom();
		}
		else
		{
			LogFeedback("연결됨");
			Debug.Log("<color=lightblue>현재 서버와 연결되어있지 않아 새로 연결을 시도합니다.</color>");
			// #Critical, we must first and foremost connect to Photon Online Server.

			PlayFabAuth.SetActive(true);

			PhotonNetwork.ConnectUsingSettings();
			PhotonNetwork.GameVersion = this.gameVersion;
		}
	}

	public void OfflineMode()
	{
		PhotonNetwork.OfflineMode = true;
		LogFeedback("오프라인 모드로 입장 중...");

		Connect();
	}

	public void DeckScene()
    {
		PhotonNetwork.LoadLevel("Deck_Scene");
    }

	/// <summary>
	/// 하나만 활성화하고 나머지 패널들을 모두 비활성화하는 함수
	/// </summary>
	public void SetActivePanel(string activePanel)
	{
		LoginPanel.SetActive(activePanel.Equals(LoginPanel.name));
		SelectionPanel.SetActive(activePanel.Equals(SelectionPanel.name));
		CreateRoomPanel.SetActive(activePanel.Equals(CreateRoomPanel.name));
		JoinRandomRoomPanel.SetActive(activePanel.Equals(JoinRandomRoomPanel.name));
		RoomListPanel.SetActive(activePanel.Equals(RoomListPanel.name));    // UI should call OnRoomListButtonClicked() to activate this
	}

	/// <summary>
	/// Logs the feedback in the UI view for the player, as opposed to inside the Unity Editor for the developer.
	/// </summary>
	/// <param name="message">Message.</param>
	private void LogFeedback(string message)
	{
		// we do not assume there is a feedbackText defined.
		if (feedbackText == null)
		{
			return;
		}

		if (feedbackText.text.Length > 10)
			statusVerticalBar.value -= 0.028f;

		// add new messages as a new line and at the bottom of the log.
		feedbackText.text += System.Environment.NewLine + message;
	}

	#endregion

	#region Private 함수

	private void ClearRoomListView()
	{
		foreach (GameObject entry in roomListEntries.Values)
		{
			Destroy(entry.gameObject);
		}

		roomListEntries.Clear();
	}

	private void UpdateCachedRoomList(List<RoomInfo> roomList)
	{
		foreach (RoomInfo info in roomList)
		{
			// Remove room from cached room list if it got closed, became invisible or was marked as removed
			if (!info.IsOpen || !info.IsVisible || info.RemovedFromList)
			{
				if (cachedRoomList.ContainsKey(info.Name))
				{
					cachedRoomList.Remove(info.Name);
				}

				continue;
			}

			// Update cached room info
			if (cachedRoomList.ContainsKey(info.Name))
			{
				cachedRoomList[info.Name] = info;
			}
			// Add new room info to cache
			else
			{
				cachedRoomList.Add(info.Name, info);
			}
		}
	}

	private void UpdateRoomListView()
	{
		foreach (RoomInfo info in cachedRoomList.Values)
		{
			GameObject entry = Instantiate(RoomListEntryPrefab);
			entry.transform.SetParent(RoomListContent.transform);
			entry.transform.localScale = Vector3.one;
			entry.GetComponent<RoomListEntry>().Initialize(info.Name, (byte)info.PlayerCount, info.MaxPlayers);

			roomListEntries.Add(info.Name, entry);
		}
	}

	#endregion

	#region UI 버튼 콜백 함수

	public void OnLogoutButtonClicked()
    {
		if (PhotonNetwork.IsConnected)
		{
			PhotonNetwork.Disconnect();
			PlayFabAuth.SetActive(false);
		}
	}

	public void OnBackButtonClicked()
	{
		if (PhotonNetwork.InLobby)
		{
			PhotonNetwork.LeaveLobby();
		}

		SetActivePanel(SelectionPanel.name);
	}

	public void OnCreateRoomButtonClicked()
	{
		if (loadingEffect)
			loadingEffect.StartLoaderAnimation();

		string roomName = RoomNameInputField.text;
		roomName = (roomName.Equals(string.Empty)) ? "Room " + Random.Range(1, 100) : roomName;

		PhotonNetwork.CreateRoom(roomName, new RoomOptions { MaxPlayers = maxPlayersPerRoom }, null);
	}

	public void OnJoinRandomRoomButtonClicked()
	{
		SetActivePanel(JoinRandomRoomPanel.name);

		if (loadingEffect)
			loadingEffect.StartLoaderAnimation();

		PhotonNetwork.JoinRandomRoom();
	}

	public void OnRoomListButtonClicked()
	{
		if (!PhotonNetwork.InLobby)
		{
			// Photon의 Lobby는 룸 목록을 보는 공간을 의미
			PhotonNetwork.JoinLobby();
		}

		SetActivePanel(RoomListPanel.name);
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
		// 최초로 마스터 서버에 연결됐을 때 콜백되는 함수
		// we don't want to do anything if we are not attempting to join a room. 
		// this case where isConnecting is false is typically when you lost or quit the game, when this level is loaded, OnConnectedToMaster will be called, in that case
		// we don't want to do anything.

		Debug.Log("<color=yellow>OnConnectedToMaster() 호출\n마스터 서버 연결됨</color>");

		if (loadingEffect)
			loadingEffect.StopLoaderAnimation();

		WebSyncPanel.SetActive(true);
		SetActivePanel(SelectionPanel.name);
	}

	public override void OnRoomListUpdate(List<RoomInfo> roomList)
	{
		Debug.Log("<color=yellow>OnRoomListUpdate() 호출\n방 목록 최신화</color>");

		ClearRoomListView();

		UpdateCachedRoomList(roomList);
		UpdateRoomListView();
	}

	// Photon의 Lobby는 룸 목록을 보는 공간을 의미
	public override void OnJoinedLobby()
	{
		// whenever this joins a new lobby, clear any previous room lists
		cachedRoomList.Clear();
		ClearRoomListView();
	}

	// note: when a client joins / creates a room, OnLeftLobby does not get called, even if the client was in a lobby before
	public override void OnLeftLobby()
	{
		cachedRoomList.Clear();
		ClearRoomListView();
	}

	public override void OnCreateRoomFailed(short returnCode, string message)
	{
		SetActivePanel(SelectionPanel.name);
	}

	public override void OnJoinRoomFailed(short returnCode, string message)
	{
		SetActivePanel(SelectionPanel.name);
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
		Debug.Log("<color=yellow>OnJoinRandomFailed() 호출\n입장 가능한 룸이 없어 새 룸을 만듭니다.</color>");

		string roomName = "Room " + Random.Range(1, 100);

		PhotonNetwork.CreateRoom(roomName, new RoomOptions { MaxPlayers = maxPlayersPerRoom }, null);
	}


	/// <summary>
	/// Called after disconnecting from the Photon server.
	/// </summary>
	public override void OnDisconnected(DisconnectCause cause)
	{
		LogFeedback("연결 해제됨");
		Debug.LogWarning("<color=yellow>Disconnected\n연결 해제됨</color>");

		WebSyncPanel.SetActive(false);
		SetActivePanel(LoginPanel.name);
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
		if (loadingEffect)
			loadingEffect.StopLoaderAnimation();

		LogFeedback("<Color=Green>OnJoinedRoom</Color> " + PhotonNetwork.CurrentRoom.PlayerCount + "명이 있는 룸에 입장합니다.");
		Debug.Log("<color=yellow>OnJoinedRoom() 호출\n이제 당신은 룸에 있습니다. 여기서 당신의 게임이 시작됩니다.</color>");

		PhotonNetwork.LoadLevel("Arrayment_Scene");
	}

	#endregion

}