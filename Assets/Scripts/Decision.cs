using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Virtual function for decisions
public class Decision : DecisionTreeNode {
    public Guard guard;
    public DecisionTreeNode trueNode;
    public DecisionTreeNode falseNode;

    public Decision(Guard guard)
    {
        this.guard = guard;
        this.trueNode = null;
        this.falseNode = null;
    }

    public new DecisionTreeNode GetBranch()
    {
        return null;
    }

    public new DecisionTreeNode MakeDecision()
    {
        return null;
    }

    public bool Test()
    {
        return false;
    }
}

public class DecisionAction : DecisionTreeNode
{
    public new DecisionAction MakeDecision() => this;
}

// Virtual function for decisions
public class DecisionTreeNode {
    public DecisionTreeNode MakeDecision()
    {
        return null;
    }
}

/*public class FloatDecision : Decision {
    float minValue;
    float maxValue;

    //float testValue;

    public DecisionTreeNode GetBranch(float testValue)
    {
        if((minValue <= testValue) && (testValue <= maxValue))
        {
            return trueNode;
        }
        return falseNode;
    }
}*/

// Is the player in sight?
public class SpottedDecision : Decision
{
    public SpottedDecision(Guard guard) : base(guard)
    {
        this.guard = guard;
        this.trueNode = null;
        this.falseNode = null;
    }

    public new DecisionTreeNode GetBranch()
    {
        if (guard.playerInSight)
        {
            return trueNode;
        }
        return falseNode;
    }
}

// Is the player nearby?
public class NearbyDecision : Decision {
    public NearbyDecision(Guard guard) : base(guard)
    {
        this.guard = guard;
        this.trueNode = null;
        this.falseNode = null;
    }

    public new DecisionTreeNode GetBranch()
    {
        if (guard.playerNearby)
        {
            return trueNode;
        }
        return falseNode;
    }
}

// Is the player aggressive?
public class AggressiveDecision : Decision {
    public AggressiveDecision(Guard guard) : base(guard)
    {
        this.guard = guard;
        this.trueNode = null;
        this.falseNode = null;
    }

    public new DecisionTreeNode GetBranch()
    {
        if (guard.playerAggressive)
        {
            return trueNode;
        }
        return falseNode;
    }
}

// Is there backup nearby?
public class BackupDecision : Decision {
    public BackupDecision(Guard guard) : base(guard)
    {
        this.guard = guard;
        this.trueNode = null;
        this.falseNode = null;
    }

    public new DecisionTreeNode GetBranch()
    {
        if (guard.haveBackup)
        {
            return trueNode;
        }
        return falseNode;
    }
}