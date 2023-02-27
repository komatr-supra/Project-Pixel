using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
{
    CharacterController characterController;
    Mover mover;
    Attacker attacker;
    public IdleState(CharacterController characterController, Mover mover, Attacker attacker)
    {
        this.characterController = characterController;
        this.mover = mover;
        this.attacker = attacker;
    }
    public void OnEnter()
    {
        //play idle anim
        Debug.Log("enter idle");
    }

    public void OnExit()
    {
        
    }

    public void Tick()
    {
        if(Input.GetKeyDown(KeyCode.Z)) attacker.Attack();
        if(characterController.jumpPressed) mover.Jump();
    }
}
