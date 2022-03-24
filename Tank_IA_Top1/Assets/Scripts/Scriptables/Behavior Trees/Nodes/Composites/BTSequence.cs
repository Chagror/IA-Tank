using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Behavior Tree/Node/Sequence Node")]
public class BTSequence : BTCompositeNode
{
    public override bool Execute()
    {
        foreach (BTNode children in childrenNodes)
        {
            if (!children.Execute())
            {
                success = false;
                break;
            }
        }
        
        return base.Execute();
    }
}
