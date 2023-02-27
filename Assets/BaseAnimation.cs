using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BaseAnimation : IState
{
    private float frameTimePerSecond = 1 / 10f;
    private float counter;
    private SpriteRenderer spriteRenderer;
    private Sprite[] sprites;
    private int index;
    private bool isLoop;
    private Action trigger;
    private Action end;
    int frameTrigger = -1;
    public BaseAnimation(Sprite[] sprites, SpriteRenderer spriteRenderer, bool loop = true)
    {
        this.spriteRenderer = spriteRenderer;
        this.sprites = sprites;
        isLoop = loop;

    }

    public void OnEnter()
    {
        index = 0;
        counter = 0;
        CheckTrigger();
        spriteRenderer.sprite = sprites[index];
    }


    public void OnExit()
    {
        
    }

    public void Tick()
    {
        Animate();
    }

    private void Animate()
    {
        counter += Time.deltaTime;
        if(counter < frameTimePerSecond) return;
        CheckTrigger();
        if(++index < sprites.Length)
        {
            spriteRenderer.sprite = sprites[index];
            counter = 0;
            return;
        }
        if(!isLoop)
        {
            end?.Invoke();
        }
        else
        {
            index = 0;
            spriteRenderer.sprite = sprites[index];
        }
    }
    private void CheckTrigger()
    {
        if(frameTrigger == -1 && index != frameTrigger) return;
        trigger?.Invoke();
    }
    public void SetTrigger(int frame, Action action)
    {
        trigger = action;
        frameTrigger = frame;
    }
}
