using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class State_Chase : States, IShootValuesReciever
{
    [Header("Chase")]
    public float chaseRepathRate = 0.1f;
    public float tagetLostResetTime = 5f;

    float nextRepath = 0f;
    float targetLostResetEnd = 0f;
    bool targetLost = false;

    AI_ShootValues ShootValues;

    [Header("Transitions")]
    public States shootState;
    public States patrolState;

    public void InjectShootValues(AI_ShootValues shootValues)
    {
        this.ShootValues = shootValues;
    }

    #region State Running

    public override void startState()
    {
        base.startState();
        brain.agent.isStopped = false;
        FollowPlayer();
    }

    public override void updateState()
    {
        if(ShootValues.CanGoToShootState())
        {
            EndState(shootState);
        }
        else if(!brain.targetVisible)
        {
            LookForLostPlayer();
        }
        else if(CanRepath())
        {
            FollowPlayer();
        }


    }

    internal override void EndState(States nextState)
    {
        base.EndState(nextState);
        if (nextState != null)
        {
            brain.ChangeState(nextState);
        }
        else
        {
            Debug.LogWarning($"Next state parameter of End State in Chase State on{gameObject.name} was null");
        }
    }

    #endregion

    #region Chase Loop

    void LookForLostPlayer()
    {
        if (CanRepath())
        { 
            brain.agent.SetDestination(brain.lastTargetPosition);
            UpdateRepathTime();
        }

        if(targetLost == false) //first time looking for lost player
        {
            LoseTarget();
        }
        else if(IsTargetFullyLost())
        {
            EndState(patrolState);
        }
    }

    void LoseTarget()
    {
        targetLost = true;
        targetLostResetEnd = Time.time + tagetLostResetTime;
    }

    /// <summary>
    /// Sets the current Navmesh Agent Destination to the target's current position and updates repath time
    /// </summary>
    void FollowPlayer()
    {
        if(targetLost == true)
        {
            targetLost = false;
        }
        brain.agent.SetDestination(brain.target.transform.position);
        UpdateRepathTime();
    }

    void UpdateRepathTime()
    {
        nextRepath = Time.time + chaseRepathRate;
    }

    #endregion

    #region Conditions

    bool CanRepath()
    {
        return Time.time > nextRepath;
    }

    bool IsTargetFullyLost()
    {
        return targetLost && Time.time > targetLostResetEnd;
    }

    #endregion
}
