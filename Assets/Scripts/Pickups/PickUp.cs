using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for all pickups
/// </summary>

public class PickUp : MonoBehaviour
{
    public float value = 1;
    public List<GameObject> Keycards;

    void PickupObject(PickupControl pickupControl)
    {
        bool pickupReceived = pickupControl.ReceivePickUp(this);
        if (pickupReceived)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other) // When an object intersects our trigger, this is call. The collider "other" is the object that has intersected us.
    {
        if (other.tag == "Player")
        {
            PickupObject(other.GetComponent<PickupControl>());
        }
    }
}
