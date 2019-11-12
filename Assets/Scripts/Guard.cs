using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AStarPathFinding a = new AStarPathFinding();
        a.FindPath(0, 26);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
