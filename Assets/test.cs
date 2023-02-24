using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class test : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera cinemachineVirtualCamera;
    [SerializeField] Spawner spawner;
    void Start()
    {
        //cinemachineVirtualCamera.m_Follow = spawner.GetPlayersTransform();

    }

    void Update()
    {
        
    }
}
