using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter_Crouch : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Set 'crouch' flag back to false, so that we only crouch once
        animator.SetBool("crouch", false);
    }
}
