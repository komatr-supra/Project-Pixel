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
        CallbackActionEnd();
        Debug.Log("idlen start");
    }

    public void OnExit()
    {

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
        if(characterController.Attack) 
        {            
            Debug.Log("attacking");
            characterController.attacker.Attack(CallbackActionEnd, out bool canMoveWithAttacking);
            if(canMoveWithAttacking)
            {
                characterController.EnableInput();
            }
            else
            {
                characterController.DisableInput();
            }
        }
        if(characterController.Jump) characterController.mover.Jump();
    }
    

    
}
