using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetState : Decision
{
    State targetState;
    List<string> actions;

    public State GetTargetState()
    {
        return targetState;
    }

    public List<string> GetAction()
    {
        return actions;
    }

    public Decision MakeDecision(Decision node)
    {
        if(node == null || node is TargetState)
        {
            return node;
        }

        if (node.testValue)
        {
            return node.trueNode;
        }

        return node.falseNode;
    }
}

public class DecisionTreeTransition
{
    TargetState targetState;

    Decision decisionTreeRoot;

    public bool IsTriggered()
    {
        targetState = (TargetState) targetState.MakeDecision(decisionTreeRoot);
        return targetState != null;
    }
    public State GetTargetState()
    {
        if(targetState != null)
        {
            return targetState.GetTargetState();
        }

        return null;
    }

    public List<string> GetAction()
    {
        if (targetState != null)
        {
            return targetState.GetAction();
        }

        return null;
    }
}


