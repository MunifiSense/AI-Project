using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaiveBayesClassifier
{
    int examplesCountPositive = 0;
    int examplesCountNegative = 0;

    List<int> attributeCountsPositive;
    List<int> attributeCountsNegative;

    public void Update(List<bool> attributes, bool label)
    {
        if (label)
        {
            for(int i=0; i<attributeCountsPositive.Count; i++)
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
        float x = NaiveProbabilities(attributes, attributeCountsPositive, examplesCountPositive, examplesCountNegative);
        float y = NaiveProbabilities(attributes, attributeCountsNegative, examplesCountPositive, examplesCountNegative);

        return x >= y;
    }

    public float NaiveProbabilities(List<bool> attributes, List<int> counts, float m, float n)
    {
        float prior = m / (m + n);
        float p = 1.0f;

        for(int i = 0; i < attributeCountsPositive.Count; i++)
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
