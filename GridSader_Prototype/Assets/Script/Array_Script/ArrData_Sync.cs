using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;

public class ArrData_Sync : MonoBehaviourPunCallbacks
{
    public GameObject[] team1;
    public GameObject[] team2;

    private Hashtable team1_table;
    private Hashtable isReady_table;

    public Text playerName;
    public Text roomStatusText;
    public Button readyButton;

    private bool isReady;


    private void Start()
    {
        // Ű Ÿ���� string��, �� Ÿ���� Character_Script������ �����ϴ� �ؽ����̺�
        // C# �ؽ����̺��� �⺻������ ���� �ؽ� ����� ����Ͽ� �����͸� �����Ѵ�.
        // �ؽ����̺��� ����ϴ� ������ Photon�� Custom Properties�� ����Ϸ��� Hashtable�� ����ؾ��ϱ� ����
        team1_table = new Hashtable();
        isReady_table = new Hashtable();
        isReady_table.Add("PlayerIsReady", false);
        PhotonNetwork.LocalPlayer.SetCustomProperties(isReady_table);

        for (int i = 0; i < 5; i++)
        {
            team1_table.Add((i + 1) + "_AttackRange", null);
            team1_table.Add((i + 1) + "_GridNumber", null);
            team1_table.Add((i + 1) + "_Damage", null);
            team1_table.Add((i + 1) + "_HP", null);
            team1_table.Add((i + 1) + "_AttackOrder", null);
        }

        playerName.text = PhotonNetwork.LocalPlayer.NickName;
        roomStatusText.text = " ";

        isReady = false;
    }

    // Start is called before the first frame update

    // Arrayment_Scene���� Ready ��ư�� ���� ��� ȣ����
    // Custom Properties �� �̿��Ͽ� ������ Team1�� Character_Script�� ����
    // https://doc.photonengine.com/ko-kr/pun/current/gameplay/synchronization-and-state : ����ȭ�ϴ� ��� 1.PhotonView 2.RPC 3.Custom Properties
    // https://doc.photonengine.com/ko-kr/pun/current/reference/serialization-in-photon : ������ �� �ִ� ������ Ÿ��
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
            team1_table[(i + 1) + "_AttackRange"] = cs.character_Attack_Range;
            team1_table[(i + 1) + "_AttackRange"] = cs.character_Attack_Range;
            team1_table[(i + 1) + "_GridNumber"] = cs.character_Num_Of_Grid;
            team1_table[(i + 1) + "_Damage"] = cs.character_Attack_Damage;
            team1_table[(i + 1) + "_HP"] = cs.character_HP;
            team1_table[(i + 1) + "_AttackOrder"] = cs.character_Attack_Order;

            Debug.Log("���� �׸��� �ѹ�: " + cs.character_Num_Of_Grid);
        }
        result = PhotonNetwork.LocalPlayer.SetCustomProperties(team1_table);
        if (!result)
            Debug.Log("Team1 Custom Property ���� ����");
        
        result = PhotonNetwork.LocalPlayer.SetCustomProperties(isReady_table);
        if (!result)
            Debug.Log("IsReady Custom Property ���� ����");
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    #region ���� �ݹ� �Լ� : MonoBehaviourPunCallbacks Ŭ������ ����� �޴� �Լ�

    /// <summary>
    /// SetCustomProperties�� ���� �� �÷��̾��� Custom Property�� �ٲ� ��� ȣ��Ǵ� �ݹ� �Լ�
    /// </summary>
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        object o_isEnemyReady;
        bool isEnemyReady = false;
        bool isAllPlayerReady;

        object o_attackRange;
        object o_gridNumber;
        object o_damage;
        object o_hp;
        object o_attackOrder;
        Character_Script cs;

        // ��� �÷��̾ �غ�Ǿ����� ���θ� �޾ƿ��� ����
        if (targetPlayer != PhotonNetwork.LocalPlayer)
        {
            targetPlayer.CustomProperties.TryGetValue("PlayerIsReady", out o_isEnemyReady);
            isEnemyReady = (bool)o_isEnemyReady;
        }
        Debug.LogWarning("isEnemyReady = " + isEnemyReady);

        // ��밡 �غ� ��ư�� ������ ����
        if (isEnemyReady && targetPlayer != PhotonNetwork.LocalPlayer)
        {
            for (int i = 0; i < team2.Length; i++)
            {
                // ������ �ִ� Team2�� Character_Script ������ ���� team2�� �����ϴ� ����

                // ��밡 �������� �ʾҰų�, Ready ��ư�� ������ ���� ���¿����� ������Ʈ�� ������ �� �����Ƿ� return ó��
                cs = team2[i].GetComponent<Character_Script>();
                if (!cs)
                    return;

                targetPlayer.CustomProperties.TryGetValue((i + 1) + "_AttackRange", out o_attackRange);
                targetPlayer.CustomProperties.TryGetValue((i + 1) + "_GridNumber", out o_gridNumber);
                targetPlayer.CustomProperties.TryGetValue((i + 1) + "_Damage", out o_damage);
                targetPlayer.CustomProperties.TryGetValue((i + 1) + "_HP", out o_hp);
                targetPlayer.CustomProperties.TryGetValue((i + 1) + "_AttackOrder", out o_attackOrder);

                Debug.Log("���� �׸��� �ѹ�: " + (int)o_gridNumber);

                cs.character_Attack_Range = (bool[])o_attackRange;
                cs.character_Num_Of_Grid = (int)o_gridNumber;
                cs.character_Attack_Damage = (int)o_damage;
                cs.character_HP = (int)o_hp;
                cs.character_Attack_Order = (int)o_attackOrder;

                cs.Debuging_Character();
            }
        }

        isAllPlayerReady = isReady && isEnemyReady;

        // ������ ������ �����Ѵ�.
        if (PhotonNetwork.IsMasterClient && isAllPlayerReady)
            SceneManager.LoadScene("BattleScene");
    }

    public override void OnPlayerEnteredRoom(Player other)
    {
        roomStatusText.text = "��� �÷��̾� " + other.NickName + " ����";
        Debug.Log("�÷��̾� " + other.NickName + " ����");
    }

    public override void OnPlayerLeftRoom(Player other)
    {
        roomStatusText.text = "��� �÷��̾� " + other.NickName + " ����";
        Debug.Log("�÷��̾� " + other.NickName + " ����");
    }

    public override void OnLeftRoom()
    {
        Debug.Log("���� �����ϴ�. �κ�� �̵��մϴ�.");
        SceneManager.LoadScene("LobbyScene");
    }
    #endregion
}
