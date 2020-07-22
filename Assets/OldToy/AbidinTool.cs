using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abidin/Tool", order = 1)]
public class AbidinTool : ScriptableObject
{
    public float p1x;
    public float p1y;
    public float p2x;
    public float p2y;
    public float p3x;
    public float p3y;
    public Triangle triangle;
    public string toolName;
    
    private void OnEnable()
    {
        triangle = new Triangle(p1x, p1y, p2x, p2y, p3x, p3y);
    }
}
