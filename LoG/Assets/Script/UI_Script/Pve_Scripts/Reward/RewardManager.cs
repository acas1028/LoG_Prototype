using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardManager : MonoBehaviour
{
    private static RewardManager _instance;
    // 인스턴스에 접근하기 위한 프로퍼티
    public static RewardManager Instance
    {
        get
        {
            // 인스턴스가 없는 경우에 접근하려 하면 인스턴스를 할당해준다.
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
        // 인스턴스가 존재하는 경우 새로생기는 인스턴스를 삭제한다.
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    public List<Sprite> RewardSprites; // 0은 골드, 1은 처형자, 2는 무장해제, 특성은 언제나 변경가능.

    protected List<Dictionary<string, object>> reward_data; // 데이터 저장소
    // Start is called before the first frame update
    void Start()
    {
        reward_data = CSVReader.Read("PVE_Reward/PVE_Rewards");

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public Sprite SettingReward(int StageNumber) // 이부분 특성들은 수동으로 고쳐주어야함.
    {
        Sprite dummy = RewardSprites[0];

        Debug.Log(reward_data[0]["Value"]);

        if ((string)reward_data[StageNumber - 1]["Reward"] == "Gold")
            dummy = RewardSprites[0];

        if((string)reward_data[StageNumber - 1]["Reward"] == "Mastery")
        {
            if((string)reward_data[StageNumber - 1]["Value"] == "무장해제")
            {
                dummy = RewardSprites[1];
            }

            if ((string)reward_data[StageNumber - 1]["Value"] == "모아니면도")
            {
                dummy = RewardSprites[2];
            }

            if((string)reward_data[StageNumber - 1]["Value"] == "발악")
            {
                dummy = RewardSprites[3];
            }

            if ((string)reward_data[StageNumber - 1]["Value"] == "격려")
            {
                dummy = RewardSprites[4];
            }

            if ((string)reward_data[StageNumber - 1]["Value"] == "생존자")
            {
                dummy = RewardSprites[5];
            }

            if ((string)reward_data[StageNumber - 1]["Value"] == "겁쟁이")
            {
                dummy = RewardSprites[6];
            }

            if ((string)reward_data[StageNumber - 1]["Value"] == "저주")
            {
                dummy = RewardSprites[7];
            }

            if ((string)reward_data[StageNumber - 1]["Value"] == "보호막")
            {
                dummy = RewardSprites[8];
            }

            if ((string)reward_data[StageNumber - 1]["Value"] == "처형인")
            {
                dummy = RewardSprites[9];
            }
        }

        return dummy;
    }

    public string SettingRewardValue(int StageNumber) // 이부분도 수동으로 고치기.
    {
        string dum = "Default";
        if ((string)reward_data[StageNumber - 1]["Reward"] == "Gold")
        {
            dum = reward_data[StageNumber - 1]["Value"] + " 골드";
        }

        if ((string)reward_data[StageNumber - 1]["Reward"] == "Mastery")
        {

            if ((string)reward_data[StageNumber - 1]["Value"] == "무장해제")
            {
                dum = "무장해제 특성";
            }

            if ((string)reward_data[StageNumber - 1]["Value"] == "모아니면도")
            {
                dum = "모아니면도 특성";
            }

            if ((string)reward_data[StageNumber - 1]["Value"] == "발악")
            {
                dum = "발악 특성";
            }

            if ((string)reward_data[StageNumber - 1]["Value"] == "격려")
            {
                dum = "격려 특성";
            }

            if ((string)reward_data[StageNumber - 1]["Value"] == "생존자")
            {
                dum = "생존자 특성";
            }

            if ((string)reward_data[StageNumber - 1]["Value"] == "겁쟁이")
            {
                dum = "겁쟁이 특성";
            }

            if ((string)reward_data[StageNumber - 1]["Value"] == "저주")
            {
                dum = "저주 특성";
            }

            if ((string)reward_data[StageNumber - 1]["Value"] == "보호막")
            {
                dum = "보호막 특성";
            }

            if ((string)reward_data[StageNumber - 1]["Value"] == "처형인")
            {
                dum = "처형인 특성";
            }
        }

        return dum;
    }
}
