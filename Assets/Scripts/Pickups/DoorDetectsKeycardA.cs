using UnityEngine;

public class DoorDetectsKeycardA : MonoBehaviour
{
    [Header("Keycard doors")]
    public GameObject Door1;

    bool foundKeycard1 = false;
    bool foundKeycard2 = false;
    bool foundKeycard3 = false;

    private void OnTriggerEnter(Collider other) // When an object intersects our trigger, this is call. The collider "other" is the object that has intersected us.
    {
        if (other.tag == "Player")
        {
            if (foundKeycard1)
            {
                Door1.SetActive(false);
            }
        }
    }

    public void DetectingCard(PickupControl control)
    {
        if (control.Gotkeycard1)
        {
            foundKeycard1 = true;
        }
        if (control.Gotkeycard2)
        {
            foundKeycard2 = true;
        }
        if (control.Gotkeycard3)
        {
            foundKeycard3 = true;
        }
    }
}
