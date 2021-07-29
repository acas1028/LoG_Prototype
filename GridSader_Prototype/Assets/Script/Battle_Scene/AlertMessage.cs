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
        Invoke("Disable", BattleManager.Instance.bM_Timegap);
    }
    
    public void CantAttack(GameObject attacker)
    {
        Character ACS = attacker.GetComponent<Character>();
        this.GetComponent<Text>().text = ACS.character_Team_Number + "�� " + ACS.character_Number + "�� ĳ���Ͱ� ����Ͽ� ���� �� �� �����ϴ�!";

        Invoke("Disable", BattleManager.Instance.bM_Timegap);
    }

    public void Attack(GameObject attacker)
    {
        Character ACS = attacker.GetComponent<Character>();
        this.GetComponent<Text>().text = ACS.character_Team_Number + "�� " + ACS.character_Number + "�� ĳ���� ����!";

        Invoke("Disable", BattleManager.Instance.bM_Timegap);
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

        Invoke("Disable", BattleManager.Instance.bM_Timegap);
    }

    private void Disable()
    {
        this.gameObject.SetActive(false);
    }
}
