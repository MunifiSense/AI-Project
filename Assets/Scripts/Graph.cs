using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph
{
    public List<List<Connection>> connections = new List<List<Connection>>();

    public void PopulateGraph()
    {
        GameObject pathNodes = GameObject.Find("PathNodes");
        foreach (Transform child in pathNodes.transform)
        {
            int num = getIndex(child.gameObject.name);
            List<Connection> localConnections = new List<Connection>();
            foreach (Transform node in pathNodes.transform)
            {
                float direction = Vector3.Angle(child.position, node.position);
                float distance  = Vector3.Distance(child.position, node.position);

                // If no collision between the object and node
                if (!Physics.Raycast(child.position, new Vector3(0, direction, 0), distance))
                {
                    localConnections.Add(new Connection(distance, num, getIndex(node.name)));
                }
            }
            connections.Add(localConnections);
        }
    }

    private int getIndex(string name)
    {
        return int.Parse(name.Substring(name.IndexOf("("), name.IndexOf(")")));
    }
}

public class Connection
{
    public float cost;
    public int fromNode;
    public int toNode;

    public Connection(float x, int y, int z)
    {
        cost = x;
        fromNode = y;
        toNode = z;
    }
}
