using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.AI;

public class AIBrain : MonoBehaviour, IHealthUpdateReceiver
{
    [Header("References")]
    [HideInInspector]
    public GameObject target;
    [HideInInspector]
    public NavMeshAgent agent;

    [Header("State Management")]
    public States startState;
    List<States> states = new List<States>();
    [SerializeField] 
    States currentState;

    bool newStateStarted = false;

    [Header("Sensors")]
    [Header("Sensor Controls")]
    public float sightAngle = 60;
    public float detectionDistance = 20f;

    [Header("Sensor Values")]
    //[HideInInspector]
    public Vector3 vectorToTarget = Vector3.zero;
    public bool targetVisible = false;
    public Vector3 lastTargetPosition = Vector3.zero;
    public bool currentTargetAlive = true;
    CharacterHealth targetHealth;

    [Header("Sight Sensor Controls")]
    public Transform eyePoint;
    public float eyeRadius = 0.5f;
    public LayerMask sightMask;

    [Header("Event States")]
    public States hurtState;
    public States deathState;

    public void GetHurt(int currentHealth, int maxHealth)
    {
        print($"Received damage on {gameObject.name}");
        if (hurtState != null
            && currentState.GetType() != typeof(State_Death))
        {
            ChangeState(hurtState);
        }
    }

    public void GetHealed(int currentHealth, int maxHealth)
    {
        throw new System.NotImplementedException();
    }

    public void GetKilled()
    {
        if(deathState != null 
            && currentState.GetType() != typeof(State_Death))
        {
            ChangeState(deathState);
        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Init();
    }

    void Init()
    {
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player");

        InjectBrain();

        gameObject.GetComponents<States>(states); //Populate our states list with all of the states on our object

        if (startState != null && states.Contains(startState))
        {
            currentState = startState;
        }
        else
        {
            currentState = states[0];
        }

        startNewState();
    }

    void InjectBrain()
    {
        List<IBrainReceiver> brainReceivers = new List<IBrainReceiver>();

        gameObject.GetComponents(brainReceivers);

        for (int i = 0; i < brainReceivers.Count; i++)
        {
            brainReceivers[i].InjectBrain(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        RunSensors();
        RunState();
    }

    #region Sensors
    void RunSensors()
    {
        currentTargetAlive = IsTargetAlive();
        vectorToTarget = GetVectorToTarget();
        targetVisible = IsTargetVisible();
        if (targetVisible)
        {
            lastTargetPosition = target.transform.position;
        }

        //-- vectorToTarget.magnitude *Distance to the target
        //-- vectorToTarget.normalized *Direction to the target
    }


    // -=-=-=-=-=-=-=- Vector (Distance) to target (Player) -=-=-=-=-=-=-=- //

    /// <summary>
    /// Target point - currentPoint = vector between 2 points
    /// </summary>
    /// <returns></returns>
    Vector3 GetVectorToTarget()
    {
        return (target.transform.position - transform.position);
    }

    #region Visibility
    // -=-=-=-=-=-=-=- Checking if the target (Player) is visible -=-=-=-=-=-=-=- //

    /// <summary>
    /// Return true if the target is within the vision anagle and a certain amount of them can be seen
    /// </summary>
    /// <returns></returns>
    bool IsTargetVisible()
    {
        bool visible = false;

        float currentAngle = Vector3.Angle(transform.forward, vectorToTarget.normalized);

        if (currentAngle < sightAngle)
        {
            visible = TestTargetVisible();
        }
        return visible;
    }

    /// <summary>
    /// Tests if a sphere cast can hit the Target. Otherwise they are considered to be too obscrured
    /// </summary>
    /// <returns></returns>
    bool TestTargetVisible()
    {
        RaycastHit hit;
        bool didHit = Physics.SphereCast(eyePoint.position,
            eyeRadius,
            vectorToTarget.normalized,
            out hit,
            detectionDistance,
            sightMask);

        return DidHitTarget(didHit, hit);
    }
    #endregion

    // -=-=-=-=-=-=-=- Checking if the target (Player) is currently alive -=-=-=-=-=-=-=- //
    bool IsTargetAlive()
    {
        bool isAlive = false;

        if (targetHealth == null) //Tries to find the component
        {
            targetHealth = target.GetComponent<CharacterHealth>();
        }
        if (targetHealth != null) // If the component is there, check if it's alive
        {
            isAlive = true;
        }
        return isAlive;
    }




    #endregion

    #region State Running

    void RunState()
    {
        if (!newStateStarted)
        {
            startNewState();
        }
        else
        {
            currentState.updateState();
        }
    }

    /// <summary>
    /// Call this to change what State is being run by the brain
    /// </summary>
    /// <param name="nextState"></param>
    /// <returns></returns>
    public void ChangeState(States nextState)
    {
        currentState = nextState;
        newStateStarted = false;
    }

    void startNewState()
    {
        currentState.startState();
        newStateStarted = true;
    }

    #endregion

    #region Conditions

    bool DidHitTarget(bool didHit, RaycastHit hit)
    {
        return didHit && hit.collider.gameObject == target;
    }

    #endregion
}
