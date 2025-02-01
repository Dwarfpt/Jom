using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetBoolBehaviour : StateMachineBehaviour
{
    [SerializeField] private string boolName;         // Имя булевой переменной
    [SerializeField] private bool updateOnState;      // Обновление в процессе состояния
    [SerializeField] private bool updateOnStateMachine; // Обновление на уровне машины состояний
    [SerializeField] private bool valueOnEnter;       // Значение при входе в состояние
    [SerializeField] private bool valueOnExit;        // Значение при выходе из состояния


    // OnStateEnter — вызывается при входе в состояние
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!string.IsNullOrEmpty(boolName))
        {
            animator.SetBool(boolName, valueOnEnter);
        }
    }

    // OnStateUpdate — вызывается каждый кадр, пока состояние активно
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (updateOnState && !string.IsNullOrEmpty(boolName))
        {
            animator.SetBool(boolName, valueOnEnter);
        }
    }

    // OnStateExit — вызывается при выходе из состояния
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!string.IsNullOrEmpty(boolName))
        {
            animator.SetBool(boolName, valueOnExit);
        }
    }

    // OnStateMachineEnter — вызывается при входе в машину состояний через Entry Node
    override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        if (updateOnStateMachine && !string.IsNullOrEmpty(boolName))
        {
            animator.SetBool(boolName, valueOnEnter);
        }
    }

    // OnStateMachineExit — вызывается при выходе из машины состояний через Exit Node
    override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        if (updateOnStateMachine && !string.IsNullOrEmpty(boolName))
        {
            animator.SetBool(boolName, valueOnExit);
        }
    }
}
