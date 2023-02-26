using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Character.Animator;
public class Mover : MonoBehaviour
{
    [SerializeField] LayerMask layerMask;
    [SerializeField] float speed;
    [SerializeField] float jumpForce;
    [SerializeField] float timeToForgiveJumpPressed = 0.1f;
    SimpleCharacterAnimator characterAnimator;
    bool jump;
    float jumpCounter;    
    Vector2 inputVector;
    Rigidbody2D rb2d;
    bool isGrounded;
    private void Awake() {
        rb2d = GetComponent<Rigidbody2D>();
        characterAnimator = GetComponent<SimpleCharacterAnimator>();
    }
    private void Update() {
        RaycastHit2D hit2D = Physics2D.Raycast(transform.position - new Vector3(0, -0.03f, 0), Vector2.down, 0.045f, layerMask);
        if(hit2D) isGrounded = true;
        else isGrounded = false;
        if(jump)
        {
            jumpCounter -= Time.deltaTime;
            if (jumpCounter < 0)
            {
                jump = false;
                return;
            }
            if(isGrounded)
            {
                rb2d.AddForce(jumpForce * Vector2.up, ForceMode2D.Impulse);
                jump = false;
            }
        }

        inputVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if(Input.GetButtonDown("Jump"))
        {
            JumpPressed();
        }
        float horizontalSpeed = inputVector.x * speed;
        rb2d.velocity = new Vector2(horizontalSpeed, rb2d.velocity.y);
        characterAnimator.SetMoveStats(new Data(rb2d.velocity, isGrounded) );
    }

    private void JumpPressed()
    {
        jump = true;
        jumpCounter = timeToForgiveJumpPressed;
    }

    void FixedUpdate()
    {
        
    }
}
