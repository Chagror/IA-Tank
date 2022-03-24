using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Enums
{
    public enum ControlType
    {
        Manual,
        UnityNavMesh,
        Dijkstra,
        AStar
    }
    
    public enum CompareType
    {
        EqualTo,
        SuperiorTo,
        InferiorTo,
        SuperiorOrEqualTo,
        InferiorOrEqualTo
    }
    
}
