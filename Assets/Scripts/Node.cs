using System;
using System.Collections.Generic;
using UnityEngine;

public class Node : IComparable
{
    public List<Node> neighbours;
    public List<Node> path;

    public int costFromOrigin;
    public int costToTarget;

    public Vector2 position;

    public Node(Vector2 startPoint, int costFromOrigin, Node target)
    {
        position = startPoint;

        path = new List<Node>();

        neighbours = new List<Node>();
        this.costFromOrigin = costFromOrigin;
        costToTarget = calcBestCostToTarget(target);
    }

    public Node(Vector2 startPoint)
    {
        position = startPoint;
        path = new List<Node>();
        neighbours = new List<Node>();
        costFromOrigin = 0;
        costToTarget = 0;
    }

    int calcBestCostToTarget(Node target)
    {
        Vector2 cost = this.position - target.position;
        return (int) Mathf.Abs(cost.x) + (int) Mathf.Abs(cost.y);
    }

    public int CompareTo(object obj)
    {
        if (costFromOrigin + costToTarget <= ((Node) obj).costFromOrigin + costToTarget)
            return -1;
        return 1;
    }

    public override bool Equals(object obj)
    {
        return position.Equals(((Node) obj).position);
    }
}