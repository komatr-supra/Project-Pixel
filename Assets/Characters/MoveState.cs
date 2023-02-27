using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Character.Animator;
public class MoveState : IState
{
    CharacterController characterController;
    Mover mover;
    
    public MoveState(CharacterController characterController, Mover mover)
    {
        this.characterController = characterController;
        this.mover = mover;
    }
    public void OnEnter()
    {
        //characterAnimator.PlayAnimation();
        Debug.Log("enter move");
    }

    public void OnExit()
    {
        mover.Stop();
    }

    public void Tick()
    {
        mover.Move(characterController.inputVector.x);
        if(characterController.jumpPressed) mover.Jump();
    }
}
