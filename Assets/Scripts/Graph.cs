using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Graph
{
    public List<List<Connection>> connections = new List<List<Connection>>();

    public void PopulateGraph()
    {
        int layerMask = 1 << 8;
        layerMask = ~layerMask;
        GameObject pathNodes = GameObject.Find("PathNodes");
        foreach (Transform child in pathNodes.transform)
        {
        //Transform child = pathNodes.transform.GetChild(0);
            int num = getIndex(child.gameObject.name);
            List<Connection> localConnections = new List<Connection>();
            foreach (Transform node in pathNodes.transform)
            {
                float distance  = Vector3.Distance(child.position, node.position);
                //node.GetComponent<MeshRenderer>().material.color = Color.red;
                //Debug.Log("Direction and Distance: " + (node.position - child.position + " " + distance + " to " + node.name));
                // If no collision between the child and node
                LayerMask mask = LayerMask.GetMask("Player");
                mask |= (1 << LayerMask.NameToLayer("Enemy"));
                mask = ~mask;
                if (!Physics.Raycast(child.position, node.position - child.position, distance, mask, QueryTriggerInteraction.Ignore))
                {
                    //Debug.DrawLine(child.position, node.position, Color.yellow, 999);
                    localConnections.Add(new Connection(distance, num, getIndex(node.gameObject.name)));
                    //Debug.Log("Did not hit a wall");
                }
                else
                {
                    //Debug.DrawLine(child.position, node.position, Color.red, 999);
                    //Debug.Log("Hit a wall");
                }
            }
            connections.Add(localConnections);
        }
    }

    private int getIndex(string name)
    {
        return int.Parse(name.Substring(name.IndexOf("(")+1, name.IndexOf(")")- name.IndexOf("(")-1));
    }

    public void DrawConnections(int node)
    {
        GameObject pathNodes = GameObject.Find("PathNodes");
        foreach (Connection connect in connections[node])
        {
            Debug.DrawLine(pathNodes.transform.GetChild(connect.fromNode).position, 
                pathNodes.transform.GetChild(connect.toNode).position, 
                Color.yellow, 999);
        }
    }

    public void DrawConnection(Connection connect)
    {
        GameObject pathNodes = GameObject.Find("PathNodes");
        Debug.DrawLine(pathNodes.transform.GetChild(connect.fromNode).position,
            pathNodes.transform.GetChild(connect.toNode).position,
            Color.yellow, 999);
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
