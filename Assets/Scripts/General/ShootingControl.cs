using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ShootingControl : MonoBehaviour, iFactionControlReciever, iAnimationCaller
{
    [Header("Shoot Vars")]
    public float maxFireRange = 1000;
    public LayerMask shootMask;
    public int weaponDamage = 5;

    FactionManager factionManager;

    [Header("Shoot Origin")]
    public Transform firePoint;
    public bool useCamera = false; //Use this for the player
    public Transform viewCamera;

    [Header("Shoot Timing")]
    public bool useInternalTimer = false; //Use this for the player
    public float timeBetweenShots = 0.2f;
    float timeToNextShot = 0f;

    [Header("Ammo Handling")]
    public bool handleAmmo = false; //If this is false, will not use ammo system
    public int maxAmmo = 50;
    public int startingAmmo = -1; //-1 means set current ammo to max ammo
    int currentAmmo = 0;

    [Header("Effects")]
    public GameObject projectileFlashPrefab;
    public GameObject projectileObjPrefab;
    public GameObject projectileHitPrefab;
    CharacterAnimationHandler animationHandler;
    //--List<CharacterAnimationHandler> animationHandlers = new List<CharacterAnimationHandler>();

    [Header("GUI")]
    public AmmoDisplayController ammoDisplay;

    public void InjectFactionControl(FactionManager factionManager)
    {
        this.factionManager = factionManager;
    }

    void iAnimationCaller.InjectAnimationHandler(CharacterAnimationHandler animationHandler)
    {
        this.animationHandler = animationHandler;
        //--animationHandlers.Add(animationHandler);
    }

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        if(handleAmmo)
        {
            InitAmmo();
        }

        InjectReceivers();
    }


    void InjectReceivers()
    {
        List<iShootControlReviever> recievers = new List<iShootControlReviever>();

        gameObject.GetComponents(recievers);

        for (int i = 0; i < recievers.Count; i++)
        {
            recievers[i].InjectShootControl(this);
        }
    }

    #region Ammo
    void InitAmmo()
    {
        SetStartingAmmo();
        if (ammoDisplay != null)
        {
            IntialiseAmmoGUI();
        }
    }

    void SetStartingAmmo()
    {
        if(startingAmmo <= -1)
        {
            currentAmmo = maxAmmo;
        }
        else
        {
            currentAmmo = startingAmmo;
        }
    }

    void IntialiseAmmoGUI()
    {
        ammoDisplay.UpdateGunName("Rifle");
        //ammoDisplay.UpdateAmmo(currentAmmo, maxAmmo);
        UpdateAmmo(0);
    }

    public void UpdateAmmo(int changeValue)
    {
        currentAmmo += changeValue;
        currentAmmo = Mathf.Clamp(currentAmmo, 0, maxAmmo);//Ammo can nver be below 0 or higher than max ammo

        //Update ammo GUI
        ammoDisplay.UpdateAmmo(currentAmmo, maxAmmo);
    }
    #endregion

    public void shoot()
    {
        if(IsClipEmpty())
        {
            Effect_DryFireAmmo(); //Play empty clip sound effect
        }

        if (!CanFireNextShot())//No ammo
        {
            return; //The rest of the function dosn't run
        }

        Effect_Fire();


        //Raycast
        // <IF there are any problem delete ne raycasthit>
        RaycastHit hit = new RaycastHit();
        bool shotDealtDamage = false;

        Vector3 fireOrigin = firePoint.position;
        Vector3 fireDirection = firePoint.forward;

        if(CanFireUsingCamera())
        {
            fireOrigin = viewCamera.position;
            fireDirection = viewCamera.forward;
        }


        //foward = (0,0,1)
        bool didHit = Physics.Raycast(fireOrigin, fireDirection, out hit, 1000f, shootMask);

        Vector3 bulletEndPoint = hit.point;


        //Hit handling
        if (didHit == true) //If it hit something
        {
            shotDealtDamage = ResolveShot(hit); //Returned true if we dealt damage
        }
        else //If it missed something
        {
            bulletEndPoint = fireOrigin + (fireDirection * 1000f);
            //--bulletEndPoint = firePoint.position + (firePoint.forward * 1000f);
            //--print("Player shot at nothing");
        }

        Effect_Projectile(bulletEndPoint);

        if (!shotDealtDamage)
        {
            Effect_Hit(hit.point);
        }

        if (handleAmmo)
        {
            UpdateAmmo(-1);
        }
        timeToNextShot = Time.time + timeBetweenShots; //Time.time = time since game, started running.
    }

    bool ResolveShot(RaycastHit hit)
    {
        bool dealtDamage = false;
        CharacterHealth enemyHealth = hit.collider.GetComponent<CharacterHealth>();
        if (CanDamageHitTarget(enemyHealth))
        {
            enemyHealth.TakeDamage(weaponDamage);
            dealtDamage = true;
        }
        //--print($"player shot {hit.collider.gameObject.name} at point {hit.point}");

        return dealtDamage;
    }

    

    #region Effects
    void Effect_Fire()
    {
        if (animationHandler != null)
        {
            animationHandler.SetTrigger("shoot");
        }

        if(projectileFlashPrefab != null)
        {
            Instantiate(projectileFlashPrefab, firePoint);
        }
    }

    void Effect_Hit(Vector3 hitPoint)
    {
        if(projectileHitPrefab != null)
        {
            Instantiate(projectileHitPrefab, hitPoint, Quaternion.identity);
        }
    }

    void Effect_Projectile(Vector3 endPoint)
    {
        GameObject newBullet = Instantiate(projectileObjPrefab, firePoint.position, Quaternion.identity);


        //After effects and variable handling
        newBullet.GetComponent<BulletTrailControl>().Init(endPoint);
    }

    void Effect_DryFireAmmo()
    {
        //Add sound effect later.
    }
    #endregion


    #region Conditions

    public bool CanReceiveAmmo()
    {
        return handleAmmo && currentAmmo < maxAmmo;
    }

    bool CanFireNextShot()
    {
        return HasRecoiledFromLastShot() && HasAmmo();
    }

    bool HasRecoiledFromLastShot()
    {
        return !useInternalTimer || useInternalTimer && Time.time > timeToNextShot;
    }

    bool HasAmmo()
    {
        return !handleAmmo || handleAmmo && currentAmmo > 0;
    }

    bool IsClipEmpty()
    {
        return useCamera && currentAmmo <= 0;
    }

    bool CanFireUsingCamera()
    {
        return useCamera && viewCamera != null;
    }

    bool CanDamageHitTarget(CharacterHealth enemyHealth)
    {
        return enemyHealth != null 
            && (factionManager == null || enemyHealth.CanReceiveDamage(factionManager.faction));
    }



    #endregion















    //public bool ReceivePickUp(PickUp pickup)
    //{
    //    if (pickup.GetType() == typeof(AmmoPickup)) //If our pickup is an ammo pickup
    //    {
    //        return PickUpAmmo(pickup);
    //    }
    //    //else if(pickup.GetType() == typeof(DamagePickUp))
    //    //{
    //    //    //Do code for receiving damage pickup
    //    //}

    //    return false;
    //}

    //bool PickUpAmmo(PickUp pickup)
    //{
    //    if (currentAmmo < maxAmmo)
    //    {
    //        UpdateAmmo((int)pickup.value);
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //}


}
