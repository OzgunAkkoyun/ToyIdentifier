using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abidin/Tool List", order = 1)]
public class ToolList : ScriptableObject
{
    public List<AbidinTool> tools;
    public float errorMargin;

    public string IdentifyTool(Vector2 p1, Vector2 p2, Vector2 p3) {
        Triangle t = new Triangle(p1, p2, p3);
        foreach (AbidinTool tool in tools) {
            if (tool == null)
            {
                return "Tool NULL";
            }
            if (tool != null && tool.triangle == null)
            {
                return "Tool triangle NULL";
            }
            if (tool.triangle.hasSimilarRatio(t, errorMargin)) {
                
                return tool.toolName;
            }
        }
        return "Default";
    }

    public void ToolSort()
    {
       List<Sorted> sorted = new List<Sorted>();
        foreach (AbidinTool tool in tools)
        {
            sorted.Add(new Sorted(tool.triangle.getR1(), tool.triangle.getR2(), tool.triangle.getR3()));
           
        }
        Debug.Log(sorted[0].sort[0]);
    }

}

class Sorted
{
    private float r1;
    private float r2;
    private float r3;
    public List<float> sort = new List<float>();
    public Sorted(float _r1,float _r2,float _r3)
    {
        r1 = _r1;
        r2 = _r2;
        r3 = _r3;

        sort.Add(this.r1);
        sort.Add(this.r2);
        sort.Add(this.r3);
        sort.Sort();
    }

    public void Sort()
    {
        
    }

}