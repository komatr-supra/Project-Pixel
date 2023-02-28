using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Character.Animator;
public class AirState : IState
{
    CharacterController characterController;
    public AirState(CharacterController characterController)
    {
        this.characterController = characterController;
    }
    public void OnEnter()
    {
        //play air anim up/down
        Debug.Log("enter air");
    }

    public void OnExit()
    {
        characterController.mover.Stop();
    }

    public void Tick()
    {
        characterController.mover.Move(characterController.inputVector.x, true);
    }
}
