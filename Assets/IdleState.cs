using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IUnitState
{
    CharacterController characterController;
    public IdleState(CharacterController characterController)
    {
        this.characterController = characterController;
    }
    public void OnEnter()
    {
        Debug.Log("idle start");
        characterController.onAttackInputChanged += AttackFromIdle;
        characterController.onJumpInputChanged += JumpFromIdle;
        CallbackActionEnd();
    }

    public void OnExit()
    {
        characterController.onAttackInputChanged -= AttackFromIdle;
        characterController.onJumpInputChanged -= JumpFromIdle;
    }

    public void CallbackActionEnd()
    {        
        characterController.characterAnimator.SetAnimation(
                Character.Animator.SimpleCharacterAnimator.AnimType.idle, 
                null, 
                null);
        characterController.EnableInput();
    }

    public void Tick()
    {
        
    }
    private void JumpFromIdle()
    {
        characterController.mover.Jump();
        characterController.ClearJumpInput();
    }
    private void AttackFromIdle()
    {
        Debug.Log("attacking from idle");
        characterController.attacker.Attack(CallbackActionEnd, out bool canMoveWithAttacking);
        if(canMoveWithAttacking)
        {
            characterController.EnableInput();
        }
        else
        {
            characterController.DisableInput();
        }
        characterController.CleatAttackInput("idle state");
    }

    
}
