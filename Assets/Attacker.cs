using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Character.Animator;
public class Attacker : MonoBehaviour
{
    
    void Update()
    {
        if(Input.GetButtonDown("Attack")) GetComponent<SimpleCharacterAnimator>().AttackTrigger();
    }
}
