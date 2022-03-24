using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BTCompositeNode : BTNode
{
    [SerializeField] protected BTNode[] childrenNodes;

    public override void Init(BTController controller)
    {
        base.Init(controller);
        
        foreach (var children in childrenNodes)
        {
            children.Init(controller);
        }
    }
    
    public override bool Execute()
    {
        return base.Execute();
    }
}
