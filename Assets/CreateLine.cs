using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateLine : MonoBehaviour
{
    LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void DrawLine(Vector3[] points)
    {
        lineRenderer.positionCount = points.Length;
        lineRenderer.SetPositions(points);
        lineRenderer.startColor = Color.magenta;
        lineRenderer.endColor = Color.blue;
        /*lineRenderer.startWidth = 1;
        lineRenderer.endWidth = 1;*/
    }

}