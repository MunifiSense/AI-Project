using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// Based off the pseudocode in the Artificial Intelligence for Games textbook
public class NGramPredictor
{
    Dictionary<List<int>, KeyDataRecord> data;
    int nValue;

    public NGramPredictor(int nValue)
    {
        data = new Dictionary<List<int>, KeyDataRecord>(new ListComparer<int>()); 
        this.nValue = nValue;
    }

    public void RegisterSequence(List<int> actions)
    {
        List<int> key = actions.GetRange(0, nValue);
        int value = actions[nValue];

        if(!data.ContainsKey(key))
        {
            //Debug.Log("Adding Key: " + key[0]);
            data.Add(key, new KeyDataRecord());
        }
        if (!data[key].counts.ContainsKey(value))
        {
            data[key].counts.Add(value, 0);
        }
        data[key].counts[value]++;
        data[key].total++;
    }

    public int GetMostLikely(List<int> actions)
    {
        List<int> key = actions.GetRange(actions.Count-nValue, nValue);
        int highestValue = 0;
        int bestAction = -1;
        // If there is any data about the current actions
        //Debug.Log("Key: " + key[0]);
        if (data.ContainsKey(key)){
            Dictionary<int, int>.KeyCollection actionsList = data[key].counts.Keys;

            foreach (int i in actionsList)
            {
                if (data[key].counts[i] > highestValue)
                {
                    highestValue = data[key].counts[i];
                    bestAction = i;
                }
            }
        }
        //Debug.Log("Best action: " + bestAction);
        return bestAction;
    }
}

public class KeyDataRecord
{
    public Dictionary<int, int> counts;
    public int total;

    public KeyDataRecord()
    {
        counts = new Dictionary<int, int>();
        total = 0;
    }
}

// Code from StackOverflow
// https://stackoverflow.com/questions/10020541/c-sharp-list-as-dictionary-key
public class ListComparer<T> : IEqualityComparer<List<T>> {
    public bool Equals(List<T> x, List<T> y)
    {
        return x.SequenceEqual(y);
    }

    public int GetHashCode(List<T> obj)
    {
        int hashcode = 0;
        foreach (T t in obj)
        {
            hashcode ^= t.GetHashCode();
        }
        return hashcode;
    }
}