using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarPathFinding
{
    Graph graph;

    public AStarPathFinding()
    {
        graph = new Graph();
        graph.PopulateGraph();
    }

    public List<Connection> FindPath(int start, int end)
    {
        NodeRecord startRecord = new NodeRecord(0, 0, 0);
        PriorityQueue open = new PriorityQueue();
        PriorityQueue closed = new PriorityQueue();
        open.Insert(startRecord);
        NodeRecord current = new NodeRecord();
        while (open.length() > 0)
        {
            current = open.Top();
            if (current.node == end)
            {
                break;
            }
            List<Connection> currConnections = graph.connections[current.node];
            foreach (Connection connect in currConnections)
            {
                int endNode = connect.toNode;
                float endNodeCost = current.costSoFar + connect.cost;
                if (closed.Contains(endNode))
                {
                    NodeRecord endNodeRecord = closed.Find(endNode);
                    if (endNodeRecord.costSoFar <= endNodeCost)
                    {
                        continue;
                    }
                    closed.Remove(endNode);
                }
                else if (open.Contains(endNode))
                {
                    NodeRecord endNodeRecord = open.Find(endNode);

                    if (endNodeRecord.costSoFar <= endNodeCost)
                    {
                        continue;
                    }

                }
                else
                {
                    NodeRecord endNodeRecord = new NodeRecord();
                    endNodeRecord.node = endNode;
                    endNodeRecord.conn = connect;
                    endNodeRecord.estimatedTotalCost = endNodeCost + Heuristic(current.node, endNode);
                }

                open.Remove(current.node);
                closed.Insert(current);
            }
        }
        List<Connection> list = new List<Connection>();
        if (current.node != end)
        {
            return null;
        }
        else
        {
            while (current.node != start)
            {
                list.Insert(0, current.conn);
                current = closed.Find(current.conn.fromNode);
            }
        }
        return list;
    }

    public float Heuristic(int node, int end)
    {
        GameObject pathNodes = GameObject.Find("PathNodes");
        return Vector3.Distance(pathNodes.transform.GetChild(node).position, pathNodes.transform.GetChild(end).position);
    }
}