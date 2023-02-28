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
        Debug.Log("enter move");
        characterController.onJumpInputChanged += JumpFromMove;
    }

    public void OnExit()
    {
        characterController.onJumpInputChanged -= JumpFromMove;
        characterController.mover.Stop();
    }

    public void Tick()
    {
        characterController.mover.Move(characterController.inputVector.x);
    }
    private void JumpFromMove()
    {
        characterController.mover.Jump();
        characterController.ClearJumpInput();
    }
}
