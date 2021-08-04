using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emoticon_Time_Script : MonoBehaviour
{
    public Animator Mine_animator;

    private void Awake()
    {
        Mine_animator = this.gameObject.GetComponent<Animator>();
    }

    private void Start()
    {
        this.gameObject.GetComponent<Animator>().Play("Emoticon_Click");
        StartCoroutine(Animation_Off(Mine_animator));
    }

    IEnumerator Animation_Off(Animator animator) //�̸�Ƽ���� 3�� ȭ���̻� �������� �̸� ��Ʈ���� �Լ�
    {
        yield return new WaitForSeconds(3.0f);

        this.gameObject.GetComponent<Animator>().Play("Emoticon_Click_Disappear");
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        float oldLength = state.length;

        yield return new WaitForSeconds(oldLength);

        Destroy(this.gameObject);
    }
}