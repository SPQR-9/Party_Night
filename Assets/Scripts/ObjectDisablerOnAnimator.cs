using UnityEngine;

public class ObjectDisablerOnAnimator : StateMachineBehaviour
{ 
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.SetActive(false);
    }
}
