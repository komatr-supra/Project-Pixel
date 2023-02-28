using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Character.Animator;
public class Mover : MonoBehaviour
{
    [SerializeField] float speed = 2f;
    [SerializeField] float inAirPenalityModifier = 0.5f;
    [SerializeField] float jumpForce = 50f;
    Vector2 inputVector;
    Rigidbody2D rb2d;
    bool jump;
    private void Awake() {
        rb2d = GetComponent<Rigidbody2D>();
    }
    public void Jump()
    {
        jump = true;
    }
    public void Move(float inputX,bool isInAir = false)
    {
        rb2d.velocity = new Vector2(isInAir ? speed * inAirPenalityModifier * inputX : speed * inputX, rb2d.velocity.y);
    }
    public void Stop()
    {
        rb2d.velocity = new Vector2(0, rb2d.velocity.y);
    }
    private void FixedUpdate() {
        if (jump)
        {            
            rb2d.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            jump = false;
        }
    }
}
