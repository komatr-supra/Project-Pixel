using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Character.Animator;
public class testAnimator : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10;
    Vector2 inputVector;
    Rigidbody2D rb2d;
    SimpleCharacterAnimator animator;
    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<SimpleCharacterAnimator>();
    }
    void Update()
    {
        inputVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if(Mathf.Abs(rb2d.velocity.x) < float.Epsilon) animator.PlayAnimation(AnimType.idle);        
        else animator.PlayAnimation(AnimType.run);
    }
    private void FixedUpdate()
    {
        if(inputVector != Vector2.zero) rb2d.velocity = inputVector * moveSpeed;
    }
}
