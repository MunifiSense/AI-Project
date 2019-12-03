using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Code loosely based off Artificial Intelliegence for Games textbook
// State machine that has Decision Trees as transitions

public class StateMachine {
    //List<State> states;
    public Guard guard;
    public State initialState;
    public State currentState;

    public StateMachine(Guard guard, State state)
    {
        //this.states = states;
        this.guard = guard;
        this.initialState = state;
        this.currentState = state;
    }

    public void Update()
    {
        if (currentState.transition.IsTriggered())
        {
            currentState.OnStateExit();
            switch((currentState.transition.GetState().state))
            {
                case "attack":
                    currentState = new AttackState(guard);
                    //Debug.Log("Attack state");
                    break;
                case "alarm":
                    currentState = new AlarmState(guard);
                    //Debug.Log("Alarm state");
                    break;
                case "wait":
                    currentState = new BackupState(guard);
                    //Debug.Log("Wait state");
                    break;
                case "patrol":
                    currentState = new PatrolState(guard);
                    //Debug.Log("Patrol state");
                    break;
            }
            currentState.OnStateEnter();
        }

        switch (currentState.action)
        {
            case "patrol":
                if (guard.Patrol(guard.patrolFrom, guard.patrolTo))
                {
                    int temp = guard.patrolFrom;
                    guard.patrolFrom = guard.patrolTo;
                    guard.patrolTo = temp;
                    guard.FindPath(guard.patrolFrom, guard.patrolTo);
                    //path.CalcParams();
                }
                break;
            case "alarm":
                // DEAL WITH ALARM CASE!!!!!!!
                break;
            case "attack":
                break;
            case "wait":
                break;
        }
    }
}

public class State : Decision
{
    public string action;
    public Transition transition;

    public State(Guard guard) : base(guard)
    {
        this.guard = guard;
        this.action = "";
        this.transition = null;
    }

    public virtual void OnStateEnter() { }
    public virtual void OnStateExit() { }

    // We hit a state, so return the state
    public override Decision MakeDecision()
    {
        return this;
    }
}

public class PatrolState : State
{
    public PatrolState(Guard guard) : base(guard)
    {
        this.guard = guard;
        this.action = "patrol";

        // Patrol Decision Tree
        AggressiveDecision a = new AggressiveDecision(guard);
        a.trueNode = new TargetState(guard, "alarm");
        a.falseNode = new TargetState(guard, "attack");

        NearbyDecision b = new NearbyDecision(guard);
        b.trueNode = a;
        b.falseNode = new TargetState(guard, "alarm");
        //Debug.Log(((TargetState) b.falseNode).state);

        SpottedDecision c = new SpottedDecision(guard);
        c.trueNode = b;
        c.falseNode = null;

        Decision tree = c;

        this.transition = new Transition(tree);
    }

    public override void OnStateEnter()
    {
        guard.FindPath(guard.patrolFrom, guard.patrolTo);
    }
    public override void OnStateExit() { }
}

public class AlarmState : State {
    int closestAlarm;
    public AlarmState(Guard guard) : base(guard)
    {
        this.guard = guard;
        this.action = "alarm";
        closestAlarm = -1;
        // Alarm Decision Tree
        BackupDecision d = new BackupDecision(guard);
        d.trueNode = new TargetState(guard, "attack");
        d.falseNode = new TargetState(guard, "wait");
        AlarmDecision e = new AlarmDecision(guard);
        e.trueNode = d;
        e.falseNode = null;

        Decision tree = e;

        this.transition = new Transition(tree);
    }

    public override void OnStateEnter()
    {
        float distance = float.MaxValue;
        GameObject[] button = GameObject.FindGameObjectsWithTag("Button");
        for(int i=0; i<button.Length; i++)
        {
            float buttonDistance = Vector3.Distance(guard.gameObject.transform.position, button[i].transform.position);
            if (buttonDistance < distance)
            {
                distance = buttonDistance;
                string name = button[i].name;
                closestAlarm = int.Parse(name.Substring(name.IndexOf("(") + 1, name.IndexOf(")") - name.IndexOf("(") - 1));
            }
        }
    }
    public override void OnStateExit() { }
}

public class AttackState : State {
    public AttackState(Guard guard) : base(guard)
    {
        this.guard = guard;
        this.action = "attack";
        // Attack Tree
        SpottedDecision g = new SpottedDecision(guard);
        g.trueNode = null;
        g.falseNode = new TargetState(guard, "patrol");

        Decision tree = g;

        this.transition = new Transition(tree);
    }

    public override void OnStateEnter() { }
    public override void OnStateExit() { }
}

public class BackupState : State {
    public BackupState(Guard guard) : base(guard)
    {
        this.guard = guard;
        this.action = "wait";

        // Backup Decision Tree
        BackupDecision f = new BackupDecision(guard);
        f.trueNode = new TargetState(guard, "attack");
        f.falseNode = null;

        Decision tree = f;

        this.transition = new Transition(tree);
    }

    public override void OnStateEnter() { }
    public override void OnStateExit() { }
}

public class TargetState : State {
    public string state;

    public TargetState(Guard guard, string state) : base(guard)
    {
        this.guard = guard;
        this.state = state;
    }
}

public class Transition {

    Decision rootDecision;

    public Transition(Decision tree)
    {
        rootDecision = tree;
    }

    public bool IsTriggered()
    {
        //TargetState resultNode = (TargetState) rootDecision.MakeDecision();
        //Debug.Log(resultNode.state);
        /*if(resultNode != null)
        {
            Debug.Log("Found Target State");
        }
        else
        {
            Debug.Log("No Target State");
        }*/

        return rootDecision.MakeDecision() != null;
    }

    public TargetState GetState()
    {
        if (rootDecision.MakeDecision() is TargetState)
        {
            TargetState resultNode = (TargetState) rootDecision.MakeDecision();
            return resultNode;
        }

        return null;
    }
}
