using UnityEngine;
using System.Collections;

public class CheckDirection : StateMachineBehaviour {

    private Vector3 direction;

    static int yoko = Animator.StringToHash("Player.usiro_kaiten");
    static int naname = Animator.StringToHash("Player.mae_kaiten");

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        direction = GameObject.FindObjectOfType<PlayerControl>().GetComponent<Rigidbody2D>().velocity;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("maete_onaji", false);
        animator.SetBool("maete_tigau", false);
        animator.SetBool("usirote_onaji", false);
        animator.SetBool("usirote_tigau", false);

        PlayerControl player = GameObject.FindObjectOfType<PlayerControl>();
        if (direction.x * player.GetComponent<Rigidbody2D>().velocity.x < 0)
        {
            if (stateInfo.fullPathHash == yoko)
            {
                animator.SetTrigger("usirote_tigau");
            }
            else if (stateInfo.fullPathHash == naname)
            {
                animator.SetTrigger("maete_tigau");
            }
        }
    }

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}
}
