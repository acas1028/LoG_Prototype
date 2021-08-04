using System.Collections;
using UnityEngine;


// ĳ������ ������ ������� Ư�� �ൿ�� ���ϴ� Ŭ����
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

        // �� ĳ���͸� �޾ƿͼ�, �� ĳ������ ������ �����Ͽ� ���� �������� ���ݷ� ��ŭ�� �����Ŵ.

        Character_Action enemy_Character_Action;
        enemy_Character_Action = enemy_Character.GetComponent<Character>() as Character_Action;

        int damage = 0;

        if (isCounter)
        {
            damage = (character_Attack_Damage * (100 + character_Buffed_Attack)) / 100 / 2;
            enemy_Character_Action.Character_Counter_Damaged(this.gameObject, damage); // ���� �������� ���� ������ڸ��� �ǰ� �Լ� �ߵ�

            character_Counter = false;
        }
        else
        {
            if (character_Skill == Skill.Attack_ArmorPiercer)
                damage = SkillManager.Instance.ArmorPiercer(this.gameObject, enemy_Character);
            else
                damage = (character_Attack_Damage * (100 + character_Buffed_Attack)) / 100;

            enemy_Character_Action.Character_Damaged(this.gameObject, damage); // ���� �������� ���� ������ڸ��� �ǰ� �Լ� �ߵ�
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

    public void Character_Damaged(GameObject attacker, int damage) // �ǰ� �Լ�
    {
        // ���� �������� �ٽ� ���.
        StartCoroutine(SetCharacterColor("red"));
        Character_Counter();

        int final_damage = (damage * (100 - character_Buffed_Damaged)) / 100;

        if (character_Divine_Shield == true && final_damage > 0)
        {
            character_Divine_Shield = false;
            final_damage = 0;
        }

        character_HP -= final_damage;

        if (character_HP <= 0) // ü���� 0���ϰ��Ǹ� ü���� 0���� �ʱ�ȭ�ϰ� ����Լ� �ߵ�
        { 
            character_HP = 0;
            Character_Dead(attacker);
        }

    }

    public void Character_Counter_Damaged(GameObject attacker, int damage) // ī���� �ߵ�
    {
        StartCoroutine(SetCharacterColor("red"));
        int final_damage = (damage * (100 + character_Buffed_Damaged)) / 100;

        if (character_Divine_Shield == true && final_damage > 0)
        {
            character_Divine_Shield = false;
            final_damage = 0;
        }

        character_HP -= final_damage;

        if (character_HP <= 0) // ü���� 0���ϰ��Ǹ� ü���� 0���� �ʱ�ȭ�ϰ� ����Լ� �ߵ�
        {
            character_HP = 0;
            Character_Dead(attacker);
        }
    }

    public void Character_Counter()
    {
        character_Counter = true;
    }

    public void Character_Dead(GameObject attacker) // ĳ���� ��� �Լ�. �Ƹ� ���߿� ���𰡰� �� �߰��ǰ���?
    {
        Debug.Log(character_Num_Of_Grid + " is Dead");
        attacker.GetComponent<Character>().character_is_Kill++;
        character_is_Killed = true;
        character_Counter = false;
    }

    public IEnumerator Dead()
    {
        this.gameObject.GetComponent<SpriteRenderer>().sprite = null;
        character_Is_Allive = false;
        character_is_Killed = false;
        yield return new WaitForSeconds(BattleManager.Instance.bM_AttackTimegap);
    }
}
