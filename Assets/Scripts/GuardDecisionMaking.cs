using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardDecisionMaking : MonoBehaviour
{
    Guard guard;
    public StateMachine sm;
    public string currentAction;
    // Start is called before the first frame update
    void Start()
    {
        guard = GetComponent<Guard>();
        PatrolState patrol = new PatrolState(guard);
        sm = new StateMachine(guard, patrol);
        currentAction = sm.currentState.action;
    }

    // Update is called once per frame
    void Update()
    {
        sm.Update();
        currentAction = sm.currentState.action;
    }
}
