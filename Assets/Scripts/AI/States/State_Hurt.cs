using UnityEngine;

public class State_Hurt : States, IShootValuesReciever
{
    [Header("Hurt")]
    public float hurtTime = 0.3f;
    AI_ShootValues shootValues;

    [Header("Transitions")]
    public States patrolState;
    public States shootState;
    public States chaseState;

    public void InjectShootValues(AI_ShootValues shootValues)
    {
        this.shootValues = shootValues;
    }

    public override void startState()
    {
        base.startState();
        SetStateEndTime(hurtTime);
        brain.agent.isStopped = true;
    }

    public override void updateState()
    {
        if(Time.time > stateEndTime)
        {
            Endhurt();
        }
    }

    internal override void EndState(States nextState)
    {
        base.EndState(nextState);
        if (nextState != null)
        {
            brain.ChangeState(nextState);
        }
    }

    void Endhurt()
    {
        brain.agent.isStopped = false;
        if(brain.currentTargetAlive == true)
        {
            SelectStateIfTargetAlive();
        }
        else
        {
            EndState(patrolState);
        }
    }

    void SelectStateIfTargetAlive()
    {
        if(shootValues.CanGoToShootState())
        {
            EndState(shootState);
        }
        else
        {
            EndState(chaseState);
        }
    }
}
