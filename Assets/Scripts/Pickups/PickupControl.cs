using JetBrains.Annotations;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class PickupControl : MonoBehaviour, iShootControlReviever
{
    ShootingControl shootConrol;
    public Keycarddisplaycontroller displayReciever;
    public DoorDetectsKeycardA DoorController;

    [HideInInspector]
    public bool Gotkeycard1 = false;
    public bool Gotkeycard2 = false;
    public bool Gotkeycard3 = false;

    public void InjectShootControl(ShootingControl shootControl)
    {
        this.shootConrol = shootControl;
    }

    public bool ReceivePickUp(PickUp pickup)
    {
        if (pickup.GetType() == typeof(AmmoPickup)) //If our pickup is an ammo pickup
        {
            return PickUpAmmo(pickup);
        }
        else if (pickup.GetType()== typeof(KeycardPickup))
        {

            for (int i = 0; i < pickup.Keycards.Count; i++)
            {
                if (pickup.Keycards[i].gameObject == pickup.Keycards[0].gameObject && Gotkeycard1 != true)//GameObject.Find("Keycard A"))
                {
                    Gotkeycard1 = true;
                    print("Got the first key card");
                    displayReciever.UpdateKeycardDisplay(this);
                    DoorController.DetectingCard(this);
                    return true;
                }
                else if (pickup.Keycards[i].gameObject == pickup.Keycards[1].gameObject && Gotkeycard2 != true)
                {
                    Gotkeycard2 = true;
                    print("Got the second key card");
                    displayReciever.UpdateKeycardDisplay(this);
                    DoorController.DetectingCard(this);
                    return true;
                }
            }
            return true;
        }

        return false;
    }

    bool PickUpAmmo(PickUp pickup)
    {
        if (shootConrol.CanReceiveAmmo())
        {
            shootConrol.UpdateAmmo((int)pickup.value);
            return true;
        }
        else
        {
            return false;
        }
    }
}
