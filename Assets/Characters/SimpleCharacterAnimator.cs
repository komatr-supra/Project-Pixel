using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Character.Animator
{
    public class SimpleCharacterAnimator : MonoBehaviour
    {
        public enum AnimType
        {
            idle,
            move,
            jump,
            fall,
            attack
        }
        [SerializeField] Sprite[] idleSprites;
        [SerializeField] Sprite[] moveSprites;
        [SerializeField] Sprite[] jumpSprites;
        [SerializeField] Sprite[] fallSprites;
        [SerializeField] Sprite[] attackSprites;
        [SerializeField] SpriteRenderer charcterRenderer;
        private bool attack;
        private BaseAnimation[] animations;
        int index;
        private void Awake() {
            var idle = new BaseAnimation(idleSprites, charcterRenderer);
            var move = new BaseAnimation(moveSprites, charcterRenderer);
            var jump = new BaseAnimation(jumpSprites, charcterRenderer);
            var fall = new BaseAnimation(fallSprites, charcterRenderer);
            var attack = new BaseAnimation(attackSprites,charcterRenderer);
            
            
            animations = new BaseAnimation[]
                    {
                        idle,
                        move,
                        jump,
                        fall,
                        attack
                    };
        }
        private void Start() {

        }
        private void Update() {
            
        }
        
        public void SetAnimation(AnimType animationTypeEnum)
        {
            if((int)animationTypeEnum == index) return;

        }
    }
}

