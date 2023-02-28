using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Character.Animator;
using System;

public class CharacterController : MonoBehaviour
{
    [SerializeField] internal SimpleCharacterAnimator characterAnimator;
    [SerializeField] LayerMask groundLayer;
    [Tooltip("Delay time in second. How much time is key virtually pressed.")]
    [Range(0,1)]
    [SerializeField] private float keyDelayTime;
    internal Vector2 inputVector;
    internal Mover mover;
    internal Attacker attacker;
    public Action onAttackInputChanged;
    public Action<Vector2> onMoveInputChanged;
    public Action onJumpInputChanged;
    private StateMachine stateMachine;
    private Rigidbody2D characterRB;
    private bool jumpInputActive;
    private float jumpDelayCounter = -1;
    private bool attactInputActive;
    private float attackDelayCounter = -1;
    private bool isGrounded;
    private bool isInputDisabled = false;
    private void Awake() {
        mover = GetComponent<Mover>();
        attacker = GetComponent<Attacker>();
        characterRB = GetComponent<Rigidbody2D>();

        stateMachine = new StateMachine();
        stateMachine.onNewTransitionStart += HandleInputFromOldState;
        var idle = new IdleState(this);
        var move = new MoveState(this);
        var air = new AirState(this);
        stateMachine.AddAnyTransition(idle, IsIdle());
        stateMachine.AddTransition(idle, move, MovingWithInput());
        stateMachine.AddAnyTransition(air, InAir());
        stateMachine.AddTransition(air, move, MovingWithInput());
        Func<bool> IsIdle() => () => isGrounded && inputVector == Vector2.zero && characterRB.velocity.x < 0.01f;
        Func<bool> MovingWithInput() => () => !isInputDisabled && isGrounded && inputVector != Vector2.zero;
        Func<bool> InAir() => () => !isGrounded;
    }
    private void Update() {
        UpdateButtonKeeper();
        var raycast = Physics2D.Raycast(transform.position + Vector3.up, Vector2.down, 2f, groundLayer);
        isGrounded = raycast;
        stateMachine.Tick();
        
    }

    private void UpdateButtonKeeper()
    {
        if(jumpDelayCounter > 0) jumpDelayCounter -= Time.deltaTime;
        else ClearJumpInput();
        if(attackDelayCounter > 0) attackDelayCounter -= Time.deltaTime;
        else CleatAttackInput(name);


    }

    public void CleatAttackInput(string who)
    {
        Debug.Log("cleaning attack input" + who);
        attactInputActive = false;
    }

    public void ClearJumpInput()
    {
        
        jumpInputActive = false;
    }

    public void EnableInput()
    {
        isInputDisabled = false;
    }
    public void DisableInput()
    {
        isInputDisabled = true;
    }
    public void SetInputVector(Vector2 inputVector)
    {
        this.inputVector = inputVector;
        onMoveInputChanged?.Invoke(inputVector);
    }
    public void PerformJump()
    {
        jumpDelayCounter = keyDelayTime;
        jumpInputActive = true;
        onJumpInputChanged?.Invoke();
    }
    public void PerformAttack()
    {
        attackDelayCounter = keyDelayTime;
        attactInputActive = true;
        onAttackInputChanged?.Invoke();

    }
    //when key was pressed in old transition, but wasnt consumed
    private void HandleInputFromOldState()
    {
        if(jumpInputActive) onJumpInputChanged?.Invoke();
        if(attactInputActive) onAttackInputChanged?.Invoke();
    }
}
