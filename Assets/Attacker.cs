using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectPixel.Character.Animation;
using System;
namespace ProjectPixel.Character
{
//maybe rename to WEaponSystem same base class or interface with mover???
    public class Attacker : MonoBehaviour
    {
        [SerializeField] BoxCollider2D attackBox;
        //animation should be controlled by states? -> they will know who call them
        //and can play right animation .... move, jump, idle....
        [SerializeField] SimpleCharacterAnimator characterAnimator;
        //this will be in weapon data, or in skills? 
        bool canMoveWithThisWeapon = false;
        //main entry => rename to something like Execute()?
        public void Attack(Action endAction, out bool canMoveWithAttacking)
        {
            canMoveWithAttacking = canMoveWithThisWeapon;
            characterAnimator.SetAnimation(AnimType.attack, CallbackAnimation, endAction);
        }
        //this is callback when animation hit enemy
        private void CallbackAnimation()
        {
            //this is behaviour for no weapon(fist) -> move it out? to Weapon abstract class(interface)? 
            //and filled with WeaponDataSO
            Collider2D[] collilidersForAttack = Physics2D.OverlapBoxAll(attackBox.bounds.center, attackBox.size, 0);
            foreach (var item in collilidersForAttack)
            {
                if (item.TryGetComponent<IAttackable>(out IAttackable target))
                {
                    target.TakeDamage();
                }
            }
        }
    }
}
