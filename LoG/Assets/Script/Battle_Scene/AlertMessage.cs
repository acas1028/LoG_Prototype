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
        this.GetComponent<Text>().text = ACS.character_Team_Number + "팀 " + ACS.character_Number + "번 캐릭터가 사망하여 공격 할 수 없습니다!";
    }

    public void Attack(GameObject attacker)
    {
        Character ACS = attacker.GetComponent<Character>();
        this.GetComponent<Text>().text = ACS.character_Team_Number + "팀 " + ACS.character_Number + "번 캐릭터의 공격!";
    }

    public void Dead(GameObject character)
    {
        Character CCS = character.GetComponent<Character>();
        this.GetComponent<Text>().text = CCS.character_Team_Number + "팀 " + CCS.character_Number + "번 캐릭터 사망!";
    }

    public void Win()
    {
        this.GetComponent<Text>().text = "승리하였습니다! 잠시 후 재배치를 시작합니다..";
    }

    public void Lose()
    {
        this.GetComponent<Text>().text = "패배하였습니다. 잠시 후 재배치를 시작합니다..";
    }

    public void Draw()
    {
        this.GetComponent<Text>().text = "비겼습니다. 잠시 후 재배치를 시작합니다..";
    }

    public void Counter(GameObject Counter)
    {
        Character CCS = Counter.GetComponent<Character>();
        this.GetComponent<Text>().text = CCS.character_Team_Number + "팀 " + CCS.character_Number + "번 캐릭터 반격!";
    }
}
