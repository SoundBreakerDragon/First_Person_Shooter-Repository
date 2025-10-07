using UnityEngine;

public class PickupControl : MonoBehaviour, iShootControlReviever
{
    ShootingControl shootConrol;

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
