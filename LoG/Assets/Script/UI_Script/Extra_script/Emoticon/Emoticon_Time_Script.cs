using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emoticon_Time_Script : MonoBehaviour
{
    private void Start()
    {
        this.gameObject.GetComponent<Animator>().Play("Emoticon_Click");
        StartCoroutine(Animation_Off());
    }

    IEnumerator Animation_Off() //이모티콘이 3초 화면이상 비춰지자 이를 꺼트리는 함수
    {
        yield return new WaitForSeconds(3.0f);

        this.gameObject.GetComponent<Animator>().Play("Emoticon_Click_Disappear");
        yield return new WaitForSeconds(1.0f);

        Destroy(this.gameObject);
    }
}