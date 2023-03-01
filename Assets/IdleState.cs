using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectPixel.Character.Animation;

namespace ProjectPixel.Character.State
{
    public class IdleState : IUnitState
    {
        CharacterController characterController;
        //basic class can have constructor
        public IdleState(CharacterController characterController)
        {
            this.characterController = characterController;
        }
        public void OnEnter()
        {
            Debug.Log("idle start");
            //subscribe the inputs -> dont need check input every frame
            characterController.onAttackInputChanged += AttackFromIdle;
            characterController.onJumpInputChanged += JumpFromIdle;
            PlayIdleAnimation();
            EnableInput();
        }

        public void OnExit()
        {
            //UNsubscribe the inputs -> this class exist even if isnt active, it will trigger local actions even in other states
            characterController.onAttackInputChanged -= AttackFromIdle;
            characterController.onJumpInputChanged -= JumpFromIdle;
        }
        //this is trigerred by END of animation(when return from attack -> attack cant be state, you can attack from idle, move, maybe shoot from jump... ??? OR ???)
        public void CallbackActionEnd()
        {
            PlayIdleAnimation();
            EnableInput();
        }

        private void EnableInput() => characterController.EnableInput();
        private void PlayIdleAnimation()
        {
            characterController.characterAnimator.SetAnimation(
                            AnimType.idle,
                            null,
                            null);
        }

        public void Tick()
        {

        }
        //jumping -> it is not a state(state is AirState and you can fall or be kicked to air)
        private void JumpFromIdle()
        {
            characterController.mover.Jump();
            characterController.ClearJumpInput();
        }
        //attack 
        private void AttackFromIdle()
        {
            Debug.Log("attacking from idle");
            characterController.attacker.Attack(CallbackActionEnd, out bool canMoveWithAttacking);
            if (canMoveWithAttacking)
            {
                characterController.EnableInput();
            }
            else
            {
                characterController.DisableInput();
            }
            characterController.ClearAttackInput();
        }
    }
}