using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlertMessage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void CantAttack(int team,int character)
    {
        StartCoroutine(CCantAttack(team,character));
    }

    IEnumerator CCantAttack(int team, int character)
    {
        this.GetComponent<Text>().text = team + "팀 " + character + "번 캐릭터가 사망하여 공격 할 수 없습니다!";

        yield return new WaitForSeconds(2.0f);

        this.gameObject.SetActive(false);
    }

    public void Attack(int team, int character)
    {
        StartCoroutine(CAttack(team,character));
    }

    IEnumerator CAttack(int team, int character)
    {
        this.GetComponent<Text>().text = team + "팀 " + character + "번 캐릭터 공격!";

        yield return new WaitForSeconds(2.0f);

        this.gameObject.SetActive(false);
    }

    public void Win()
    {
        StartCoroutine(CWin());
    }

    IEnumerator CWin()
    {
        this.GetComponent<Text>().text = "승리!";

        yield return new WaitForSeconds(2.0f);
    }

    public void Lose()
    {
        StartCoroutine(CLose());
    }

    IEnumerator CLose()
    {
        this.GetComponent<Text>().text = "패배..";

        yield return new WaitForSeconds(2.0f);
    }

    public void Counter(int team,int character)
    {
        StartCoroutine(CCounter(team,character));
    }
    
    IEnumerator CCounter(int team,int character)
    {
        this.GetComponent<Text>().text = team + "팀 " + character + "번 캐릭터 반격!";

        yield return new WaitForSeconds(2.0f);

        this.gameObject.SetActive(false);
    }
}
