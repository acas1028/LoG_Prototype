using System.Collections;
using UnityEngine;
using CharacterStats;


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
        if (character_Is_Allive)
            // 현재 ID 개수(14개)만큼의 스프라이트가 없어서 임시로 적용함
            // gameObject.GetComponent<SpriteRenderer>().sprite = Character_Sprite[Character_ID];
            gameObject.GetComponent<SpriteRenderer>().sprite = Character_Sprite[(character_ID - 1) % 7 + 1];
        else
            gameObject.GetComponent<SpriteRenderer>().sprite = Character_Sprite[0];

        if (isMoveToEnemy && enemyTransform)
            transform.position = Vector3.SmoothDamp(transform.position, enemyTransform.transform.position, ref velocity,
                (character_Counter ? BattleManager.Instance.bM_Timegap : BattleManager.Instance.bM_AttackTimegap) / 4);
        else if (!isMoveToEnemy)
            transform.position = Vector3.SmoothDamp(transform.position, startPosition, ref velocity,
                (character_Counter ? BattleManager.Instance.bM_Timegap : BattleManager.Instance.bM_AttackTimegap) / 4);
    }

    public IEnumerator SetCharacterColor(string colorName = "default")
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
                this.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                break;
        }

        yield return new WaitForSeconds(colorName == "red" ? BattleManager.Instance.bM_AttackTimegap : BattleManager.Instance.bM_Timegap);

        this.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }

    public IEnumerator Attack(GameObject enemy_Character, bool isCounter)
    {
        enemyTransform = enemy_Character.transform;
        gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Attacker";
        isMoveToEnemy = true;

        yield return new WaitForSeconds(BattleManager.Instance.bM_AttackTimegap);

        // 적 캐릭터를 받아와서, 그 캐릭터의 정보에 접근하여 받을 데미지에 공격력 만큼을 저장시킴.

        Character_Action enemy_Character_Action;
        enemy_Character_Action = enemy_Character.GetComponent<Character_Action>();

        int damage = 0;

        if (enemy_Character.GetComponent<Character>().character_Is_Allive)
        {
            if (isCounter)
            {
                damage = character_Attack_Damage;
                enemy_Character_Action.Character_Counter_Damaged(this.gameObject, damage); // 받을 데미지에 값이 저장되자마자 피격 함수 발동

                character_Counter = false;
            }
            else
            {
                if (character_Skill == CharacterSkill.Attack_ArmorPiercer)
                    damage = SkillManager.Instance.ArmorPiercer(this.gameObject, enemy_Character);
                else
                    damage = (character_Attack_Damage * (100 + character_Buffed_Attack)) / 100;

                enemy_Character_Action.Character_Damaged(this.gameObject, damage); // 받을 데미지에 값이 저장되자마자 피격 함수 발동
            }
        }

        isMoveToEnemy = false;
        enemyTransform = null;

        yield return new WaitForSeconds(BattleManager.Instance.bM_AttackTimegap);

        gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Characters";
    }

    public IEnumerator SkillAttack()
    {
        StopAllCoroutines();
        yield return StartCoroutine(SetCharacterColor("blue"));
    }

    public void Character_Damaged(GameObject attacker, int damage) // 피격 함수
    {
        // 받을 데미지를 다시 계산.
        StartCoroutine(SetCharacterColor("red"));
        Character_Counter();

        int final_damage = (damage * (100 - character_Buffed_Damaged)) / 100;

        if (character_Divine_Shield == true && final_damage > 0)
        {
            character_Divine_Shield = false;
            final_damage = 0;
        }

        is_hit_this_turn = true;
        character_HP -= final_damage;

        if (character_HP == character_MaxHP && character_HP <= final_damage)
            is_overkill = true;

        if (character_HP <= 0) // 체력이 0이하가되면 체력을 0으로 초기화하고 사망함수 발동
        { 
            Character_Dead(attacker);
        }

    }

    public void Character_Counter_Damaged(GameObject attacker, int damage) // 카운터 발동
    {
        StartCoroutine(SetCharacterColor("red"));
        int final_damage = (damage * (100 - character_Buffed_Damaged)) / 100;

        if (character_Divine_Shield == true && final_damage > 0)
        {
            character_Divine_Shield = false;
            final_damage = 0;
        }

        is_hit_this_turn = true;
        character_HP -= final_damage;

        if (character_HP <= 0) // 체력이 0이하가되면 체력을 0으로 초기화하고 사망함수 발동
        {
            Character_Dead(attacker);
        }
    }

    public void Character_Counter()
    {
        if (character_Counter_Probability > 100)
            character_Counter_Probability = 100;
        if (character_Counter_Probability < 0)
            character_Counter_Probability = 0;

        int rand = Random.Range(0, 100);

        if (rand < character_Counter_Probability)
            character_Counter = true;
        else
            character_Counter = false;
    }

    public void Character_Dead(GameObject attacker) // 캐릭터 사망 함수. 아마 나중에 무언가가 더 추가되겠지?
    {
        Debug.Log(character_Num_Of_Grid + " is Dead");
        character_HP = 0;
        attacker.GetComponent<Character>().character_is_Kill++;
        character_is_Killed = true;
        character_Counter = false;
        killedBy = attacker;
    }

    public IEnumerator Dead()
    {
        character_Is_Allive = false;
        character_is_Killed = false;
        yield return new WaitForSeconds(BattleManager.Instance.bM_AttackTimegap);
    }
}
