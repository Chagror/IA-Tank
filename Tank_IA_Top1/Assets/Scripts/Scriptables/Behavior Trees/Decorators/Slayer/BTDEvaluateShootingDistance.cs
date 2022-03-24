using System.Collections;
using System.Collections.Generic;
using Complete;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Behavior Tree/Decorator/Check Shooting Distance")]
public class BTDEvaluateShootingDistance : BTDecorator
{
    private float shootDistance;
    private TankMovement tankMovement;
    public override void Init(BTController newController)
    {
        base.Init(newController);
        shootDistance = blackboard.GetValue<float>("ShootDistance");
        tankMovement = controller.GetComponent<TankMovement>();
    }

    public override bool Execute()
    {
        Vector3 enemyTargetPosition = blackboard.GetValue<GameObject>("TargetEnemy").transform.position;

        float distanceToEnemyTarget = Vector3.Distance(tankMovement.transform.position, enemyTargetPosition);

        success = distanceToEnemyTarget <= shootDistance;

        return base.Execute();
    }
}
