using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Behavior Tree/Behavior Tree")]
public class BehaviorTree : ScriptableObject
{
    [SerializeField] private BTController controller;
    [SerializeField] private BTNode startNode;
    
    public void Init(BTController newController)
    {
        controller = newController;
        startNode.Init(controller);
    }

    public void Execute()
    {
        startNode.Execute();
    }
    
    
}
