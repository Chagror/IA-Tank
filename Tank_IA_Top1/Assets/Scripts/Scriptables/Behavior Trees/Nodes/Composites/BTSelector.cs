using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Behavior Tree/Node/Selector Node")]
public class BTSelector : BTCompositeNode
{
    public override bool Execute()
    {
        success = false;
        
        foreach (BTNode children in childrenNodes)
        {
            if (children.Execute())
            {
                success = true;
                break;
            }
        }

        return base.Execute();
    }
}
