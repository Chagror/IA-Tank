using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Complete;

[CreateAssetMenu(menuName = "AI/Behavior Tree/Task/Patrol")]
public class BTTaskPatrol : BTTask
{
    public override bool Execute()
    {
        GameObject helipad = controller.blackboard.GetValue<GameObject>("Helipad");
        float DistanceToPatrolAroundHelipad = blackboard.GetValue<float>("PatrolRange") - 3f;
        TankMovement tankMovement = controller.GetComponent<TankMovement>();

        Vector3 targetPos = helipad.transform.position;
        targetPos.x = helipad.transform.position.x + DistanceToPatrolAroundHelipad * Mathf.Cos(Time.time * 1.15f);
        targetPos.z = helipad.transform.position.z + DistanceToPatrolAroundHelipad * Mathf.Sin(Time.time * 1.15f);
        
        List<Vector3> path = tankMovement.GetCurrentPathMovement().CalculatePath(tankMovement.transform.position, targetPos);

        if (path == null)
        {
            success = false;
        }
        else
        {
            tankMovement.SetPath(path);
        }
        return base.Execute();
    }
}