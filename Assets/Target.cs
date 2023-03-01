using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//testing for taking attack
namespace ProjectPixel.Character.Enemy
{
    public class Target : MonoBehaviour, IAttackable
    {
        public void TakeDamage()
        {
            Debug.Log(name + " was damaged");
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}