using UnityEngine;
using UnityStandardAssets.Effects;

public class State_Shoot : States, iShootControlReviever, IShootValuesReciever
{
    [Header("Shoot Vars")]
    public float timeBetweenShots = 0.5f;
    float timeToNextShot = 0f;
    int currentShotCount = 0;
    AI_ShootValues shootValues;
    ShootingControl shootControl;

    [Header("Shoot Roatation")]
    public float rotateSpeed = 120f;
    public float rotateMinimumAngle = 5;
    float currentRotateSpeed = 0f;

    [Header("Transition")]
    public States chaseState;
    public States patrolState;

    public void InjectShootControl(ShootingControl shootControl)
    {
        this.shootControl = shootControl;
    }

    public void InjectShootValues(AI_ShootValues shootValues)
    {
        this.shootValues = shootValues;
    }



    #region State Management

    public override void startState()
    {
        base.startState();
        currentShotCount = 0;
        brain.agent.isStopped = true;
    }

    public override void updateState()
    {
        if(CanLoseShootTarget())
        {
            EndState(chaseState);
        }
        else if(!brain.currentTargetAlive)
        {
            EndState(patrolState);
        }
        else if (Time.time > timeToNextShot)
        {
            Fire();
        }
        else
        {
            PointToTarget();
        }
    }

    internal override void EndState(States nextState)
    {
        brain.agent.isStopped = false;


        base.EndState(nextState);
        if(nextState != null)
        {
            brain.ChangeState(nextState);
        }
    }

    #endregion

    #region Loop

    void Fire()
    {
        shootControl.shoot();
        currentShotCount++;

        timeToNextShot = Time.time + timeBetweenShots;
    }

    void PointToTarget()
    {
        Vector3 heightAdjustedTarget = brain.target.transform.position;
        heightAdjustedTarget.y = shootControl.firePoint.position.y;
        Vector3 targetDirection = (heightAdjustedTarget - shootControl.firePoint.position).normalized;
        float angleToTarget = Vector3.SignedAngle(shootControl.firePoint.forward, targetDirection, transform.up);

        if(Mathf.Abs(angleToTarget) > rotateMinimumAngle) //Abs takes one number and makes it positive
        {
            RotateActor(angleToTarget);
        }
        //else
        //{
        //    print("Stopped"); //for debugging just in case
        //}
    }

    void RotateActor(float angleToTarget)
    {
        currentRotateSpeed = rotateSpeed * (Mathf.Sign(angleToTarget)); //Sign returns 1 if value is positive and -1 if value is negative
        float nextRotation = currentRotateSpeed * Time.deltaTime;

        if(WillRotationOverShoot(angleToTarget, nextRotation))
        {
            nextRotation = angleToTarget;
        }

        transform.Rotate(transform.up, nextRotation);
    }

    #endregion

    #region Conditions

    bool CanLoseShootTarget()
    {
        return (!brain.targetVisible
            || brain.targetVisible && shootValues.IsTargetInShootRange() == false) 
            && currentShotCount > 0;
    }

    bool WillRotationOverShoot(float angleToTarget, float nextRotation)
    {
        return Mathf.Sign(angleToTarget) != Mathf.Sign(currentRotateSpeed)
            && Mathf.Abs(angleToTarget) < Mathf.Abs(nextRotation);
    }

    #endregion
}
