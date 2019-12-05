using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardLearning : MonoBehaviour
{
    Guard guard;
    public NGramPredictor nGramPredictor;
    public NaiveBayesClassifier bayesClassifier;
    public int nValue = 1;

    public int roomsEntered;
    List<int> lastRoomsVisited;

    // Start is called before the first frame update
    void Start()
    {
        guard = GetComponent<Guard>();
        nGramPredictor = new NGramPredictor(nValue);
        bayesClassifier = new NaiveBayesClassifier();
        roomsEntered = 0;
        lastRoomsVisited = new List<int>();
    }

    // Update is called once per frame
    void Update()
    {
        if(roomsEntered == nValue+1)
        {
            roomsEntered = 0;
            //Debug.Log(lastRoomsVisited);
            nGramPredictor.RegisterSequence(lastRoomsVisited);
            lastRoomsVisited = new List<int>();
        }
        List<bool> stuffToClassify = new List<bool>() { guard.playerSighted, GameObject.Find("GameState").GetComponent<GameState>().alarm };
        guard.playerAggressive = bayesClassifier.Predict(stuffToClassify);
        Debug.Log("Aggressive: " + guard.playerAggressive);
    }

    public void AddRoomVisit(int room)
    {
        lastRoomsVisited.Add(room);
        if(lastRoomsVisited.Count == nValue)
        {
            nGramPredictor.GetMostLikely(lastRoomsVisited);
        }
        roomsEntered++;
    }
}
