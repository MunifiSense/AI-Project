using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Code adapted from pseudocode in the Artificial Intelligence for Games textbook
public class Path
{
    public List<Connection> list;
    private List<float> nodeParam;

    public Path()
    {
        list = new List<Connection>();
        nodeParam = new List<float>();
    }

    public void CalcParams()
    {
        float param = 0;
        GameObject pathNodes = GameObject.Find("PathNodes");
        foreach (Connection connection in list)
        {
            nodeParam.Add(param);
            param += Vector3.Distance(pathNodes.transform.GetChild(connection.fromNode).position,
            pathNodes.transform.GetChild(connection.toNode).position);
        }
    }

    // Draws the path (For debugging)
    public void DrawPath()
    {
        GameObject pathNodes = GameObject.Find("PathNodes");
        foreach (Connection connection in list)
        {
            Debug.DrawLine(pathNodes.transform.GetChild(connection.fromNode).position,
            pathNodes.transform.GetChild(connection.toNode).position,
            Color.green, 999);
        }
    }

    // From Unity forums https://forum.unity.com/threads/how-do-i-find-the-closest-point-on-a-line.340058/
    public Vector3 NearestPointOnFiniteLine(Vector3 start, Vector3 end, Vector3 pnt)
    {
        var line = (end - start);
        var len = line.magnitude;
        line.Normalize();

        var v = pnt - start;
        var d = Vector3.Dot(v, line);
        d = Mathf.Clamp(d, 0f, len);
        return start + line * d;
    }

    public float getParam(Vector3 position)
    {
        float distance = float.MaxValue;
        int closestNode = 0;
        int closestNodeTo = 0;
        GameObject pathNodes = GameObject.Find("PathNodes");
        for(int i = 0; i < list.Count; i++)
        {
            float distanceFrom = Vector3.Distance(position, pathNodes.transform.GetChild(list[i].fromNode).position);
            // If last connection
            if (i == list.Count - 1)
            {
                distanceFrom = Vector3.Distance(position, pathNodes.transform.GetChild(list[i].toNode).position);
                if (distanceFrom <= distance)
                {
                    distance = distanceFrom;
                    closestNode = list[i].toNode;
                }
                return nodeParam[closestNode];
            }
            else if (Vector3.Distance(position, pathNodes.transform.GetChild(list[i].fromNode).position) <= distance)
            {
                distance = distanceFrom;
                closestNode = list[i].fromNode;
                closestNodeTo = list[i].toNode;
            }
        }
        Vector3 nearestPoint = NearestPointOnFiniteLine(pathNodes.transform.GetChild(closestNode).position,
            pathNodes.transform.GetChild(closestNodeTo).position, position);
        return nodeParam[closestNode] + Vector3.Distance(pathNodes.transform.GetChild(closestNode).position, nearestPoint);
    }

    public Vector3 getPosition(float param)
    {
        GameObject pathNodes = GameObject.Find("PathNodes");
        int closestNode = 0;
        int closestNodeTo = 0;
        float distance = float.MaxValue;
        for (int i = 0; i < list.Count; i++)
        {
            if(param - nodeParam[i] < distance)
            {
                closestNode = list[i].fromNode;
                closestNodeTo = list[i].toNode;
                distance = param - nodeParam[i];
            }
        }
        return Vector3.Lerp(pathNodes.transform.GetChild(closestNode).position, 
            pathNodes.transform.GetChild(closestNodeTo).position, distance);
    }
}
