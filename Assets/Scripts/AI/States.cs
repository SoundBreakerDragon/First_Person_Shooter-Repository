using UnityEngine;

public class States : MonoBehaviour, IBrainReceiver
{
    internal bool hasStartedUp = false;
    internal AIBrain brain;
    internal float stateEndTime = 0f;

    internal void SetStateEndTime(float endTime)
    {
        stateEndTime = Time.time + endTime;
    }



    public void InjectBrain(AIBrain brian)
    {
        this.brain = brian;
    }

    public virtual void startState() //State entry class
    {
        hasStartedUp = true;
    }

    public virtual void updateState() //State update class
    {
        
    }

    /// <summary>
    /// To use if you ony have one state to transition to.
    /// </summary>
    internal virtual void EndState() //State exit class
    {
        hasStartedUp = false;
    }

    /// <summary>
    /// To use when you have multiple states that you could transition to
    /// </summary>
    /// <param name="nextState"></param>
    internal virtual void EndState(States nextState)
    {
        hasStartedUp = false;
    }
}