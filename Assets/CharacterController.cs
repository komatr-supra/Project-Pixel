using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectPixel.Character.Animation;
using System;
using ProjectPixel.Character;
using ProjectPixel.Character.State;
public class CharacterController : MonoBehaviour
{
    #region Variables
    [SerializeField] LayerMask groundLayer;
    [Tooltip("Delay time in second. How much time is key virtually pressed.")]
    [Range(0,1)]
    [SerializeField] private float keyDelayTime;
    [HideInInspector] public SimpleCharacterAnimator characterAnimator;
    [HideInInspector] public Mover mover;
    [HideInInspector] public Attacker attacker;
    public Action onAttackInputChanged;
    public Action<Vector2> onMoveInputChanged;
    public Action onJumpInputChanged;
    [HideInInspector] public Vector2 inputVector;
    private StateMachine stateMachine;
    private Rigidbody2D characterRB;
    private bool jumpInputActive;
    private float jumpDelayCounter = -1;
    private bool attactInputActive;
    private float attackDelayCounter = -1;
    private bool isGrounded;
    private bool isInputDisabled = false;
    #endregion

    private void Awake() {
        mover = GetComponent<Mover>();
        attacker = GetComponent<Attacker>();
        characterRB = GetComponent<Rigidbody2D>();
        characterAnimator = GetComponent<SimpleCharacterAnimator>();
        #region State Machine Implementation
        //create state machine
        stateMachine = new StateMachine();
        stateMachine.onNewTransitionStart += HandleInputFromOldState;
        //create states
        var idle = new IdleState(this);
        var move = new MoveState(this);
        var air = new AirState(this);
        //transition between states
        stateMachine.AddAnyTransition(idle, IsIdle());
        stateMachine.AddTransition(idle, move, MovingWithInput());
        stateMachine.AddAnyTransition(air, InAir());
        stateMachine.AddTransition(air, move, MovingWithInput());
        //predictors, simple functions. driving transitions
        Func<bool> IsIdle() => () => isGrounded && inputVector == Vector2.zero && characterRB.velocity.x < 0.01f;
        Func<bool> MovingWithInput() => () => !isInputDisabled && isGrounded && inputVector != Vector2.zero;
        Func<bool> InAir() => () => !isGrounded;
        #endregion
    }
    private void Update() {
        UpdateButtonKeeper();
        //TODO make a real function for checking character (in air, water, speed...)
        var raycast = Physics2D.Raycast(transform.position + Vector3.up, Vector2.down, 2f, groundLayer);
        isGrounded = raycast;
        stateMachine.Tick();        
    }
    #region Inputs
    //this virtually extends time for keys -> key look as pressed for keyDelayTime
    private void UpdateButtonKeeper()
    {
        if(jumpDelayCounter > 0) jumpDelayCounter -= Time.deltaTime;
        else ClearJumpInput();
        if(attackDelayCounter > 0) attackDelayCounter -= Time.deltaTime;
        else ClearAttackInput();


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
    public void ClearAttackInput()
    {
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
    #endregion

    //when key was pressed in old transition, but wasnt consumed
    private void HandleInputFromOldState()
    {
        if(jumpInputActive) onJumpInputChanged?.Invoke();
        if(attactInputActive) onAttackInputChanged?.Invoke();
    }
}
