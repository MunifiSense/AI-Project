using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Condition
{
    public bool Test()
    {
        return false;
    }
}public class FloatCondition: Condition {
    float minValue;
    float maxValue;

    float testValue;

    public FloatCondition(float minValue, float maxValue, float testValue)
    {
        this.minValue = minValue;
        this.maxValue = maxValue;
        this.testValue = testValue;
    }

    public new bool Test()
    {
        return (minValue <= testValue) && (testValue <= maxValue);
    }
}

public class AndCondition: Condition
{
    Condition condA;
    Condition condB;

    public AndCondition(Condition a, Condition b)
    {
        condA = a;
        condB = b;
    }

    public new bool Test()
    {
        return condA.Test() && condB.Test();
    }
}

public class NotCondition: Condition
{
    Condition cond;

    public NotCondition(Condition a)
    {
        cond = a;
    }

    public new bool Test()
    {
        return !cond.Test();
    }
}

public class OrCondition : Condition
{
    Condition condA;
    Condition condB;

    public OrCondition(Condition a, Condition b)
    {
        condA = a;
        condB = b;
    }

    public new bool Test()
    {
        return condA.Test() || condB.Test();
    }
}