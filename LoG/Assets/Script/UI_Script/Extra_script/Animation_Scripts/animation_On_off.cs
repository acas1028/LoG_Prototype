using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animation_On_off : MonoBehaviour
{
    public Animator animator;
    public GameObject temporary_button; //배치된 캐릭터 팝업 클릭시 등장하는 배치 취소 버튼이 거스렬서 제작, 추후에 삭제

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
