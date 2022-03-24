using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Behavior Tree/Node/Parallel Node")]
public class BTParallel : BTCompositeNode
{
    public override bool Execute()
    {
        success = false;
        
        foreach (BTNode children in childrenNodes)
        {
            if (children.Execute())
            {
                success = true;
            }
        }

        return base.Execute();
    }
}
