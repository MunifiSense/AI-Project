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
            string transitionState = currentState.transition.GetState().state;
            currentState.OnStateExit();
            //Debug.Log(guard.name + " transitioning to: " + transitionState);
            switch(transitionState)
            {
                case "attack":
                    currentState = new AttackState(guard);
                    //Debug.Log("Attack state");
                    break;
                case "alarm":
                    currentState = new AlarmState(guard);
                    //Debug.Log("Alarm state");
                    break;
                case "patrol":
                    currentState = new PatrolState(guard);
                    //Debug.Log("Patrol state");
                    break;
                case "investigate":
                    currentState = new InvestigateState(guard);
                    //Debug.Log("Investigate state");
                    break;
            }
            currentState.OnStateEnter();
        }

        switch (currentState.action)
        {
            case "patrol":
                if (guard.atPatrol)
                {
                    if (guard.FollowPath(guard.path, guard.pathOffset))
                    {
                        int temp = guard.patrolFrom;
                        guard.patrolFrom = guard.patrolTo;
                        guard.patrolTo = temp;
                        guard.FindPath(guard.patrolFrom, guard.patrolTo);
                    }
                }
                else
                {
                   if(guard.FollowPath(guard.path, guard.pathOffset))
                    {
                        guard.atPatrol = true;
                        guard.FindPath(guard.patrolFrom, guard.patrolTo);
                    }
                }
                break;
            case "alarm":
                guard.FollowPath(guard.path, guard.pathOffset);
                break;
            case "attack":
                if (!guard.playerInSight)
                {
                    if(guard.playerLastLastSighting != guard.playerLastSighting)
                    {
                        guard.playerLastLastSighting = guard.playerLastSighting;
                        guard.FindPathTo(42);
                    }
                    if (guard.FollowPath(guard.path, guard.pathOffset))
                    {
                        guard.atPlayerLastLocation = true;
                    }
                    
                }
                else
                {
                    guard.ChasePlayer();
                }
                break;
            case "investigate":
                //guard.path.DrawPath();
                if (guard.FollowPath(guard.path, guard.pathOffset))
                {
                    List<int> playerLoc = new List<int>() { guard.nearbyRoomTripped };
                    int nextRoom = guard.GetComponent<GuardLearning>().nGramPredictor.GetMostLikely(playerLoc);
                    if (guard.checkedRooms == 0 && nextRoom != -1)
                    {
                        Debug.Log("There is data on player room exploring. Checking if player is in Room " + nextRoom);
                        guard.FindPathToRoom(nextRoom);
                        guard.checkedRooms++;
                    }
                    else
                    {
                        guard.arrivedAtRoom = true;
                    }
                }
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

        AlarmDecision d = new AlarmDecision(guard);
        d.trueNode = new TargetState(guard, "attack");
        d.falseNode = c;

        LaserDecision e = new LaserDecision(guard);
        e.trueNode = new TargetState(guard, "investigate");
        e.falseNode = d;

        Decision tree = e;

        this.transition = new Transition(tree);
    }

    public override void OnStateEnter()
    {
        if(Vector3.Distance(guard.transform.position, GameObject.Find("Node ("+guard.patrolFrom + ")").transform.position) <
            Vector3.Distance(guard.transform.position, GameObject.Find("Node (" + guard.patrolTo + ")").transform.position))
        {
            guard.FindPathTo(guard.patrolFrom);
        }
        else
        {
            guard.FindPathTo(guard.patrolTo);
            int x = guard.patrolFrom;
            guard.patrolFrom = guard.patrolTo;
            guard.patrolTo = x;
        }
        //guard.path.DrawPath();
    }
    public override void OnStateExit()
    {
        guard.atPatrol = false;
    }
}

public class AlarmState : State {
    public AlarmState(Guard guard) : base(guard)
    {
        this.guard = guard;
        this.action = "alarm";
        // Alarm Decision Tree
        /*BackupDecision d = new BackupDecision(guard);
        d.trueNode = new TargetState(guard, "attack");
        d.falseNode = new TargetState(guard, "wait");*/
        AlarmDecision e = new AlarmDecision(guard);
        e.trueNode = new TargetState(guard, "attack");
        e.falseNode = null;

        Decision tree = e;

        this.transition = new Transition(tree);
    }

    public override void OnStateEnter()
    {
        foreach(Transform b in GameObject.Find("Buttons").transform)
        {
            b.gameObject.GetComponent<BoxCollider>().enabled = true;
        }
        float distance = float.MaxValue;
        GameObject[] button = GameObject.FindGameObjectsWithTag("Button");
        int closestAlarm = -1;
        for (int i=0; i<button.Length; i++)
        {
            float buttonDistance = Vector3.Distance(guard.gameObject.transform.position, button[i].transform.position);
            if (buttonDistance < distance)
            {
                distance = buttonDistance;
                string name = button[i].name;
                closestAlarm = int.Parse(name.Substring(name.IndexOf("(") + 1, name.IndexOf(")") - name.IndexOf("(") - 1));

                //Debug.Log("Closest alarm: " + closestAlarm);
            }
        }
        guard.FindPathTo(closestAlarm);
    }
    public override void OnStateExit()
    {
        foreach (Transform b in GameObject.Find("Buttons").transform)
        {
            b.gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }
}

public class AttackState : State {
    public AttackState(Guard guard) : base(guard)
    {
        this.guard = guard;
        this.action = "attack";
        // Attack Tree

        SpottedDecision a = new SpottedDecision(guard);
        a.trueNode = null;
        a.falseNode = new TargetState(guard, "patrol");
        ArrivedDecision b = new ArrivedDecision(guard);
        b.trueNode = a;
        b.falseNode = null;

        Decision tree = b;

        this.transition = new Transition(tree);
    }

    public override void OnStateEnter()
    {
        guard.FindPathTo(42);
    }
    public override void OnStateExit()
    {
        guard.playerLastLastSighting = guard.playerLastSighting;
        guard.atPlayerLastLocation = false;
        GameObject.Find("GameState").GetComponent<GameState>().StopAlarm();
        List<bool> stuffToClassify = new List<bool>() { guard.playerSighted, GameObject.Find("GameState").GetComponent<GameState>().alarm };
        GameObject[] guards = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject g in guards)
        {
            g.GetComponent<GuardLearning>().bayesClassifier.Update(stuffToClassify, guard.shotAt);
        }
        guard.shotAt = false;
        guard.playerSighted = false;
    }
}

public class InvestigateState : State {
    public InvestigateState(Guard guard) : base(guard)
    {
        this.guard = guard;
        this.action = "investigate";

        // Investigate Decision Tree
        AggressiveDecision a = new AggressiveDecision(guard);
        a.trueNode = new TargetState(guard, "alarm");
        a.falseNode = new TargetState(guard, "attack");

        NearbyDecision b = new NearbyDecision(guard);
        b.trueNode = a;
        b.falseNode = new TargetState(guard, "alarm");
        //Debug.Log(((TargetState) b.falseNode).state);

        SpottedDecision c = new SpottedDecision(guard);
        c.trueNode = b;
        c.falseNode = new TargetState(guard, "patrol");

        //If player spotted during travel to room
        SpottedDecision d = new SpottedDecision(guard);
        d.trueNode = b;
        d.falseNode = null;

        RoomDecision e = new RoomDecision(guard);
        e.trueNode = c;
        e.falseNode = d;

        Decision tree = e;

        this.transition = new Transition(tree);
    }

    public override void OnStateEnter()
    {
        guard.FindPathToRoom(guard.nearbyRoomTripped);
    }
    public override void OnStateExit()
    {
        guard.arrivedAtRoom = false;
        guard.nearbyRoomTripped = -1;
    }
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
