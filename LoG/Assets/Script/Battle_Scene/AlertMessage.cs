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

    public void Message(string message)
    {
        this.GetComponent<Text>().text = message;
    }
    
    public void CantAttack(GameObject attacker)
    {
        Character ACS = attacker.GetComponent<Character>();
        this.GetComponent<Text>().text = ACS.character_Team_Number + "�� " + ACS.character_Number + "�� ĳ���Ͱ� ����Ͽ� ���� �� �� �����ϴ�!";
    }

    public void Attack(GameObject attacker)
    {
        Character ACS = attacker.GetComponent<Character>();
        this.GetComponent<Text>().text = ACS.character_Team_Number + "�� " + ACS.character_Number + "�� ĳ������ ����!";
    }

    public void Win()
    {
        this.GetComponent<Text>().text = "�¸�!";
    }


    public void Lose()
    {
        this.GetComponent<Text>().text = "�й�..";
    }

    public void Counter(GameObject Counter)
    {
        Character CCS = Counter.GetComponent<Character>();
        this.GetComponent<Text>().text = CCS.character_Team_Number + "�� " + CCS.character_Number + "�� ĳ���� �ݰ�!";
    }
}
