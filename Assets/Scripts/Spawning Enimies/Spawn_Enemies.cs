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

    [Header("Tranfer enemy patrol points (if any)")]
    public List<Transform> patrolPoints;

    //This area of code is planned to allow the spawners to get enabled by a trigger
    #region spawn trigger
    //bool SpawnCompleted = false;
    //[SerializeField]
    //public bool playerDetection = false;
    //int loopCount = 0;
    //bool loopCompleted = false;
    //public Enemy_Spawner_trigger enemy_Spawner_Trigger;

    //Enemy_Spawner_trigger playerDetection;
    #endregion

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
        //Delays the amount of time each enemy spawns
        if (enemySpawned < enemyAmount)
        {
            timer += Time.deltaTime;

            if (timer > spawnTime)
            {
                spawnEnemy();
                ResetSpawnTime();
            }
        }

        #region Spawn trigger version of code (Unfinished)
        //if (playerDetection == true)
        //{
        //    
        //    print("Player entered area :)");
        //    Debug.LogWarning(enemySpawned);
        //    Debug.LogWarning(enemyAmount);
        //    
        //
        //    //Delays the amount of time each enemy spawns
        //    if (enemySpawned < enemyAmount)
        //    {
        //        timer += Time.deltaTime;
        //
        //        if (timer > spawnTime)
        //        {
        //            spawnEnemy();
        //            ResetSpawnTime();
        //            loopCount ++;
        //            print(loopCount);
        //        }
        //    }
        //
        //}
        //
        //if (loopCount > enemyAmount)
        //{
        //    print("Enter output");
        //    enemy_Spawner_Trigger.spawncomplete = true;
        //    playerDetection = false;
        //}
        #endregion

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
