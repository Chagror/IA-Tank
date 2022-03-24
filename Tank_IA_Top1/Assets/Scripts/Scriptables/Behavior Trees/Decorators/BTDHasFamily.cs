using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Behavior Tree/Decorator/Has Family")]
public class BTDHasFamily : BTDecorator
{
    public override bool Execute()
    {
        GameObject family = blackboard.GetValue<GameObject>("FamilyRusher");
        if (family && family.activeInHierarchy)
        {
            success = true;
        }
        else success = false;
        
        return base.Execute();
    }
    
}