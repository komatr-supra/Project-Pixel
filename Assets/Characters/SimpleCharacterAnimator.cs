using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Character.Animator
{
    public struct Data
    {
        public Data(Vector2 dir, bool gnd)
        {
            direction = dir;
            isGrounded = gnd;
        }
        public Vector2 direction;
        public bool isGrounded;
    }
    public class SimpleCharacterAnimator : MonoBehaviour
    {
        [SerializeField] Sprite[] idleSprites;
        [SerializeField] Sprite[] moveSprites;
        [SerializeField] Sprite[] jumpSprites;
        [SerializeField] Sprite[] fallSprites;
        [SerializeField] SpriteRenderer charcterRenderer;
        private StateMachine stateMachine;
        private Data moveData;
        private bool attack;
        private void Awake() {
            stateMachine = new StateMachine();
            var idle = new BaseAnimation(idleSprites, charcterRenderer);
            var move = new BaseAnimation(moveSprites, charcterRenderer);
            var jump = new BaseAnimation(jumpSprites, charcterRenderer);
            var fall = new BaseAnimation(fallSprites, charcterRenderer);
            stateMachine.AddAnyTransition(idle, () => Mathf.Abs(moveData.direction.x) < 0.01f && moveData.isGrounded);
            stateMachine.AddTransition(idle, move, () => Mathf.Abs(moveData.direction.x) > 0.01f);
            stateMachine.AddAnyTransition(jump, () => moveData.direction.y > 0.01f && !moveData.isGrounded);
            stateMachine.AddAnyTransition(fall, () => moveData.direction.y < 0.01f && !moveData.isGrounded);
            stateMachine.SetState(idle);
        }
        private void Start() {
        }
        private void Update() {
            stateMachine.Tick();
        }
        public void SetMoveStats(Data data)
        {
            moveData = data;

        }
        public void AttackTrigger()
        {
            if(attack == true || !moveData.isGrounded) return;
            attack = true;
        }
    }
}

