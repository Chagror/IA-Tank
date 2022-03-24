using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Complete;

[CreateAssetMenu(menuName = "AI/Behavior Tree/Task/Get Distance To Helipad")]
public class BTTaskGetDistanceToHelipad : BTTask
{
    public override bool Execute()
    {
        GameObject helipad = blackboard.GetValue<GameObject>("Helipad");
        float distanceToHelipad = Vector3.Distance(helipad.transform.position,
            controller.GetComponent<TankMovement>().transform.position);

        blackboard.SetValue("DistanceFromHelipad", distanceToHelipad);

        return base.Execute();
    }
}