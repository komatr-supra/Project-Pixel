using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Character.Animator;
public class MoveState : IState
{
    CharacterController characterController;
    
    public MoveState(CharacterController characterController)
    {
        this.characterController = characterController;
    }
    public void OnEnter()
    {
        //characterAnimator.PlayAnimation();
        Debug.Log("enter move");
    }

    public void OnExit()
    {
        characterController.mover.Stop();
    }

    public void Tick()
    {
        characterController.mover.Move(characterController.inputVector.x);
        if(characterController.Jump) characterController.mover.Jump();
    }
}
