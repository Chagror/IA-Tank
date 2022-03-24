using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Behavior Tree/Decorator/Is Too Far From Helipad")]
public class BTDIsTooFarFromHelipad : BTDecorator
{
    public override bool Execute()
    {
        if (blackboard.GetValue<float>("DistanceFromHelipad") > blackboard.GetValue<float>("PatrolRange")) success = true;
        else success = false;
        return base.Execute();
    }
    
}