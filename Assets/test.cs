using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class test : MonoBehaviour
{
    [SerializeField] float speed = 10;

    void Update()
    {
        transform.position += new Vector3(Input.GetAxis("Horizontal"),
                Input.GetAxis("Vertical"),
                0) * speed * Time.deltaTime;
    }
}
