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
    
    public void CantAttack(GameObject attacker)
    {
        StartCoroutine(CCantAttack(attacker));
    }

    IEnumerator CCantAttack(GameObject attacker)
    {
        Character ACS = attacker.GetComponent<Character>();
        this.GetComponent<Text>().text = ACS.character_Team_Number + "�� " + ACS.character_Number + "�� ĳ���Ͱ� ����Ͽ� ���� �� �� �����ϴ�!";

        yield return new WaitForSeconds(2.0f);

        this.gameObject.SetActive(false);
    }

    public void Attack(GameObject attacker)
    {
        StartCoroutine(CAttack(attacker));
    }

    IEnumerator CAttack(GameObject attacker)
    {
        Character ACS = attacker.GetComponent<Character>();
        this.GetComponent<Text>().text = ACS.character_Team_Number + "�� " + ACS.character_Number + "�� ĳ���� ����!";

        yield return new WaitForSeconds(2.0f);

        this.gameObject.SetActive(false);
    }

    public void Win()
    {
        StartCoroutine(CWin());
    }

    IEnumerator CWin()
    {
        this.GetComponent<Text>().text = "�¸�!";

        yield return new WaitForSeconds(2.0f);
    }

    public void Lose()
    {
        StartCoroutine(CLose());
    }

    IEnumerator CLose()
    {
        this.GetComponent<Text>().text = "�й�..";

        yield return new WaitForSeconds(2.0f);
    }

    public void Counter(GameObject Counter)
    {
        Character CCS = Counter.GetComponent<Character>();
        StartCoroutine(CCounter(Counter));
    }
    
    IEnumerator CCounter(GameObject Counter)
    {
        Character CCS = Counter.GetComponent<Character>();
        this.GetComponent<Text>().text = CCS.character_Team_Number + "�� " + CCS.character_Number + "�� ĳ���� �ݰ�!";

        yield return new WaitForSeconds(2.0f);

        this.gameObject.SetActive(false);
    }
}
