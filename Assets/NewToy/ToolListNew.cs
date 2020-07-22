using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ToyIdentifier/Tool List New", order = 1)]
public class ToolListNew : ScriptableObject
{
    public List<Tools> tools;
    public float errorMargin;
  
}
