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
        if(counter > frameTimePerSecond)
        {
            index = ++index < sprites.Length ? index : 0;
            counter = 0;
        }
        if(!isLoop)
        spriteRenderer.sprite = sprites[index];
    }
}
