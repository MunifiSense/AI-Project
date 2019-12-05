using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Based off pseudocode from Artificial Intelligence for Games textbook
public class NaiveBayesClassifier
{
    int examplesCountPositive = 0;
    int examplesCountNegative = 0;

    List<int> attributeCountsPositive = new List<int>();
    List<int> attributeCountsNegative = new List<int>();

    public void Update(List<bool> attributes, bool label)
    {
        if(attributeCountsNegative.Count != attributes.Count)
        {
            foreach(bool a in attributes)
            {
                attributeCountsPositive.Add(0);
                attributeCountsNegative.Add(0);
            }
        }
        if (label)
        {
            for(int i=0; i<attributes.Count; i++)
            {
                if (attributes[i])
                {
                    attributeCountsPositive[i]++;
                    examplesCountPositive++;
                }
                else
                {
                    attributeCountsNegative[i]++;
                    examplesCountNegative++;
                }
            }
        }
    }

    public bool Predict(List<bool> attributes)
    {
        if(attributeCountsNegative.Count + attributeCountsPositive.Count > 0)
        {
            float x = NaiveProbabilities(attributes, attributeCountsPositive, examplesCountPositive, examplesCountNegative);
            float y = NaiveProbabilities(attributes, attributeCountsNegative, examplesCountPositive, examplesCountNegative);

            return x >= y;
        }
        Debug.Log("Can't predict. No data. Default false.");
        return false;
    }

    public float NaiveProbabilities(List<bool> attributes, List<int> counts, float m, float n)
    {
        float prior = m / (m + n);
        float p = 1.0f;

        for(int i = 0; i < attributes.Count; i++)
        {
            p /= m;
            if (attributes[i])
            {
                p *= counts[i];
            }
            else
            {
                p *= m - counts[i];
            }
        }

        return prior * p;
    }
}
