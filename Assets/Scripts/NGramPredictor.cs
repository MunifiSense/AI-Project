using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NGramPredictor
{
    Dictionary<List<int>, KeyDataRecord> data;
    int nValue;

    public void RegisterSequence(List<int> actions)
    {
        List<int> key = actions;
        int value = actions[nValue];

        if(data.ContainsKey(key))
        {
            data[key] = new KeyDataRecord();
        }

        data[key].counts[value]++;
        data[key].total++;
    }

    public int GetMostLikely(List<int> actions)
    {
        int highestValue = 0;
        int bestAction = -1;

        Dictionary<int, int>.KeyCollection actionsList = data[actions].counts.Keys;

        foreach(int i in actionsList)
        {
            if(data[actions].counts[i] > highestValue)
            {
                highestValue = data[actions].counts[i];
                bestAction = i;
            }
        }

        return bestAction;
    }
}

public class KeyDataRecord
{
    public Dictionary<int, int> counts;
    public int total;
}