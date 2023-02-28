using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField] CharacterController characterController;
    private Vector2 previousInputVector = new Vector2();
    void Update()
    {
        Vector2 inputVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if(inputVector != previousInputVector)
        {
            characterController.SetInputVector(inputVector);
            previousInputVector = inputVector;
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            characterController.PerformJump();
        }
        if(Input.GetKeyDown(KeyCode.Z))
        {
            characterController.PerformAttack();
        }
    }
}
