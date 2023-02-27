using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Character.Animator;

public class Attacker : MonoBehaviour
{
    ContactFilter2D contactFilter2D;
    [SerializeField] BoxCollider2D attackBox;
    [SerializeField] SimpleCharacterAnimator characterAnimator; 

    Vector2 size;
    private void Awake() {
        contactFilter2D = new ContactFilter2D();
        size = attackBox.size;
    }
    public void Attack()
    {
        
        
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
