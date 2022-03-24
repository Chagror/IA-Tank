using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Behavior Tree/Decorator/Target Enemy")]
public class BTDTargetEnemy : BTDecorator
{
    public override bool Execute()
    {
        GameObject enemy = blackboard.GetValue<GameObject>("TargetEnemy");
        if (enemy && enemy.activeInHierarchy)
        {
            success = true;
        }
        else success = false;
        return base.Execute();
    }

}
