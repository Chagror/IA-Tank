using System.Collections;
using System.Collections.Generic;
using Complete;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Behavior Tree/Task/MoveToEnemyTarget")]
public class BTTMoveToEnemyTarget : BTTask
{
    private TankMovement tankMovement;
    
    public override void Init(BTController newController)
    {
        base.Init(newController);

        tankMovement = controller.GetComponent<TankMovement>();

    }

    public override bool Execute()
    {
        List<Vector3> path = blackboard.GetValue<List<Vector3>>("CurrentPath");

        if (path.Count > 0)
        {
            success = true;
            tankMovement.SetPath(path);
        }
        else
        {
            success = false;
        }
        
        return base.Execute();
    }
}
