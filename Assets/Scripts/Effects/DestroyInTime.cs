using UnityEngine;

public class DestroyInTime : MonoBehaviour
{
    public float destroyTime = 0.33333f; //Aproximately 1 frame at 30 frames per second
    float endTime = 0f;


    void Start()
    {
        endTime = Time.time + destroyTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > endTime)
        {
            Destroy(gameObject);
        }
    }
}
