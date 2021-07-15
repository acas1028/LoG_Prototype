using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animation_On_off : MonoBehaviour
{
    public Animator animator;

    private void Awake()
    {
        animator = this.gameObject.GetComponent<Animator>();
    }




    public void AnimationOn()
    {
        this.gameObject.SetActive(true);
       
    }

    public void AnimationOff()
    {
        animator.SetInteger("PopUp", 1);
        StartCoroutine(WaitForAnimation(animator));


    }

    private IEnumerator WaitForAnimation(Animator animator)
    {
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        float oldLength = state.length;

        yield return new WaitForSeconds(oldLength);

        this.gameObject.SetActive(false);

    }
}
