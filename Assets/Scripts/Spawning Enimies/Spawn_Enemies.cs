using System.Collections.Generic;
using UnityEngine;

public class Spawn_Enemies : MonoBehaviour
{
    [Header("Spawn the enemy")]
    public GameObject Enemytype;
    public int enemyAmount;

    [Header("Debuging options")]
    public float spawnDelayTimer = 2f;

    [Header("Patrol point for patrolling enimies")]
    public List<Transform> patrolPoints;

    bool SpawnCompleted = false;

    //This will be hadle by the tigger GameObject later on.
    bool playerDetected = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //void Start()
    //{
    //    
    //}

    // Update is called once per frame
    void Update()
    {
        //if (playerDetected == true)
        //{
        //    spawnEnemy();
        //}
        if (Input.GetKeyUp(KeyCode.E))
        {
            spawnEnemy();
        }
    }

    private void spawnEnemy()
    {
        Instantiate(Enemytype, transform.position, transform.rotation);
        

        //Try and figure out how to add state patrol to this.

        //GetComponents<Patrol_points>
    }
}
