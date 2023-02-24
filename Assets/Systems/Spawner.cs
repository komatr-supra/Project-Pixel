using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;
    GameObject player;

    public void SpawnPlayer(Vector3 position)
    {
        player = Instantiate(playerPrefab, position, Quaternion.identity);
    }

    internal Transform GetPlayersTransform()
    {
        return player.transform;
    }
}
