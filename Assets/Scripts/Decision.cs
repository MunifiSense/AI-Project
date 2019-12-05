using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Code based off Artificial Intelliegence for Games textbook
// Decision Trees

// Virtual function for decisions
public class Decision : DecisionTreeNode {
    public Guard guard;
    public Decision trueNode;
    public Decision falseNode;

    public Decision(Guard guard)
    {
        this.guard = guard;
        this.trueNode = null;
        this.falseNode = null;
    }

    public virtual Decision GetBranch()
    {
        return null;
    }

    public new virtual Decision MakeDecision()
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
    public virtual DecisionTreeNode MakeDecision()
    {
        return null;
    }
}

// Is the player in sight?
public class SpottedDecision : Decision
{
    public SpottedDecision(Guard guard) : base(guard)
    {
        this.guard = guard;
        this.trueNode = null;
        this.falseNode = null;
    }

    public override Decision GetBranch()
    {
        //Debug.Log("Is the player in sight?");
        if (guard.playerInSight)
        {
            //Debug.Log("Player in sight");
            return trueNode;
        }
        return falseNode;
    }

    public override Decision MakeDecision()
    {
        Decision branch = GetBranch();
        if(branch is null)
        {
            return branch;
        }
        return branch.MakeDecision();
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

    public override Decision GetBranch()
    {
        //Debug.Log("Is the player nearby?");
        if (guard.playerNearby)
        {
            return trueNode;
        }
        //Debug.Log("Player not nearby " + ((TargetState) falseNode).state);
        return falseNode;
    }

    public override Decision MakeDecision()
    {
        Decision branch = GetBranch();
        if (branch is null)
        {
            return branch;
        }
        return branch.MakeDecision();
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

    public override Decision GetBranch()
    {
        //Debug.Log("Is the player aggressive?");
        if (guard.playerAggressive)
        {
            return trueNode;
        }
        return falseNode;
    }

    public override Decision MakeDecision()
    {
        Decision branch = GetBranch();
        if (branch is null)
        {
            return branch;
        }
        return branch.MakeDecision();
    }
}


// Is there backup nearby?
public class ArrivedDecision : Decision {
    public ArrivedDecision(Guard guard) : base(guard)
    {
        this.guard = guard;
        this.trueNode = null;
        this.falseNode = null;
    }

    public override Decision GetBranch()
    {
        //Debug.Log("Reached backup location?");
        if (guard.atPlayerLastLocation)
        {
            return trueNode;
        }
        return falseNode;
    }

    public override Decision MakeDecision()
    {
        Decision branch = GetBranch();
        if (branch is null)
        {
            return branch;
        }
        return branch.MakeDecision();
    }
}

// Is the alarm triggered?
public class AlarmDecision : Decision
{
    public AlarmDecision(Guard guard) : base(guard)
    {
        this.guard = guard;
        this.trueNode = null;
        this.falseNode = null;
    }

    public override Decision GetBranch()
    {
        //Debug.Log("Is the alarm triggered?");
        if (GameObject.Find("GameState").GetComponent<GameState>().Alarm())
        {
            return trueNode;
        }
        return falseNode;
    }

    public override Decision MakeDecision()
    {
        Decision branch = GetBranch();
        if (branch is null)
        {
            return branch;
        }
        return branch.MakeDecision();
    }
}

// Are we at the room?
public class RoomDecision : Decision {
    public RoomDecision(Guard guard) : base(guard)
    {
        this.guard = guard;
        this.trueNode = null;
        this.falseNode = null;
    }

    public override Decision GetBranch()
    {
        //Debug.Log("Is the alarm triggered?");
        if (guard.arrivedAtRoom)
        {
            //Debug.Log("AT ROOM");
            return trueNode;
        }
        return falseNode;
    }

    public override Decision MakeDecision()
    {
        Decision branch = GetBranch();
        if (branch is null)
        {
            return branch;
        }
        return branch.MakeDecision();
    }
}

// Are we at the room?
public class LaserDecision : Decision {
    public LaserDecision(Guard guard) : base(guard)
    {
        this.guard = guard;
        this.trueNode = null;
        this.falseNode = null;
    }

    public override Decision GetBranch()
    {
        //Debug.Log("Is the alarm triggered?");
        if (guard.nearbyRoomTripped > -1)
        {
            return trueNode;
        }
        return falseNode;
    }

    public override Decision MakeDecision()
    {
        Decision branch = GetBranch();
        if (branch is null)
        {
            return branch;
        }
        return branch.MakeDecision();
    }
}