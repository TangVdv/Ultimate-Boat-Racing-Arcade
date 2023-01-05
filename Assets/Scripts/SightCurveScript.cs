using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SightCurveScript : MonoBehaviour
{
    [SerializeField] private Transform point1;
    [SerializeField] private Transform point2;
    [SerializeField] private Transform point3;
    public int vertexCount = 12;
    public bool debug = false;
    public Material mat;

    private LineRenderer _lineRenderer;

    private void Start()
    {
        _lineRenderer = gameObject.GetComponent<LineRenderer>();
        _lineRenderer.material = mat;
        point2.position = new Vector3(point1.position.x, point1.position.y + 50, point1.position.z - 50);
        point3.position = new Vector3(point1.position.x, point1.position.y, point1.position.z - 100);
    }

    private void Update()
    {
        point2.RotateAround(point1.position, Vector3.up, 0);
        point3.RotateAround(point1.position, Vector3.up, 0);
        
        var pointList = new List<Vector3>();
        for (float ratio = 0; ratio <= 1; ratio+= 1.0f / vertexCount)
        {
            var tangentLineVertex1 = Vector3.Lerp(point1.position, point2.position, ratio);
            var tangentLineVertex2 = Vector3.Lerp(point2.position, point3.position, ratio);
            var bezierPoint = Vector3.Lerp(tangentLineVertex1, tangentLineVertex2, ratio);
            pointList.Add(bezierPoint);
        }
        
        _lineRenderer.positionCount = pointList.Count;
        _lineRenderer.SetPositions(pointList.ToArray());
    }

    private void OnDrawGizmos()
    {
        if (debug)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(point1.position, point2.position);

            Gizmos.color = Color.red;
            Gizmos.DrawLine(point2.position, point3.position);

            Gizmos.color = Color.yellow;
            for (float ratio = .5f / vertexCount; ratio < 1; ratio += 1.0f / vertexCount)
            {
                Gizmos.DrawLine(
                    Vector3.Lerp(point1.position, point2.position, ratio),
                    Vector3.Lerp(point2.position, point3.position, ratio));
            }
        }

    }
}
