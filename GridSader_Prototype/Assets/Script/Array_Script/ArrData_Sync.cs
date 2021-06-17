using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;

public class ArrData_Sync : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject[] team1;
    [SerializeField]
    private GameObject[] team2;

    private Hashtable team1_table;
    private Hashtable isReady_table;

    public Text playerName;
    public Text roomStatusText;

    private bool isReady;
    private bool stopGetEnemyData;


    private void Start()
    {
        // 키 타입은 string형, 값 타입은 Character_Script형으로 저장하는 해시테이블
        // C# 해시테이블은 기본적으로 이중 해싱 방법을 사용하여 데이터를 저장한다.
        // 해시테이블을 사용하는 이유는 Photon의 Custom Properties를 사용하려면 Hashtable을 사용해야하기 때문
        team1_table = new Hashtable();
        isReady_table = new Hashtable();
        isReady_table.Add("PlayerIsReady", false);
        PhotonNetwork.LocalPlayer.SetCustomProperties(isReady_table);

        playerName.text = PhotonNetwork.LocalPlayer.NickName;
        roomStatusText.text = " ";

        isReady = false;
        stopGetEnemyData = false;
    }

    private void Update()
    {
        object o_isEnemyReady;
        bool isEnemyReady;

        object o_attackRange;
        object o_gridNumber;
        object o_damage;
        object o_hp;
        object o_attackOrder;
        Character_Script cs;

        // 한 룸에서 2명이 최대이므로 나를 제외한 플레이어는 1명이니 루프횟수는 1번
        foreach (Player player in PhotonNetwork.PlayerListOthers)
        {
            // 상대 플레이어가 준비되었는지 여부를 받아오는 과정
            player.CustomProperties.TryGetValue("PlayerIsReady", out o_isEnemyReady);
            isEnemyReady = (bool)o_isEnemyReady;

            if (!isEnemyReady || stopGetEnemyData)
                return;

            for (int i = 0; i < team2.Length; i++)
            {
                // 서버에 있는 Team2의 Character_Script 정보를 여기 team2에 저장하는 과정

                // 상대가 접속하지 않았거나, Ready 버튼을 누르지 않은 상태에서는 컴포넌트를 가져올 수 없으므로 return 처리
                cs = team2[i].GetComponent<Character_Script>();
                if (!cs)
                    return;

                player.CustomProperties.TryGetValue((i + 1) + "_AttackRange", out o_attackRange);
                player.CustomProperties.TryGetValue((i + 1) + "_GridNumber", out o_gridNumber);
                player.CustomProperties.TryGetValue((i + 1) + "_Damage", out o_damage);
                player.CustomProperties.TryGetValue((i + 1) + "_HP", out o_hp);
                player.CustomProperties.TryGetValue((i + 1) + "_AttackOrder", out o_attackOrder);

                Debug.Log("받은 그리드 넘버: " + (int)o_gridNumber);

                cs.character_Attack_Range = (bool[])o_attackRange;
                cs.character_Num_Of_Grid = (int)o_gridNumber;
                cs.character_Attack_Damage = (int)o_damage;
                cs.character_HP = (int)o_hp;
                cs.character_Attack_Order = (int)o_attackOrder;

                cs.Debuging_Character();
            }

            stopGetEnemyData = true;
        }
    }

    // Start is called before the first frame update

    // Arrayment_Scene에서 Ready 버튼을 누른 즉시 호출함
    // Custom Properties 를 이용하여 서버에 Team1의 Character_Script를 전송
    // https://doc.photonengine.com/ko-kr/pun/current/gameplay/synchronization-and-state : 동기화하는 방법 1. RPC 2. Custom Properties
    // https://doc.photonengine.com/ko-kr/pun/current/reference/serialization-in-photon : 전송할 수 있는 데이터 타입
    public void DataSync(GameObject[] array_team)
    {
        bool result = false;
        team1 = array_team;
        Character_Script cs;
        isReady = true;

        isReady_table["PlayerIsReady"] = isReady;

        for (int i = 0; i < array_team.Length; i++)
        {
            cs = team1[i].GetComponent<Character_Script>();
            team1_table.Add((i + 1) + "_AttackRange", cs.character_Attack_Range);
            team1_table.Add((i + 1) + "_GridNumber", cs.character_Num_Of_Grid);
            team1_table.Add((i + 1) + "_Damage", cs.character_Attack_Damage);
            team1_table.Add((i + 1) + "_HP", cs.character_HP);
            team1_table.Add((i + 1) + "_AttackOrder", cs.character_Attack_Order);

            Debug.Log("보낸 그리드 넘버: " + cs.character_Num_Of_Grid);
        }
        result = PhotonNetwork.LocalPlayer.SetCustomProperties(team1_table);
        if (!result)
            Debug.Log("Team1 Custom Property 설정 실패");
        
        result = PhotonNetwork.LocalPlayer.SetCustomProperties(isReady_table);
        if (!result)
            Debug.Log("IsReady Custom Property 설정 실패");
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    #region 포톤 콜백 함수 : MonoBehaviourPunCallbacks 클래스의 상속을 받는 함수
    public override void OnPlayerEnteredRoom(Player other)
    {
        roomStatusText.text = "상대 플레이어 " + other.NickName + " 입장";
        Debug.Log("플레이어 " + other.NickName + " 입장");
    }

    public override void OnPlayerLeftRoom(Player other)
    {
        roomStatusText.text = "상대 플레이어 " + other.NickName + " 퇴장";
        Debug.Log("플레이어 " + other.NickName + " 퇴장");
    }

    public override void OnLeftRoom()
    {
        Debug.Log("룸을 나갑니다. 로비로 이동합니다.");
        SceneManager.LoadScene("LobbyScene");
    }
    #endregion
}
