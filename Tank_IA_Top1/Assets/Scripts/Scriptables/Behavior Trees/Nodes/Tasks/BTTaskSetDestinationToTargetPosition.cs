using System.Collections;
using System.Collections.Generic;
using Complete;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Behavior Tree/Task/Set Destination to Target Position")]
public class BTTaskSetDestinationToTargetPosition : BTTask
{
    public override bool Execute()
    {
        Complete.TankMovement tankMovement = controller.GetComponent<Complete.TankMovement>();
        
        GameObject enemy = blackboard.GetValue<GameObject>("TargetEnemy");
        
        if (enemy && tankMovement)
        {
            Vector3 currentTankPosition = tankMovement.transform.position;

            Vector3 currentEnemyPosition = enemy.transform.position;
            
            List<Vector3> path = tankMovement.GetCurrentPathMovement().CalculatePath(currentTankPosition, currentEnemyPosition);
        
            blackboard.SetValue("CurrentPath", path);
            success = true;
        }
        else
        {
            success = false;
        }
        
        return base.Execute();
    }
    
    
    
    
    
}
