using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Code adapted from pseudocode in Artificial Intelligence for Games textbook

public class AStarPathFinding
{
    public static Path FindPath(int start, int end)
    {
        Graph graph = new Graph();
        graph.PopulateGraph();
        int maxLoops = 9999;
        int loop = -1;
        NodeRecord startRecord = new NodeRecord(start, 0, 0);
        PriorityQueue open = new PriorityQueue();
        PriorityQueue closed = new PriorityQueue();
        open.Insert(startRecord);
        NodeRecord current = new NodeRecord();
        while (!open.IsEmpty())
        {
            loop++;
            if(loop > maxLoops)
            {
                Debug.Log("Infinite loop!");
                break;
            }
            current = open.smallestElement();
            //Debug.Log("Current Node: " + current.node);
            // If we reached the goal
            if (current.node == end)
            {
                break;
            }

            List<Connection> currConnections = graph.connections[current.node];
            foreach (Connection connect in currConnections)
            {
                //graph.DrawConnection(connect);
                int endNode = connect.toNode;
                float endNodeCost = current.costSoFar + connect.cost;

                // Connected node is in closed
                if (closed.Contains(endNode))
                {
                    NodeRecord endNodeRecord = closed.Find(endNode);
                    if (endNodeRecord.costSoFar <= endNodeCost)
                    {
                        continue;
                    }
                    closed.Remove(endNode);
                }
                // Connected node is in open
                else if (open.Contains(endNode))
                {
                    NodeRecord endNodeRecord = open.Find(endNode);

                    if (endNodeRecord.costSoFar <= endNodeCost)
                    {
                        continue;
                    }

                }
                // Unvisited node
                else
                {
                    NodeRecord endNodeRecord = new NodeRecord();
                    endNodeRecord.node = endNode;
                    endNodeRecord.conn = connect;
                    endNodeRecord.costSoFar = endNodeCost;
                    endNodeRecord.estimatedTotalCost = endNodeCost + Heuristic(endNode, end);

                    open.Insert(endNodeRecord);
                }
            }
            open.Remove(current.node);
            closed.Insert(current);
        }
        Path path = new Path();
        if(current.node == end)
        {
            while (current.node != start)
            {
                path.list.Insert(0, current.conn);
                current = closed.Find(current.conn.fromNode);
            }
        }
        return path;
    }

    // Heuristic function (just distance from a to b)
    public static float Heuristic(int node, int end)
    {
        GameObject pathNodes = GameObject.Find("PathNodes");
        return Vector3.Distance(pathNodes.transform.GetChild(node).position, pathNodes.transform.GetChild(end).position);
    }

}