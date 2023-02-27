using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Character.Animator;
public class AirState : IState
{
    CharacterController characterController;
    Mover mover;
    
    public AirState(CharacterController characterController, Mover mover)
    {
        this.characterController = characterController;
        this.mover = mover;
    }
    public void OnEnter()
    {
        //play air anim up/down
        Debug.Log("enter air");
    }

    public void OnExit()
    {
        mover.Stop();
    }

    public void Tick()
    {
        mover.Move(characterController.inputVector.x, true);
    }
}
