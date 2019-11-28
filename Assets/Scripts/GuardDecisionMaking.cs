using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardDecisionMaking : MonoBehaviour
{
    // AI States
    State patrol;
    State investigate;
    State raiseAlarm;
    State attack;
    State waitForBackup;

    // AI Decision Trees
    DecisionTreeNode decisionPatrol;
    DecisionTreeNode decisionInvestigate;
    DecisionTreeNode decisionRaiseAlarm;
    DecisionTreeNode decisionAttack;
    DecisionTreeNode decisionWaitForBackup;
    // Start is called before the first frame update
    void Start()
    {
        // State setup

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
