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
        // Ű Ÿ���� string��, �� Ÿ���� Character_Script������ �����ϴ� �ؽ����̺�
        // C# �ؽ����̺��� �⺻������ ���� �ؽ� ����� ����Ͽ� �����͸� �����Ѵ�.
        // �ؽ����̺��� ����ϴ� ������ Photon�� Custom Properties�� ����Ϸ��� Hashtable�� ����ؾ��ϱ� ����
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

        // �� �뿡�� 2���� �ִ��̹Ƿ� ���� ������ �÷��̾�� 1���̴� ����Ƚ���� 1��
        foreach (Player player in PhotonNetwork.PlayerListOthers)
        {
            for (int i = 0; i < team2.Length; i++)
            {
                // ������ �ִ� Team2�� Character_Script ������ ���� team2�� �����ϴ� ����
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
                    Debug.Log("�� ĳ���� " + (i + 1) + " �� ������ ���� ����");
            }
        }
    }

    // Start is called before the first frame update

    // Arrayment_Scene���� Ready ��ư�� ���� ��� ȣ����
    // Custom Properties �� �̿��Ͽ� ������ Team1�� Character_Script�� ����
    //https://doc.photonengine.com/ko-kr/pun/current/gameplay/synchronization-and-state : ����ȭ�ϴ� ��� 1. RPC 2. Custom Properties
    //https://doc.photonengine.com/ko-kr/pun/current/reference/serialization-in-photon : ������ �� �ִ� ������ Ÿ��
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
            Debug.Log("Team1 Custom Property ���� ����");
    }
}
