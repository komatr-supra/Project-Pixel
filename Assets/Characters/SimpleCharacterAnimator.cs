using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Character.Animator
{
    #region Animator enums
    public enum AnimType
    {
        idle,
        run,
        attack,
        jump
    }
    #endregion
    public class SimpleCharacterAnimator : MonoBehaviour
    {        
        #region Variables
        [SerializeField] TextMeshPro textMeshDebug;
        [SerializeField] private Sprite[] idleSprites;
        [SerializeField] private Sprite[] runSprites;
        [SerializeField] private Sprite[] attackSprites;
        [SerializeField] private Sprite[] jumpSprites;
        private Sprite[] actualAnimation;
        [SerializeField] private float animationSpeed = 5f;
        [SerializeField] SpriteRenderer spriteRendererMain;
        private float counter;
        private int index;
        private Dictionary<AnimType, Sprite[]> spritesDictionary;
        AnimType actualAnimType;
        #endregion
        private void Awake()
        {
            
            spritesDictionary = new();        
            LoadSprites();
        }
        private void Start()
        {
            //set starting animation
            actualAnimType = AnimType.idle;
            actualAnimation = spritesDictionary.GetValueOrDefault(actualAnimType);
        }
        private void Update()
        {
            if(counter < 1)
            {
                counter += Time.deltaTime * animationSpeed;
                return;
            }
            //time for next frame
            counter = 0;
            index = GetNextIndex();
            textMeshDebug.text = index.ToString();
            spriteRendererMain.sprite = actualAnimation[index];
        }

        private int GetNextIndex()
        {
            return ++index < actualAnimation.Length ? index : 0;
        }

        private void LoadSprites()
        {
            spritesDictionary.Add(AnimType.idle, idleSprites);
            spritesDictionary.Add(AnimType.run, runSprites);
            spritesDictionary.Add(AnimType.attack, attackSprites);
            spritesDictionary.Add(AnimType.jump, jumpSprites);
        }
        public void PlayAnimation(AnimType animationType)
        {
            if(animationType == actualAnimType)return;
            actualAnimType = animationType;
            actualAnimation = spritesDictionary.GetValueOrDefault(animationType);
            counter = 0;
            index = 0;
            spriteRendererMain.sprite = actualAnimation[index];
        }
    }
}

