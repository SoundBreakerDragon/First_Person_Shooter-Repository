using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class State_Patrol : States
{
    [Header("Patrolling")]
    public List<Transform> patrolPoints;
    public float detectionDistance = 20f;
    public float patrolStoppingDistance = 1f;

    int currentPatrolPoint = 0;

    [Header("Patrol Pausing")]
    public bool patrolPauseAtPoints = false;
    public float patrolPauseTime = 1.5f;

    bool patrolIsPaused = false;
    float patrolUnpauseTime = 0f; //Controls when we move next when paused

    [Header("Transitions")]
    public States alertState;

    #region State Running

    public override void startState()
    {
        base.startState();
        currentPatrolPoint = GetNearestPatrolPointIndex();
        brain.agent.SetDestination(patrolPoints[currentPatrolPoint].position);


    }

    /// <summary>
    /// Finds the nearest Patrol point and finds out what index it is in our list
    /// </summary>
    /// <returns></returns>
    int GetNearestPatrolPointIndex()
    {
        int closestPoint = -1; //-1 is a useful starting value because a list does not have a valid index value of -1
        float closestDistance = Mathf.Infinity; //For the first cycle in the loop
        //because any distance will be less than infinity

        for(int i = 0; i < patrolPoints.Count; i ++)
        {
            float currentDistance = Vector3.Distance(transform.position, patrolPoints[i].position);
            if(currentDistance < closestDistance)
            {
                closestDistance = currentDistance;
                closestPoint = i;
            }

        }


        return closestPoint;
    }

    public override void updateState()
    {
        if(CanGoToAlertFromPatrol())
        {
            EndState();
        }
        else if(patrolPauseAtPoints)
        {
            ProcessPausingPatrol();
        }
        else
        {
            ProcessContinousPatrol();
        }
    }


    internal override void EndState()
    {
        base.EndState();

        if (alertState != null) //if chase state has been assigned
        {
            brain.ChangeState(alertState);
        }
        else //if it hasn't
        {
            Debug.LogWarning($"Alert state variable of Patrol State on {gameObject.name} has not been assigned");
        }
    }

    #endregion

    #region Partrol loop

    void ProcessPausingPatrol()
    {
        if(CanPausePatrol())
        {
            PausePatrol();
        }
        else if (CanUnpausePatrol())
        {
            ContinuePatrol();
        }
    }

    void PausePatrol()
    {
        patrolIsPaused = true;
        patrolUnpauseTime = Time.time + patrolPauseTime; //The current moment in time, plus a few seconds.
    }

    void ProcessContinousPatrol()
    {
        if(IsInRangeOfCurrentPatrolPoint())
        {
            ContinuePatrol();
        }
    }

    void ContinuePatrol()
    {
        patrolIsPaused = false;
        IncrementPatrolPoint();
        brain.agent.SetDestination(patrolPoints[currentPatrolPoint].position);
    }

    /// <summary>
    /// Clamps the current PatrolPoint variable to prevent it from going outside the range of the Patrol Points list
    /// </summary>
    void IncrementPatrolPoint()
    {
        currentPatrolPoint += 1;
        if (currentPatrolPoint >= patrolPoints.Count)
        {
            currentPatrolPoint = 0;
        }
    }

    #endregion


    #region Conditions

    bool CanGoToAlertFromPatrol ()
    {
        return brain.targetVisible 
            && brain.vectorToTarget.magnitude < brain.detectionDistance;
    }

    bool IsInRangeOfCurrentPatrolPoint()
    {
        return Vector3.Distance(transform.position, GetHeightAdjustedPatrolPosition()) < patrolStoppingDistance;
    }

    Vector3 GetHeightAdjustedPatrolPosition()
    {
        Vector3 patrolPosition = patrolPoints[currentPatrolPoint].position;
        return new Vector3(patrolPosition.x, patrolPosition.y + brain.agent.baseOffset, patrolPosition.z);
    }

    bool CanPausePatrol()
    {
        return patrolIsPaused == false && IsInRangeOfCurrentPatrolPoint();
    }

    bool CanUnpausePatrol()
    {
        return Time.time > patrolUnpauseTime && patrolIsPaused == true;
    }

    #endregion
}
