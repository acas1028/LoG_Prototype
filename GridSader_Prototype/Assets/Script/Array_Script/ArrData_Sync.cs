using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;

public class ArrData_Sync : MonoBehaviourPunCallbacks
{
    public static ArrData_Sync instance = null;
    public GameObject[] team1;
    public GameObject[] team2;

    private Hashtable team1_table;
    private Hashtable isReady_table;

    public Text playerName;
    public Text roomStatusText;
    public Button readyButton;

    private bool isReady;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        // 키 타입은 string형, 값 타입은 Character_Script형으로 저장하는 해시테이블
        // C# 해시테이블은 기본적으로 이중 해싱 방법을 사용하여 데이터를 저장한다.
        // 해시테이블을 사용하는 이유는 Photon의 Custom Properties를 사용하려면 Hashtable을 사용해야하기 때문
        team1_table = new Hashtable();
        isReady_table = new Hashtable();
        isReady_table.Add("PlayerIsReady", false);
        PhotonNetwork.LocalPlayer.SetCustomProperties(isReady_table);

        for (int i = 0; i < 5; i++)
        {
            team1_table.Add((i + 1) + "_ID", null);
            team1_table.Add((i + 1) + "_IsAlive", null);
            team1_table.Add((i + 1) + "_HP", null);
            team1_table.Add((i + 1) + "_AP", null);
            team1_table.Add((i + 1) + "_AttackDamage", null);
            team1_table.Add((i + 1) + "_AttackRange", null);
            team1_table.Add((i + 1) + "_GridNumber", null);
            team1_table.Add((i + 1) + "_AttackOrder", null);
            team1_table.Add((i + 1) + "_AttackCount", null);
            team1_table.Add((i + 1) + "_Damaged", null);
            team1_table.Add((i + 1) + "_BuffedAttack", null);
            team1_table.Add((i + 1) + "_BuffedDamaged", null);
            team1_table.Add((i + 1) + "_DivineShield", null);
            team1_table.Add((i + 1) + "_Revivial", null);
        }

        playerName.text = PhotonNetwork.LocalPlayer.NickName;
        roomStatusText.text = " ";

        isReady = false;
    }

    // Start is called before the first frame update

    // Arrayment_Scene에서 Ready 버튼을 누른 즉시 호출함
    // Custom Properties 를 이용하여 서버에 Team1의 Character_Script를 전송
    // https://doc.photonengine.com/ko-kr/pun/current/gameplay/synchronization-and-state : 동기화하는 방법 1.PhotonView 2.RPC 3.Custom Properties
    // https://doc.photonengine.com/ko-kr/pun/current/reference/serialization-in-photon : 전송할 수 있는 데이터 타입
    public void DataSync(GameObject[] array_team)
    {
        bool result = false;
        team1 = array_team;
        Character_Script cs;
        isReady = !isReady;

        ColorBlock cb = readyButton.colors;
        cb.normalColor = new Color(isReady ? 1.0f : 0.0f, 0.0f, 0.0f);
        cb.selectedColor = new Color(isReady ? 1.0f : 0.0f, 0.0f, 0.0f);
        readyButton.colors = cb;

        isReady_table["PlayerIsReady"] = isReady;

        for (int i = 0; i < array_team.Length; i++)
        {
            cs = team1[i].GetComponent<Character_Script>();
            team1_table[(i + 1) + "_ID"] = cs.character_ID;
            team1_table[(i + 1) + "_IsAlive"] = cs.character_Is_Allive;
            team1_table[(i + 1) + "_HP"] = cs.character_HP;
            team1_table[(i + 1) + "_AP"] = cs.character_AP;
            team1_table[(i + 1) + "_AttackDamage"] = cs.character_Attack_Damage;
            team1_table[(i + 1) + "_AttackRange"] = cs.character_Attack_Range;
            team1_table[(i + 1) + "_GridNumber"] = cs.character_Num_Of_Grid;
            team1_table[(i + 1) + "_AttackOrder"] = cs.character_Attack_Order;
            team1_table[(i + 1) + "_AttackCount"] = cs.character_Attack_Count;
            team1_table[(i + 1) + "_Damaged"] = cs.character_Damaged;
            team1_table[(i + 1) + "_BuffedAttack"] = cs.character_Buffed_Attack;
            team1_table[(i + 1) + "_BuffedDamaged"] = cs.character_Buffed_Damaged;
            team1_table[(i + 1) + "_DivineShield"] = cs.character_Divine_Shield;
            team1_table[(i + 1) + "_Revivial"] = cs.character_Revivial;

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

    /// <summary>
    /// SetCustomProperties를 통해 각 플레이어의 Custom Property가 바뀐 경우 호출되는 콜백 함수
    /// </summary>
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        object o_isEnemyReady;
        bool isEnemyReady = false;
        bool isAllPlayerReady;

        object o_id;
        object o_isAlive;
        object o_hp;
        object o_ap;
        object o_attackDamage;
        object o_attackRange;
        object o_gridNumber;
        object o_attackOrder;
        object o_attackCount;
        object o_damaged;
        object o_buffedAttack;
        object o_buffedDamaged;
        object o_divineShield;
        object o_revivial;
        Character_Script cs;

        // 상대 플레이어가 준비되었는지 여부를 받아오는 과정
        if (targetPlayer != PhotonNetwork.LocalPlayer)
        {
            targetPlayer.CustomProperties.TryGetValue("PlayerIsReady", out o_isEnemyReady);
            isEnemyReady = (bool)o_isEnemyReady;
        }
        Debug.LogWarning("isEnemyReady = " + isEnemyReady);

        // 상대가 준비 버튼을 눌렀을 때만
        if (isEnemyReady && targetPlayer != PhotonNetwork.LocalPlayer)
        {
            for (int i = 0; i < team2.Length; i++)
            {
                // 서버에 있는 Team2의 Character_Script 정보를 여기 team2에 저장하는 과정

                // 상대가 접속하지 않았거나, Ready 버튼을 누르지 않은 상태에서는 컴포넌트를 가져올 수 없으므로 return 처리
                cs = team2[i].GetComponent<Character_Script>();
                if (!cs)
                    return;

                targetPlayer.CustomProperties.TryGetValue((i + 1) + "_ID", out o_id);
                targetPlayer.CustomProperties.TryGetValue((i + 1) + "_IsAlive", out o_isAlive);
                targetPlayer.CustomProperties.TryGetValue((i + 1) + "_HP", out o_hp);
                targetPlayer.CustomProperties.TryGetValue((i + 1) + "_AP", out o_ap);
                targetPlayer.CustomProperties.TryGetValue((i + 1) + "_AttackDamage", out o_attackDamage);
                targetPlayer.CustomProperties.TryGetValue((i + 1) + "_AttackRange", out o_attackRange);
                targetPlayer.CustomProperties.TryGetValue((i + 1) + "_GridNumber", out o_gridNumber);
                targetPlayer.CustomProperties.TryGetValue((i + 1) + "_AttackOrder", out o_attackOrder);
                targetPlayer.CustomProperties.TryGetValue((i + 1) + "_AttackCount", out o_attackCount);
                targetPlayer.CustomProperties.TryGetValue((i + 1) + "_Damaged", out o_damaged);
                targetPlayer.CustomProperties.TryGetValue((i + 1) + "_BuffedAttack", out o_buffedAttack);
                targetPlayer.CustomProperties.TryGetValue((i + 1) + "_BuffedDamaged", out o_buffedDamaged);
                targetPlayer.CustomProperties.TryGetValue((i + 1) + "_DivineShield", out o_divineShield);
                targetPlayer.CustomProperties.TryGetValue((i + 1) + "_Revivial", out o_revivial);

                Debug.Log("받은 그리드 넘버: " + (int)o_gridNumber);

                cs.character_ID = (int)o_id;
                cs.character_Is_Allive = (bool)o_isAlive;
                cs.character_HP = (int)o_hp;
                cs.character_AP = (int)o_ap;
                cs.character_Attack_Damage = (int)o_attackDamage;
                cs.character_Attack_Range = (bool[])o_attackRange;
                cs.character_Num_Of_Grid = (int)o_gridNumber;
                cs.character_Attack_Order = (int)o_attackOrder;
                cs.character_Attack_Count = (int)o_attackCount;
                cs.character_Damaged = (int)o_damaged;
                cs.character_Buffed_Attack = (int)o_buffedAttack;
                cs.character_Buffed_Damaged = (int)o_buffedDamaged;
                cs.character_Divine_Shield = (bool)o_divineShield;
                cs.character_Revivial = (bool)o_revivial;

                cs.Debuging_Character();
            }
        }

        isAllPlayerReady = isReady && isEnemyReady;

        // 방장이 게임을 시작한다.
        if (PhotonNetwork.IsMasterClient && isAllPlayerReady)
            SceneManager.LoadScene("BattleScene");
    }

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
