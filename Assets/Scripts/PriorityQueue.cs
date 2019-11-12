using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriorityQueue
{
    private List<NodeRecord> list = new List<NodeRecord>();

    public bool IsEmpty()
    {
        return list.Count == 0;
    }

    // Inserts item into priority queue
    public void Insert(NodeRecord record)
    {
        list.Add(record);
    }

    // Returns element with highest priority
    public NodeRecord Top()
    {
        NodeRecord smallest = new NodeRecord();
        float currValue = float.MaxValue;
        for(int i = 0; i<list.Count; i++)
        {
            if(list[0].estimatedTotalCost < currValue)
            {
                currValue = list[0].estimatedTotalCost;
                smallest = list[0];
            }
        }
        return smallest;
    }

    // Deletes the element with highest priority
    public void Pop()
    {
        NodeRecord smallest = new NodeRecord();
        float currValue = float.MaxValue;
        for (int i = 0; i < list.Count; i++)
        {
            if (list[0].estimatedTotalCost < currValue)
            {
                currValue = list[0].estimatedTotalCost;
                smallest = list[0];
            }
        }
        list.Remove(smallest);
    }

    // Deletes an element from the list
    public void Remove(int node)
    {
        list.Remove(list.Find(x => x.node == node));
    }

    public bool Contains(int node)
    {
        foreach(NodeRecord item in list)
        {
            if (item.node == node)
            {
                return true;
            }
        }
        return false;
    }

    public NodeRecord Find(int node)
    {
        return list.Find(x => x.node == node);
    }

    public int length()
    {
        return list.Count;
    }
}

public struct NodeRecord: IEquatable<NodeRecord>
{
    public int node;
    public Connection conn;
    public float costSoFar;
    public float estimatedTotalCost;

    public NodeRecord(int x, int y, int z)
    {
        node = x;
        conn = null;
        costSoFar = y;
        estimatedTotalCost = z;
    }
    public bool Equals(NodeRecord other)
    {
        return node == other.node;
    }
}