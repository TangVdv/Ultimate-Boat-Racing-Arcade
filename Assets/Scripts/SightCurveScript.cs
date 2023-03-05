using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SightCurveScript : MonoBehaviour
{
    [SerializeField] private Transform bodyCannon;
    [SerializeField] private Transform barrel;
    [SerializeField] private Transform point1;
    [SerializeField] private Transform point2;
    [SerializeField] private Transform point3;
    [SerializeField] private GameObject bullet;
    [Range(0, 50)] public int vertexCount;
    public bool debug = false;
    public Material mat;
    public float force;
    
    private LineRenderer _lineRenderer;
    private float _gravity;
    private float _initialVelocity;
    private float _x0;
    private float _h0;

    private void Start()
    {
        //TODO : Change calcul _initialVelocity (currently : acceleration calcul)
        _initialVelocity = force / bullet.GetComponent<Rigidbody>().mass;
        _gravity = -Physics.gravity.y;
        _x0 = point1.position.x;
        _h0 = point1.position.y;
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
        // Calculate the max height of the trajectory
        float hmax =  Mathf.Pow((float)(_initialVelocity * Math.Sin(alpha)), 2) / (2*_gravity);
        // Calculate the max distance of the trajectory
        double dmax = dmaxCalcul(alpha);
        
        point2.position = new Vector3(_x0, hmax, (float)dmax/2);
        point3.position = new Vector3(_x0, _h0, (float)dmax);

        // Calculate the rotation angle based on the body cannon
        float rotationAngle = bodyCannon.localEulerAngles.y;

        // Rotate point2 and point3 around point1
        Vector3 relativePos2 = point2.position - point1.position;
        Vector3 rotatedPos2 = Quaternion.AngleAxis(rotationAngle, Vector3.up) * relativePos2;
        point2.position = point1.position + rotatedPos2;

        Vector3 relativePos3 = point3.position - point1.position;
        Vector3 rotatedPos3 = Quaternion.AngleAxis(rotationAngle, Vector3.up) * relativePos3;
        point3.position = point1.position + rotatedPos3;
        
        // Create a list of points for the bezier curve
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

    // This method is used to draw Gizmos for debugging purpose
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
    
    // This method is used to calculate the max distance of the trajectory
    private double dmaxCalcul(double alpha)
    {
        double dmax = 0;
        double a = _gravity / 2;
        double b = _initialVelocity* Math.Sin(alpha);
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

            dmax = _initialVelocity * Mathf.Cos((float)alpha) * txmax;
        }

        return dmax;
    }
}
