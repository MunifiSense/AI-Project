using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardDecisionMaking : MonoBehaviour
{
    Guard guard;
    // AI States
    TargetState patrol;
    TargetState raiseAlarm;
    TargetState attack;
    TargetState waitForBackup;

    // AI Decision Trees
    DecisionTreeNode decisionPatrol;
    DecisionTreeNode decisionRaiseAlarm;
    DecisionTreeNode decisionAttack;
    DecisionTreeNode decisionWaitForBackup;


    // Start is called before the first frame update
    void Start()
    {
        guard = GetComponent<Guard>();

        // State setup

        // Patrol state
        State statePatrol = new State(new List<string>(new string[] { "patrol" }),
            new List<string>(new string[] { }),
            new List<string>(new string[] { }),
            new List<Transition>(new Transition[] { }));
        patrol = new TargetState(guard, statePatrol);

        // Alarm state
        State stateAlarm = new State(new List<string>(new string[] { "alarm" }),
            new List<string>(new string[] { }),
            new List<string>(new string[] { }),
            new List<Transition>(new Transition[] { }));
        raiseAlarm = new TargetState(guard, stateAlarm);

        // Attack state
        State stateAttack = new AttackState();
        attack = new TargetState(guard, stateAttack);

        // Wait state
        State stateWait = new State(new List<string>(new string[] { "wait" }),
            new List<string>(new string[] { }),
            new List<string>(new string[] { }),
            new List<Transition>(new Transition[] { }));
        waitForBackup = new TargetState(guard, stateWait);

        // Decision Tree Setup

        // Patrol Decision Tree
        AggressiveDecision a = new AggressiveDecision(guard);
        a.trueNode = raiseAlarm;
        a.falseNode = attack;

        NearbyDecision b = new NearbyDecision(guard);
        b.trueNode = a;
        b.falseNode = attack;

        SpottedDecision c = new SpottedDecision(guard);
        c.trueNode = b;
        c.falseNode = null;

        decisionPatrol = c;

        // Alarm Decision Tree
        BackupDecision d = new BackupDecision(guard);
        d.trueNode = attack;
        d.falseNode = waitForBackup;
        AggressiveDecision e = new AggressiveDecision(guard);
        e.trueNode = d;
        e.falseNode = patrol;

        decisionRaiseAlarm = e;

        // Backup Tree
        BackupDecision f = new BackupDecision(guard);
        f.trueNode = attack;
        f.falseNode = null;

        decisionWaitForBackup = f;

        // Attack Tree
        SpottedDecision g = new SpottedDecision(guard);
        f.trueNode = null;
        f.falseNode = patrol;

        decisionAttack = g;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
