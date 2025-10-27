using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationHandler : MonoBehaviour
{
    public Animator targetAnimator;

    private void Start()
    {
        List<iAnimationCaller> animationCallers = new List<iAnimationCaller>();
        GetComponents(animationCallers);

        for (int i = 0;  i < animationCallers.Count; i++)
        {
            animationCallers[i].InjectAnimationHandler(this);
        }
    }

    public void ShootAnimation()
    {
        targetAnimator.SetTrigger("shoot");
    }

    public void SetTrigger(string trigger)
    {
        targetAnimator.SetTrigger(trigger);
    }

    
}
