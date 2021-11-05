using System.Collections;
using UnityEngine;
using CharacterStats;

using Photon.Pun;

// ĳ������ ������ ������� Ư�� �ൿ�� ���ϴ� Ŭ����
public class Character_Action : Character, IPunObservable
{
    public GameObject hudDamageText;
    private Vector3 startPosition;
    private bool isMoveToEnemy;
    private Transform enemyTransform;
    private Vector3 velocity;

    private int counterRand;

    private void Start()
    {
        startPosition = Vector3.zero;
        isMoveToEnemy = false;
        counterRand = 100;

        if (!photonView.IsMine && !PhotonNetwork.OfflineMode)
        {
            BattleManager.Instance.bM_Character_Team2.Add(gameObject);
        }
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

    public void SetStartPos(Vector3 startPos)
    {
        startPosition = startPos;
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
        enemy_Character_Action = enemy_Character.GetComponent<Character_Action>();

        int damage = 0;

        if (enemy_Character.GetComponent<Character>().character_Is_Allive)
        {
            if (isCounter)
            {
                damage = character_Attack_Damage;
                enemy_Character_Action.Character_Counter_Damaged(this.gameObject, damage); // ���� �������� ���� ������ڸ��� �ǰ� �Լ� �ߵ�

                character_Counter = false;
            }
            else
            {
                if (character_Skill == CharacterSkill.Attack_ArmorPiercer)
                    damage = SkillManager.Instance.ArmorPiercer(this.gameObject, enemy_Character);
                else
                    damage = (character_Attack_Damage * (100 + character_Buffed_Attack)) / 100;

                enemy_Character_Action.Character_Damaged(this.gameObject, damage); // ���� �������� ���� ������ڸ��� �ǰ� �Լ� �ߵ�
            }
        }

        isMoveToEnemy = false;
        enemyTransform = null;

        yield return new WaitForSeconds(BattleManager.Instance.bM_AttackTimegap);
        

        if(character_Num_Of_Grid == 1 || character_Num_Of_Grid == 2 || character_Num_Of_Grid == 3)
        {
            gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Characters_Line1";
        }

        if (character_Num_Of_Grid == 4 || character_Num_Of_Grid == 5 || character_Num_Of_Grid == 6)
        {
            gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Characters_Line2";
        }

        if (character_Num_Of_Grid == 7 || character_Num_Of_Grid == 8 || character_Num_Of_Grid == 9)
        {
            gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Characters_Line3";
        }
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

        GameObject hudText = Instantiate(hudDamageText);
        hudText.transform.SetParent(GameObject.Find("Canvas").transform);
        hudText.transform.position = hudText.GetComponent<DamageUI>().worldToUISpace(GameObject.Find("Canvas").transform.GetComponent<Canvas>(), this.transform.position);
        hudText.GetComponent<DamageUI>().damage = final_damage;

        is_hit_this_turn = true;
        character_HP -= final_damage;

        if (character_HP == character_MaxHP && character_HP <= final_damage)
            is_overkill = true;

        if (character_HP <= 0) // ü���� 0���ϰ��Ǹ� ü���� 0���� �ʱ�ȭ�ϰ� ����Լ� �ߵ�
        { 
            Character_Dead(attacker);
        }

    }

    public void Character_Counter_Damaged(GameObject attacker, int damage) // ī���� �ߵ�
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

        if (character_HP <= 0) // ü���� 0���ϰ��Ǹ� ü���� 0���� �ʱ�ȭ�ϰ� ����Լ� �ߵ�
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

        if (photonView.IsMine)
            counterRand = Random.Range(0, 100); // 0~99

        if (counterRand < character_Counter_Probability)
            character_Counter = true;
        else
            character_Counter = false;
    }

    public void Character_Dead(GameObject attacker) // ĳ���� ��� �Լ�. �Ƹ� ���߿� ���𰡰� �� �߰��ǰ���?
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
        spriteManager.SetDeadSprite();
        character_Is_Allive = false;
        character_is_Killed = false;
        yield return new WaitForSeconds(BattleManager.Instance.bM_AttackTimegap);
    }

    #region ���� ������ ��ȯ, IPunObservable �������̽� ���
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo message)
    {
        if (stream.IsWriting)
            stream.SendNext(counterRand);
        else // stream.IsReading
            counterRand = (int)stream.ReceiveNext();
    }
    #endregion
}
