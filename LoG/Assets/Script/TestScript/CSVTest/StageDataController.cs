using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageDataController : MonoBehaviour
{
    public GameObject[] PlayerCharacters;
    public GameObject[] EnemyCharacters;

    public Arrayed_Data ArrayData;

    public string StageName;


    // Start is called before the first frame update

    private void Start()
    {
        StageName = "PVE_Stage/PVE_Character_Stage" + CSVManager.StageNumber.ToString();
        for(int i=0;i<5;i++)
        {
            EnemyCharacters[i] = Arrayed_Data.instance.team2[i];
        }
        if (ArrayData)
        {
            SettingCharacter();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SettingCharacter()
    {
        for(int i = 0; i < PlayerCharacters.Length; i++)
        {
            PlayerCharacters[i].GetComponent<Character>().PVE_Player_Character_Setting(i + 5, StageName);
        }

        for(int j = 0; j < EnemyCharacters.Length; j++)
        {
            EnemyCharacters[j].GetComponent<Character>().PVE_Enemy_Character_Setting(j, StageName);
        }
    }
}
