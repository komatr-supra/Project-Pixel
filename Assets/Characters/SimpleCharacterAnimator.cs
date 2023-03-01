using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace ProjectPixel.Character.Animation
{
    public enum AnimType
        {
            idle,
            move,
            jump,
            fall,
            attack
        }
    public class SimpleCharacterAnimator : MonoBehaviour
    {
        #region Variables
        [SerializeField] Sprite[] idleSprites;
        [SerializeField] Sprite[] moveSprites;
        [SerializeField] Sprite[] jumpSprites;
        [SerializeField] Sprite[] fallSprites;
        [SerializeField] Sprite[] attackSprites;
        [SerializeField] SpriteRenderer charcterRenderer;
        private BaseAnimation[] animations;
        int index;
        #endregion

        private void Awake() {
            var idle = new BaseAnimation(idleSprites, charcterRenderer);
            var move = new BaseAnimation(moveSprites, charcterRenderer);
            var jump = new BaseAnimation(jumpSprites, charcterRenderer);
            var fall = new BaseAnimation(fallSprites, charcterRenderer);
            var attack = new BaseAnimation(attackSprites,charcterRenderer, false);
            attack.SetFrameForTrigger(4);

            animations = new BaseAnimation[]
                    {
                        idle,
                        move,
                        jump,
                        fall,
                        attack
                    };
        }
        private void Update() {
            animations[index].Tick();
        }        
        public void SetAnimation(AnimType animationTypeEnum, Action callback, Action end)
        {
            if((int)animationTypeEnum == index) return;
            animations[index].OnExit();
            index = (int)animationTypeEnum;
            animations[index].OnEnter();

            animations[index].SetTrigger(callback);
            animations[index].SetEndAction(end);
        }
    }
}

