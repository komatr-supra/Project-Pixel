using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//interface for attackable items. enemy, barel, box, door....
public interface IAttackable
{
    public void TakeDamage();
}
