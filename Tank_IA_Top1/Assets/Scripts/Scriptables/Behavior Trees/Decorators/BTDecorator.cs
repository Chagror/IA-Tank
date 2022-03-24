using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class BTDecorator : BTNode
{
    protected BTController controller;
    protected BTBlackboard blackboard;

    public BTNode childNode;

    public override void Init(BTController newController)
    {
        controller = newController;
        blackboard = controller.GetBlackboard();
        childNode?.Init(controller);
    }

    public override bool Execute()
    {
        if (success) childNode.Execute();
        return success;
    }
    
}
