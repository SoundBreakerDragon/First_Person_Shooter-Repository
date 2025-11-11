using System;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Spawner : MonoBehaviour
{
    [SerializeField] private Transform[] spawners;

    [SerializeField] private GameObject enemy_type;

    public int spawner_number;
    public List<Transform> patrolPoints;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.E))
        {
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        print("Debug text");
        Instantiate(enemy_type, spawners[spawner_number].position, spawners[spawner_number].rotation);
        //patrolPoints = State_Patrol.instance.GetComponent<Transform>();
    }
}
