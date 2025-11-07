using UnityEngine;

public class Spawn_Enemies : MonoBehaviour
{
    [Header("Spawn the enemy")]
    public GameObject Enemytype;
    public int enemyAmount;

    [Header("Debuging options")]
    public float spawnDelayTimer = 2f;

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
        Instantiate(Enemytype, Enemytype.transform.position, Quaternion.identity);

        //Try and figure out how to add state patrol to this.
        //GetComponents<State_Patrol> getComponents = GetComponents<State_Patrol>();
    }
}
