using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class AI_ShootValues : MonoBehaviour, IBrainReceiver
{
    public float shootableRange = 10;

    AIBrain brain;

    public void InjectBrain(AIBrain brian)
    {
        this.brain = brian;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InjectShootValues();
    }

    void InjectShootValues()
    {
        List<IShootValuesReciever> valueReceivers = new List<IShootValuesReciever>();
        
        gameObject.GetComponents(valueReceivers);

        for (int i = 0; i < valueReceivers.Count; i++)
        {
            valueReceivers[i].InjectShootValues(this);
        }
    }

    public bool CanGoToShootState()
    {
        return brain.targetVisible && IsTargetInShootRange();
    }

    public bool IsTargetInShootRange()
    {
        return brain.vectorToTarget.magnitude < shootableRange;
    }
}
