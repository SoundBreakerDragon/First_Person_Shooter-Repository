using UnityEngine;

public class BulletTrailControl : MonoBehaviour
{
    public float speed = 50f;
    Vector3 endPoint;

    public void Init (Vector3 endPoint)
    {
        this.endPoint = endPoint;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 difference = endPoint - transform.position; //Durection from our position to the endpoint as a vector
        //The magnitude / length of that vector will be the distance between these two points.
        
        if (difference.magnitude > 0f)
        {
            MoveTrail(difference);
        }
    }

    void MoveTrail (Vector3 difference)
    {
        Vector3 translation = difference.normalized * speed * Time.deltaTime;

        if (translation.magnitude > difference.magnitude)//If our distance is less than how far we woul move normally
        {
            transform.Translate(difference);
        }
        else
        {
            transform.Translate(translation);
        }
    }

}
