using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardManager : MonoBehaviour
{
    private static RewardManager _instance;
    // �ν��Ͻ��� �����ϱ� ���� ������Ƽ
    public static RewardManager Instance
    {
        get
        {
            // �ν��Ͻ��� ���� ��쿡 �����Ϸ� �ϸ� �ν��Ͻ��� �Ҵ����ش�.
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(RewardManager)) as RewardManager;

                if (_instance == null)
                    Debug.Log("no Singleton obj");
            }
            return _instance;
        }
    }
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        // �ν��Ͻ��� �����ϴ� ��� ���λ���� �ν��Ͻ��� �����Ѵ�.
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    public List<Sprite> RewardSprites; // 0�� ���, 1�� ó����, 2�� ��������, Ư���� ������ ���氡��.

    protected List<Dictionary<string, object>> reward_data; // ������ �����
    // Start is called before the first frame update
    void Start()
    {
        reward_data = CSVReader.Read("PVE_Reward/PVE_Rewards");

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public Sprite SettingReward(int StageNumber) // �̺κ� Ư������ �������� �����־����.
    {
        Sprite dummy = RewardSprites[0];

        Debug.Log(reward_data[0]["Value"]);

        if ((string)reward_data[StageNumber - 1]["Reward"] == "Gold")
            dummy = RewardSprites[0];

        if((string)reward_data[StageNumber - 1]["Reward"] == "Mastery")
        {
            if((string)reward_data[StageNumber - 1]["Value"] == "��������")
            {
                dummy = RewardSprites[1];
            }

            if ((string)reward_data[StageNumber - 1]["Value"] == "��ƴϸ鵵")
            {
                dummy = RewardSprites[2];
            }

            if((string)reward_data[StageNumber - 1]["Value"] == "�߾�")
            {
                dummy = RewardSprites[3];
            }

            if ((string)reward_data[StageNumber - 1]["Value"] == "�ݷ�")
            {
                dummy = RewardSprites[4];
            }

            if ((string)reward_data[StageNumber - 1]["Value"] == "������")
            {
                dummy = RewardSprites[5];
            }

            if ((string)reward_data[StageNumber - 1]["Value"] == "������")
            {
                dummy = RewardSprites[6];
            }

            if ((string)reward_data[StageNumber - 1]["Value"] == "����")
            {
                dummy = RewardSprites[7];
            }

            if ((string)reward_data[StageNumber - 1]["Value"] == "��ȣ��")
            {
                dummy = RewardSprites[8];
            }

            if ((string)reward_data[StageNumber - 1]["Value"] == "ó����")
            {
                dummy = RewardSprites[9];
            }
        }

        return dummy;
    }

    public string SettingRewardValue(int StageNumber) // �̺κе� �������� ��ġ��.
    {
        string dum = "Default";
        if ((string)reward_data[StageNumber - 1]["Reward"] == "Gold")
        {
            dum = reward_data[StageNumber - 1]["Value"] + " ���";
        }

        if ((string)reward_data[StageNumber - 1]["Reward"] == "Mastery")
        {

            if ((string)reward_data[StageNumber - 1]["Value"] == "��������")
            {
                dum = "�������� Ư��";
            }

            if ((string)reward_data[StageNumber - 1]["Value"] == "��ƴϸ鵵")
            {
                dum = "��ƴϸ鵵 Ư��";
            }

            if ((string)reward_data[StageNumber - 1]["Value"] == "�߾�")
            {
                dum = "�߾� Ư��";
            }

            if ((string)reward_data[StageNumber - 1]["Value"] == "�ݷ�")
            {
                dum = "�ݷ� Ư��";
            }

            if ((string)reward_data[StageNumber - 1]["Value"] == "������")
            {
                dum = "������ Ư��";
            }

            if ((string)reward_data[StageNumber - 1]["Value"] == "������")
            {
                dum = "������ Ư��";
            }

            if ((string)reward_data[StageNumber - 1]["Value"] == "����")
            {
                dum = "���� Ư��";
            }

            if ((string)reward_data[StageNumber - 1]["Value"] == "��ȣ��")
            {
                dum = "��ȣ�� Ư��";
            }

            if ((string)reward_data[StageNumber - 1]["Value"] == "ó����")
            {
                dum = "ó���� Ư��";
            }
        }

        return dum;
    }
}
