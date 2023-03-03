using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Match;

public class SightCurveScript : MonoBehaviour
{
    [SerializeField] private Transform point1;
    //TODO : change x0 and y0 to specific transform axis from point1
    [SerializeField] private float x0;
    [SerializeField] private float h0;
    [SerializeField] private Transform point2;
    [SerializeField] private Transform point3;
    [SerializeField] private Transform barrel;
    public int vertexCount = 12;
    public bool debug = false;
    public Material mat;
    
    private LineRenderer _lineRenderer;

    private float g = -Physics.gravity.y;
    private float v0 = 1000 / 50;
    
    private void Start()
    {
        _lineRenderer = gameObject.GetComponent<LineRenderer>();
        _lineRenderer.material = mat;
    }

    private void Update()
    {
        double alpha = barrel.localEulerAngles.x * Math.PI / 180;

        if (alpha > 0)
        {
            DrawLineRenderer(alpha);   
        }
        else
        {
            _lineRenderer.positionCount = 0;
            _lineRenderer.SetPositions(Array.Empty<Vector3>());
        }
    }

    private void DrawLineRenderer(double alpha)
    {
        double hmax =  Mathf.Pow(v0 * (float)alpha, 2) / (2*g);
        double dmax = dmaxCalcul(alpha);
        print("x : "+x0+" ; h0 : "+h0);
        print("dmax : "+dmax+" ; hmax : "+hmax);
        
        point2.localPosition = new Vector3(x0, (float)hmax, (float)dmax/2);
        point3.localPosition = new Vector3(x0, h0, (float)dmax);
        
        transform.RotateAround(point1.position, Vector3.up, 0);
        
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

    private double dmaxCalcul(double alpha)
    {
        double dmax = 0;
        double a = g / 2;
        double b = v0 * Math.Sin(alpha);
        float delta = Mathf.Pow((float)b,2);
        if (delta > 0)
        {
            double t1 = (-b + Mathf.Sqrt(delta)) / (2 * a);
            double t2 = (-b - Mathf.Sqrt(delta)) / (2 * a);
            double txmax;
            if (Mathf.Cos((float)alpha) > 0)
                txmax = t2;
            else if (Mathf.Cos((float)alpha) < 0)
                txmax = t1;
            else
                txmax = 0;

            dmax = v0 * Mathf.Cos((float)alpha) * txmax;
        }

        return dmax;
    }
}
