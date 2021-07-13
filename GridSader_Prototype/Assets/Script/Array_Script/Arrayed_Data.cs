using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrayed_Data : MonoBehaviour
{
    public static GameObject instance;
    public GameObject[] team1;
    public GameObject[] team2;

    private void Awake()
    {
        // 이 오브젝트가 있는 씬에 왔을 때, 기존 오브젝트를 파괴하고 새로 만들어진 씬을 instance에 넣는다.
        // 때문에 라운드 종료 후 재배치를 하러 Arrayment_Scene에 왔을 때 기존 캐릭터 배치 정보를 가지고 올 수 있도록 수정이 필요하다.
        if (instance)
            Destroy(instance);
        instance = this.gameObject;

        DontDestroyOnLoad(this.gameObject);

        for (int i = 0; i < team1.Length; i++)
        {
            team1[i].GetComponent<Character_Script>().Character_Reset();
        }

        for (int i = 0; i < team2.Length; i++)
        {
            team2[i].GetComponent<Character_Script>().Character_Reset();
        }
    }
}
