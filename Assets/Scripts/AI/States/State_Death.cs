using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class State_Death : States
{
    [Header("Object handling")]
    public List<Component> deathClearList = new List<Component> ();

    [Header("Layer Switching")]
    public string deathLayer = "Ignore Player";
    public List<GameObject> layerChangeList = new List<GameObject>();

    [Header("Effects")]
    public float bodyRemainTime = 2; //How long the body stays in the game
    public float fadeTime = 2; //Fadeout time if fade gets implemented
    public float deathBounce = 2f;
    public float deathMass = 5f;
    public float deathForceAngleMultiplier = 0.1f;

    bool fadeoutStarted = false;

    public override void startState()
    {
        base.startState();
        SetStateEndTime(bodyRemainTime);

        for (int i = 0; i < deathClearList.Count; ++i)
        {
            Destroy(deathClearList[i]);
        }

        for (int i = 0; i < layerChangeList.Count;  i++)
        {
            layerChangeList[i].layer = LayerMask.NameToLayer(deathLayer);
        }

        ApplyDeathEffect();
    }

    void ApplyDeathEffect()
    {
        Rigidbody deathRigidbody = gameObject.AddComponent<Rigidbody>();
        deathRigidbody.mass = deathMass;
        deathRigidbody.AddForce(GetDeathForceAngle() * deathBounce);
    }

    public override void updateState()
    {
        if(CanStartFadeOut())
        {
            startFadeOut();
        }
        else if (IsFadeoutDone())
        {
            Destroy(gameObject);
        }
    }

    Vector3 GetDeathForceAngle()
    {
        return (transform.up + (new Vector3(Random.Range(-1, 1),
            0f, 
            Random.Range(-1, 1f)).normalized) * deathForceAngleMultiplier).normalized;
    }

    void startFadeOut()
    {
        fadeoutStarted = true;
        SetStateEndTime(fadeTime);
    }

    #region Conditions

    bool CanStartFadeOut()
    {
        return !fadeoutStarted && Time.time > stateEndTime;
    }

    bool IsFadeoutDone()
    {
        return fadeoutStarted && Time.time > stateEndTime;
    }

    #endregion
}
