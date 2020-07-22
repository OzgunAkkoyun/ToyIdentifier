using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllToys
{
    public List<float> ratios;
    public List<float> distances;
    public string toolName;
    public string toolIndex;

    public AllToys(List<float> _ratios,List<float> _distances, string _toolName, string _toolIndex)
    {
        ratios = _ratios;
        distances = _distances;
        toolName = _toolName;
        toolIndex = _toolIndex;
    }
}
