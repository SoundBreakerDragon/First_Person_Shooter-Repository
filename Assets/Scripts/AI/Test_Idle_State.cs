using UnityEngine;

/// <summary>
/// This class is for testing that the brain is working and nothing else - only use for testing not for game logic
/// </summary>
public class Test_idle_state : States
{

    public override void startState()
    {
        base.startState();
        print("Idle started");
    }
}
