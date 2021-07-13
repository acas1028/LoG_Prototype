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
        // �� ������Ʈ�� �ִ� ���� ���� ��, ���� ������Ʈ�� �ı��ϰ� ���� ������� ���� instance�� �ִ´�.
        // ������ ���� ���� �� ���ġ�� �Ϸ� Arrayment_Scene�� ���� �� ���� ĳ���� ��ġ ������ ������ �� �� �ֵ��� ������ �ʿ��ϴ�.
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
