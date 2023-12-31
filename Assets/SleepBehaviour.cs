using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepBehaviour : StateMachineBehaviour
{
    [SerializeField]
    private float timeUntilBored;
    [SerializeField]
    private int numberOfBoredAnimations;

    private bool isBored;
    private float idleTIme;
    private int boredAnimation;
    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ResetIdle();
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (isBored == false)
        {
            idleTIme += Time.deltaTime;
            if (idleTIme > timeUntilBored && stateInfo.normalizedTime %1<0.02f)
            {
                isBored = true;
                boredAnimation = Random.Range(1, numberOfBoredAnimations + 1);
                boredAnimation = boredAnimation * 2 - 1;

                animator.SetFloat("BoredAnimation", boredAnimation - 1);
            }
        }
        else if (stateInfo.normalizedTime %1 >0.98)
        {
            ResetIdle();
        }
        animator.SetFloat("BoredAnimation", boredAnimation,0.2f,Time.deltaTime);
    }

    private void ResetIdle()
    {
        if (isBored)
        {
            boredAnimation--;
        }
        isBored = false;
        idleTIme = 0;
    }

}
