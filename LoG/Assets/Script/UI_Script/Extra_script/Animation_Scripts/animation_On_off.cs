using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animation_On_off : MonoBehaviour
{
    public Animator animator;
    public GameObject temporary_button; //��ġ�� ĳ���� �˾� Ŭ���� �����ϴ� ��ġ ��� ��ư�� �Ž��ļ� ����, ���Ŀ� ����

    private void Awake()
    {
        animator = this.gameObject.GetComponent<Animator>();
    }

    private void Start()
    {
        
    }




    public void AnimationOn()
    {
        this.gameObject.SetActive(true);
        
    }

    public void AnimationOff()
    {
        if(temporary_button!=null)
        {
            temporary_button.SetActive(false);
        }

        animator.SetInteger("PopUp", 1);

        if (this.gameObject.activeInHierarchy)
        {
            StartCoroutine(WaitForAnimation(animator));
        }
    }

    private IEnumerator WaitForAnimation(Animator animator)
    {
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        float oldLength = state.length;

        yield return new WaitForSeconds(oldLength);


        Limit_Popup.Limit_Poup_instance.BoolPopupOn_Off();
        this.gameObject.SetActive(false);

    }
}
