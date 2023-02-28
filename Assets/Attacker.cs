using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Character.Animator;
using System;

public class Attacker : MonoBehaviour
{
    [SerializeField] BoxCollider2D attackBox;
    [SerializeField] SimpleCharacterAnimator characterAnimator;
    bool canMoveWithThisWeapon = false;
    Vector2 size;
    private void Awake() {
        size = attackBox.size;
    }
    public void Attack(Action endAction, out bool canMoveWithAttacking)
    {
        canMoveWithAttacking = canMoveWithThisWeapon;
        characterAnimator.SetAnimation(SimpleCharacterAnimator.AnimType.attack, CallbackAnimation, endAction);
        
    }
    private void CallbackAnimation()
    {
        Collider2D[] collilidersForAttack = Physics2D.OverlapBoxAll(attackBox.bounds.center, attackBox.size, 0);
        foreach (var item in collilidersForAttack)
        {
            if(item.TryGetComponent<IAttackable>(out IAttackable target))
            {
                target.TakeDamage();
            }
        }
    }
}
