using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

class LineSegment
{
    public float length;
    public Vector2 endPoint;

    public LineSegment(Vector2 endPoint)
    {
        this.length = 0;
        this.endPoint = endPoint;
    }

    public LineSegment(float length, Vector2 endPoint)
    {
        this.length = length;
        this.endPoint = endPoint;
    }
}

public class CaptureLine : MonoBehaviour
{
    public LineRenderer line;
    [SerializeField] private float maxDist;
    private LinkedList<LineSegment> pointQueue;

    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
        pointQueue = new LinkedList<LineSegment>();
    }

    // Update is called once per frame
    void Update()
    {
        
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
            LineSegment newPoint = new LineSegment(Vector2.Distance(lastPoint.endPoint, pos), pos);
            pointQueue.AddLast(newPoint);
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
        Vector3[] positions = pointQueue.Select(item => (Vector3) item.endPoint).ToArray();
        line.positionCount = positions.Length;
        line.SetPositions(positions);
    }

    private bool CanAppend(Vector2 pos)
    {
        if (line.positionCount == 0)
        {
            return true;
        }

        float lineDist = Vector2.Distance(line.GetPosition(line.positionCount - 1), pos);

        return (lineDist > 0.1f);
    }
}
