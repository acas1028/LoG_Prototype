using System.Collections;
using UnityEngine;


// 캐릭터의 스탯을 기반으로 특정 행동을 취하는 클래스
public class Character_Action : Character
{
    private Vector3 startPosition;
    private bool isMoveToEnemy;
    private Transform enemyTransform;
    private Vector3 velocity;

    private void Start()
    {
        startPosition = transform.position;
        isMoveToEnemy = false;
    }

    private void Update()
    {
        if (isMoveToEnemy)
            transform.position = Vector3.SmoothDamp(transform.position, enemyTransform.transform.position, ref velocity,
                (character_Counter ? BattleManager.Instance.bM_Timegap : BattleManager.Instance.bM_AttackTimegap) / 4);
        else
            transform.position = Vector3.SmoothDamp(transform.position, startPosition, ref velocity,
                (character_Counter ? BattleManager.Instance.bM_Timegap : BattleManager.Instance.bM_AttackTimegap) / 4);
    }

    public IEnumerator SetCharacterColor(string colorName)
    {
        switch (colorName)
        {
            case "red":
                this.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                break;
            case "green":
                this.gameObject.GetComponent<SpriteRenderer>().color = Color.green;
                break;
            case "blue":
                this.gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
                break;
            default:
                break;
        }

        yield return new WaitForSeconds(colorName == "red" ? BattleManager.Instance.bM_AttackTimegap : BattleManager.Instance.bM_Timegap);

        this.gameObject.GetComponent<SpriteRenderer>().color = Color.white;

        yield break;
    }

    IEnumerator Set_Attack_Motion(GameObject enemy_Character)
    {
        gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Damaged_Grid";
        isMoveToEnemy = true;

        yield return new WaitForSeconds(BattleManager.Instance.bM_AttackTimegap);

        isMoveToEnemy = false;
        enemyTransform = null;

        yield return new WaitForSeconds(BattleManager.Instance.bM_AttackTimegap);

        gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Characters";
    }

    public void Character_Attack(GameObject enemy_Character) // 캐릭터 스크립트 내에 있는 공격 함수.
    {
        enemyTransform = enemy_Character.transform;
        // 적 캐릭터를 받아와서, 그 캐릭터의 정보에 접근하여 받을 데미지에 공격력 만큼을 저장시킴.
        StartCoroutine(SetCharacterColor("red"));
        StartCoroutine(Set_Attack_Motion(enemy_Character));

        Character_Action enemy_Character_Action;
        enemy_Character_Action = enemy_Character.GetComponent<Character>() as Character_Action;

        int damage = 0;

        if (character_Skill == Skill.Attack_ArmorPiercer)
            damage = SkillManager.Instance.Skill_Attack_ArmorPiercer(this.gameObject,enemy_Character);
        else
            damage = (character_Attack_Damage * (100 + character_Buffed_Attack)) / 100;

        enemy_Character_Action.Character_Damaged(this.gameObject, damage); // 받을 데미지에 값이 저장되자마자 피격 함수 발동
    }

    public void Character_Damaged(GameObject attacker, int damage) // 피격 함수
    {
        // 받을 데미지를 다시 계산.

        Character_Counter();

        int final_damage = (damage * (100 - character_Buffed_Damaged)) / 100;

        if (character_Divine_Shield == true && final_damage > 0)
            character_Divine_Shield = false;
        else
            character_HP -= final_damage;

        if (character_HP <= 0) // 체력이 0이하가되면 체력을 0으로 초기화하고 사망함수 발동
        {
            character_HP = 0;
            Character_Dead(attacker);
        }
    }

    public void Character_Counter_Attack(GameObject enemy_Character) //카운터 발동
    {
        enemyTransform = enemy_Character.transform;
        StartCoroutine(SetCharacterColor("red"));
        StartCoroutine(Set_Attack_Motion(enemy_Character));

        Character_Action enemy_Character_Action;
        enemy_Character_Action = enemy_Character.GetComponent<Character>() as Character_Action;

        int damage = (character_Attack_Damage * (100 + character_Buffed_Attack)) / 100 / 2;
        enemy_Character_Action.Character_Counter_Damaged(this.gameObject, damage); // 받을 데미지에 값이 저장되자마자 피격 함수 발동

        character_Counter = false;
    }

    public void Character_Counter_Damaged(GameObject attacker, int damage) // 카운터 발동
    {
        int final_damage = (damage * (100 + character_Buffed_Damaged)) / 100;

        character_HP -= final_damage;

        if (character_HP <= 0) // 체력이 0이하가되면 체력을 0으로 초기화하고 사망함수 발동
        {
            character_HP = 0;
            Character_Dead(attacker);
        }
    }

    public void Character_Counter()
    {
        character_Counter = true;
    }

    public void Character_Dead(GameObject attacker) // 캐릭터 사망 함수. 아마 나중에 무언가가 더 추가되겠지?
    {
        Debug.Log(character_Num_Of_Grid + " is Dead");
        attacker.GetComponent<Character>().character_is_Kill++;
        this.gameObject.GetComponent<SpriteRenderer>().sprite = null;
        character_Is_Allive = false;
        character_Counter = false;
    }
}
