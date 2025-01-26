using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

class LineSegment
{
    public float length;
    public Vector2 endPoint;
    public bool decay;

    public LineSegment(Vector2 endPoint)
    {
        this.length = 0;
        this.endPoint = endPoint;
        this.decay = false;
    }

    public LineSegment(float length, Vector2 endPoint)
    {
        this.length = length;
        this.endPoint = endPoint;
        this.decay = false;
    }
}

public class CaptureLine : MonoBehaviour
{
    [SerializeField] private LineRenderer line;
    [SerializeField] private EdgeCollider2D edgeCol;
    [SerializeField] private PolygonCollider2D loopCol;

    [SerializeField] private float maxDist;
    [SerializeField] private float stepDist;

    private LinkedList<LineSegment> pointQueue;

    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
        pointQueue = new LinkedList<LineSegment>();
        edgeCol.transform.position -= transform.position;
        loopCol.points = new Vector2[0];
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        LinkedListNode<LineSegment> currentTail = pointQueue.First;

        if (currentTail != null && currentTail.Value.decay)
        {
            pointQueue.RemoveFirst();
        }

        if (pointQueue.First != null)
        {
            pointQueue.First.Value.length = 0;
        }
    }

    public void SetPosition(Vector2 pos)
    {
        if (!CanAppend(pos))
        {
            return;
        }

        //Add new point to line, trim line if maximum distance
        if (pointQueue.Count == 0)
        {
            LineSegment newPoint = new LineSegment(pos);
            pointQueue.AddLast(newPoint);
        }

        else
        {
            LineSegment lastPoint = pointQueue.Last.Value;
            
            //check distance
            if (Vector2.Distance(pos, lastPoint.endPoint) > stepDist)
            {
                LineSegment newPoint = new LineSegment(Vector2.Distance(lastPoint.endPoint, pos), pos);
                pointQueue.AddLast(newPoint);
            }
        }

        //check total length of line
        float length = pointQueue.Select(item => item.length).ToArray().Sum();
        
        while (length > maxDist)
        {
            pointQueue.RemoveFirst();

            LineSegment newFirst = pointQueue.First.Value;
            length -= newFirst.length;
            newFirst.length = 0;
        }

        //Grab positions, Set New Line
        Vector2[] positions = pointQueue.Select(item => item.endPoint).ToArray();
        Vector3[] positions3 = positions.Select(item => (Vector3) item).ToArray();
        line.positionCount = positions.Length;
        line.SetPositions(positions3);
        edgeCol.points = new Vector2[0];

        if (positions.Length > 2)
        {
            edgeCol.points = positions[..(positions.Length - 2)];
        }
        
    }

    private bool CanAppend(Vector2 pos)
    {
        if (line == null) return false;
        if (line.positionCount == 0)
        {
            return true;
        }

        float lineDist = Vector2.Distance(line.GetPosition(line.positionCount - 1), pos);

        return (lineDist > 0.1f);
    }

    public void ClearHalf()
    {
        int length = pointQueue.Count;
        int mid = length / 2;
        int count = 0;

        LinkedListNode<LineSegment> currentNode = pointQueue.First;
        while(count < mid)
        {
            currentNode.Value.decay = true;
            currentNode = currentNode.Next;
            count++;
        }
    }

    private void CloseLoop(Vector2 closePoint)
    {
        int closeIndex = Array.IndexOf(edgeCol.points, closePoint);
        Vector2[] loop = edgeCol.points[closeIndex..];
        loopCol.points = loop;
        Debug.Log("Loop Detected");

        ClearHalf();
    }

    private Vector2 ClosestVertex(Vector2 other)
    {
        float minDist = Mathf.Infinity;
        Vector2 res = Vector2.zero;

        foreach(Vector2 point in edgeCol.points)
        {
            float dist = Vector2.Distance(point, other);
            if (dist < minDist)
            {
                minDist = dist;
                res = point;
            }
        }

        return res;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Head")
        {
            Vector2 closePoint = ClosestVertex(other.transform.position);
            CloseLoop(closePoint);
        }
    }
}
