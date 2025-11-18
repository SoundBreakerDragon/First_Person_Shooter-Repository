using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class Enemy_Spawner_trigger : MonoBehaviour
{
    bool playerDetected = false;
    public bool spawncomplete = false;
    public Spawn_Enemies callEnemySpawn;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        print("Got message");
        if (spawncomplete == false)
        {
            print("Entered loop");
            if (other.gameObject.name == "Player")
            {
                print("Sended boolean to spawnpoint");
                //Tells the spawn enemy object to spawn an enemy
                //--(For enemy spawn trigger code)--callEnemySpawn.playerDetection = true;
            }
        }

        print("End of trigger");
    }
}
