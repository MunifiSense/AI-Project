using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetState : Decision
{
    public State targetState;
    //List<string> actions;

    public TargetState(Guard guard, State targetState/*, List<string> actions*/) : base(guard)
    {
        this.guard = guard;
        this.targetState = targetState;
        //this.actions = actions;
    }

    public State GetTargetState()
    {
        return targetState;
    }

    /*public List<string> GetAction()
    {
        return actions;
    }*/
}

public class DecisionTreeTransition : Transition
{
    State targetState;

    Decision decisionTreeRoot;

    public new bool IsTriggered()
    {
        DecisionTreeNode node = MakeDecision(decisionTreeRoot);
        targetState = ((TargetState) node).targetState;
        return targetState != null;
    }
    public new State GetTargetState()
    {
        if(targetState != null)
        {
            return targetState;
        }

        return null;
    }

    public new List<string> GetAction()
    {
        if (targetState != null)
        {
            return targetState.GetAction();
        }

        return null;
    }

    public DecisionTreeNode MakeDecision(Decision node)
    {
        if (node == null || node is TargetState)
        {
            return node;
        }

        if (node.Test())
        {
            return MakeDecision((Decision) node.trueNode);
        }

        return MakeDecision((Decision) node.falseNode);
    }
}



