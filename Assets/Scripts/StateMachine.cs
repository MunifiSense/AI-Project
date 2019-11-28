using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    List<State> states;
    State initialState;
    State currentState;
    
    public StateMachine(List<State> states, State initialState, State currentState)
    {
        this.states = states;
        this.initialState = initialState;
        this.currentState = initialState;
    }

    public List<string> Update()
    {

        Transition triggeredTransition = null;

        foreach(Transition transition in currentState.GetTransitions())
        {
            if (transition.IsTriggered())
            {
                triggeredTransition = transition;
            }

            if(triggeredTransition != null)
            {
                State targetState = triggeredTransition.GetTargetState();

                List<string> actions = currentState.GetEntryAction();
                foreach(string action in triggeredTransition.GetAction())
                {
                    actions.Add(action);
                }
                foreach (string action in targetState.GetEntryAction())
                {
                    actions.Add(action);
                }

                currentState = targetState;
                return actions;
            }
        }
        //Otherwise return current state's actions
        return currentState.GetAction();
    }
}

public class State
{
    List<string> actions;
    List<string> entryActions;
    List<string> exitActions;
    List<Transition> transitions;

    public State(List<string> actions, List<string> entryActions, List<string> exitActions, List<Transition> transitions)
    {
        this.actions = actions;
        this.entryActions = entryActions;
        this.exitActions = exitActions;
        this.transitions = transitions;
    }

    public List<string> GetAction()
    {
        return actions;
    }

    public List<string> GetEntryAction()
    {
        return entryActions;
    }

    public List<string> GetExitAction()
    {
        return exitActions;
    }

    public List<Transition> GetTransitions()
    {
        return transitions;
    }
}

public class Transition
{
    Condition cond;
    State targetState;
    List<string> actions;

    /*public Transition(Condition cond, State targetState, List<string> actions)
    {
        this.cond = cond;
        this.targetState = targetState;
        this.actions = actions;
    }*/

    public bool IsTriggered()
    {
        return cond.Test();
    }

    public State GetTargetState()
    {
        return targetState;
    }

    public List<string> GetAction()
    {
        return actions;
    }
}
