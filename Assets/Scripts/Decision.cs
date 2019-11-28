using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Virtual function for decisions
public class Decision : DecisionTreeNode {
    public Decision trueNode;
    public Decision falseNode;
    public bool testValue;

    public new Decision GetBranch()
    {
        return null;
    }

    public new Decision MakeDecision()
    {
        Decision branch = GetBranch();
        return branch.MakeDecision();
    }
}

public class DecisionAction : DecisionTreeNode
{
    public new DecisionAction MakeDecision() => this;
}

// Virtual function for decisions
public class DecisionTreeNode {

    public Decision GetBranch()
    {
        return null;
    }

    public void MakeDecision()
    {

    }
}

public class FloatDecision : Decision {
    float minValue;
    float maxValue;

    //float testValue;

    public FloatDecision(float minValue, float maxValue)
    {
        this.minValue = minValue;
        this.maxValue = maxValue;
        //this.testValue = testValue;
    }

    public Decision GetBranch(float testValue)
    {
        if((minValue <= testValue) && (testValue <= maxValue))
        {
            return trueNode;
        }
        return falseNode;
    }
}