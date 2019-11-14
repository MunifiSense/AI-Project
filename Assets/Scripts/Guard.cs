using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AStarPathFinding a = new AStarPathFinding();
        Path path = a.FindPath(0, 26);
        // Drawing path for debugging
        path.DrawPath();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
