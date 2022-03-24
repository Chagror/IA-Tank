using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Complete;

[CreateAssetMenu(menuName = "AI/Behavior Tree/Task/GoTo Helipad")]
public class BTTaskGoToHelipad : BTTask
{
    public override bool Execute()
    {
        GameObject helipad = controller.GetBlackboard().GetValue<GameObject>("Helipad");
        TankMovement tankMovement = controller.GetComponent<TankMovement>();
        List<Vector3> path = tankMovement.GetCurrentPathMovement().CalculatePath(tankMovement.transform.position, helipad.transform.position);

        if (path == null)
        {
            success = false;
        }
        else
        {
            tankMovement.SetPath(path);
            success = true;
        }

        return base.Execute();
    }
}
