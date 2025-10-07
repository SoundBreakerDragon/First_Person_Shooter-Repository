using UnityEngine;

public class State_Alert : States
{
    [Header("Alert")]
    public float alertTime = 0.666666f;

    [Header("Transitions")]
    public States chaseState;

    public override void startState()
    {
        base.startState();
        SetStateEndTime(alertTime);
        RunAlertEffects();
    }

    void RunAlertEffects()
    {
        //run startup animation / sounds here
        print("Alerted");
        brain.agent.isStopped = true;
    }

    public override void updateState()
    {
        base.updateState();
        if(Time.time > stateEndTime)
        {
            EndState();
        }
    }

    internal override void EndState()
    {
        base.EndState();
        if (chaseState != null) //if chase state has been assigned
        {
            brain.ChangeState(chaseState);
        }
        else //if it hasn't
        {
            Debug.LogWarning($"Chase state variable of Alert State on {gameObject.name} has not been assigned");
        }
    }
}
