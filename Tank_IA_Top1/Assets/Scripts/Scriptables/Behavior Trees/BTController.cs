using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BTController : MonoBehaviour
{
    public BehaviorTree behaviorTree;
    public BTBlackboard blackboard;

    private void Start()
    {
        blackboard?.Init(this);
        behaviorTree?.Init(this);
        if (!blackboard) Debug.LogError($"No blackboard on the tank: {name}");
        if (!behaviorTree) Debug.LogError($"No behaviorTree on the tank: {name}");

    }

    private void Update()
    {
        behaviorTree?.Execute();
        if (!behaviorTree) Debug.LogError($"Cannot execute the behaviorTree, it's value is {behaviorTree}, on the tank: {name}");
    }

    public BTBlackboard GetBlackboard()
    {
        return blackboard;
    }
}
