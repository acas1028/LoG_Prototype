using UnityEngine;

using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;

public class ArrData_Sync : MonoBehaviour
{
    [SerializeField]
    private GameObject[] team1;
    [SerializeField]
    private GameObject[] team2;

    private Hashtable team1_character;
    private Hashtable team2_character;

    private void Start()
    {
        // 키 타입은 string형, 값 타입은 Character_Script형으로 저장하는 해시테이블
        // C# 해시테이블은 기본적으로 이중 해싱 방법을 사용하여 데이터를 저장한다.
        // 해시테이블을 사용하는 이유는 Photon의 Custom Properties를 사용하려면 Hashtable을 사용해야하기 때문
        team1_character = new Hashtable();
    }

    private void Update()
    {
        object o_attackRange;
        object o_gridNumber;
        object o_damage;
        object o_hp;
        object o_attackOrder;
        Character_Script cs;

        // 한 룸에서 2명이 최대이므로 나를 제외한 플레이어는 1명이니 루프횟수는 1번
        foreach (Player player in PhotonNetwork.PlayerListOthers)
        {
            for (int i = 0; i < team2.Length; i++)
            {
                // 서버에 있는 Team2의 Character_Script 정보를 여기 team2에 저장하는 과정
                player.CustomProperties.TryGetValue("team_" + (i + 1) + "_AttackRange", out o_attackRange);
                player.CustomProperties.TryGetValue("team_" + (i + 1) + "_GridNumber", out o_gridNumber);
                player.CustomProperties.TryGetValue("team_" + (i + 1) + "_Damage", out o_damage);
                player.CustomProperties.TryGetValue("team_" + (i + 1) + "_HP", out o_hp);
                player.CustomProperties.TryGetValue("team_" + (i + 1) + "_AttackOrder", out o_attackOrder);

                cs = team2[i].GetComponent<Character_Script>();
                cs.Debug_character_Attack_Range = (bool[])o_attackRange;
                cs.Debug_character_Grid_Number = (int)o_gridNumber;
                cs.Debug_Character_Damage = (int)o_damage;
                cs.Debug_Character_HP = (int)o_hp;
                cs.Debug_Character_Attack_order = (int)o_attackOrder;
                if (!cs)
                    Debug.Log("적 캐릭터 " + (i + 1) + " 의 정보를 받지 못함");
            }
        }
    }

    // Start is called before the first frame update

    // Arrayment_Scene에서 Ready 버튼을 누른 즉시 호출함
    // Custom Properties 를 이용하여 서버에 Team1의 Character_Script를 전송
    //https://doc.photonengine.com/ko-kr/pun/current/gameplay/synchronization-and-state : 동기화하는 방법 1. RPC 2. Custom Properties
    //https://doc.photonengine.com/ko-kr/pun/current/reference/serialization-in-photon : 전송할 수 있는 데이터 타입
    public void DataSync(GameObject[] array_team)
    {
        bool result = false;
        team1 = array_team;
        Character_Script cs;

        for (int i = 0; i < array_team.Length; i++)
        {
            cs = team1[i].GetComponent<Character_Script>();
            team1_character.Add("team_" + (i+1) + "_AttackRange", cs.Debug_character_Attack_Range);
            team1_character.Add("team_" + (i+1) + "_GridNumber", cs.Debug_character_Grid_Number);
            team1_character.Add("team_" + (i+1) + "_Damage", cs.Debug_Character_Damage);
            team1_character.Add("team_" + (i+1) + "_HP", cs.Debug_Character_HP);
            team1_character.Add("team_" + (i+1) + "_AttackOrder", cs.Debug_Character_Attack_order);
        }
        result = PhotonNetwork.LocalPlayer.SetCustomProperties(team1_character);
        if (!result)
            Debug.Log("Team1 Custom Property 설정 실패");
    }
}
