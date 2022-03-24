using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BTTask : BTNode
{
    protected BTController controller;
    protected BTBlackboard blackboard;
    
    public override void Init(BTController newController)
    {
        controller = newController;
        blackboard = controller.GetBlackboard();
    }

    //Return true if successfully executed
    public override bool Execute()
    {
        return base.Execute();
    }


    
}
