using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IUnitState
{
    CharacterController characterController;
    bool isMoveDisabled;
    public IdleState(CharacterController characterController)
    {
        this.characterController = characterController;
    }
    public void OnEnter()
    {
        CallbackActionEnd();
    }

    public void OnExit()
    {
        
    }

    public void CallbackActionEnd()
    {
        isMoveDisabled = false;
        characterController.characterAnimator.SetAnimation(Character.Animator.SimpleCharacterAnimator.AnimType.idle, null, null);
    }

    public void Tick()
    {
        if(Input.GetKeyDown(KeyCode.Z)) characterController.attacker.Attack(CallbackActionEnd, out isMoveDisabled);
        if(characterController.jumpPressed) characterController.mover.Jump();
    }
    

    
}
