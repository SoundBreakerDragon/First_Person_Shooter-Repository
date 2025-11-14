using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Enemies : MonoBehaviour
{
    [Header("Spawn the enemy")]
    public GameObject Enemytype;
    public int enemyAmount = 1;
    int enemySpawned = 0;

    [Header("Debuging options")]
    public float spawnDelayTimer = 2f;
    float spawnTime;
    float timer;

    [Header("Patrol point for patrolling enimies")]
    public List<Transform> patrolPoints;

    
    bool SpawnCompleted = false;

    //This will be hadle by the tigger GameObject later on.
    bool playerDetected = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ResetSpawnTime();
    }

    void ResetSpawnTime()
    {
        spawnTime = Time.time + spawnDelayTimer;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemySpawned < enemyAmount)
        {
            timer += Time.deltaTime;

            if (timer > spawnTime)
            {
                spawnEnemy();
                ResetSpawnTime();
            }
        }
    }

    private void spawnEnemy()
    {
        //Spawns the enemy
        GameObject enemy = Instantiate(Enemytype, transform.position, transform.rotation);
        enemySpawned ++;

        //Inserts patrol points into the patrol list in State_Patrol
        State_Patrol patrol = enemy.GetComponent<State_Patrol>();
        patrol.patrolPoints = patrolPoints;


    }


}
