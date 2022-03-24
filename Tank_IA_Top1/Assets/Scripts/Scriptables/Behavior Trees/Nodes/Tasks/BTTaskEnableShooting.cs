using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Behavior Tree/Task/Enable Shooting")]
public class BTTaskEnableShooting : BTTask
{
    private Complete.TankMovement tankMovement;
    private Complete.TankShooting tankShooting;
    private List<GameObject> allies;
    private List<GameObject> enemies;
    private List<Vector3> pathToEnemy;
    private float shootPrecision;
    public override void Init(BTController newController)
    {
        base.Init(newController);
        tankMovement = controller.GetComponent<Complete.TankMovement>();
        tankShooting = controller.GetComponent<Complete.TankShooting>();
        shootPrecision = blackboard.GetValue<float>("ShootPrecision");
        allies = blackboard.GetValue<List<GameObject>>("Allies");
        enemies = blackboard.GetValue<List<GameObject>>("Enemies");
        pathToEnemy = blackboard.GetValue<List<Vector3>>("CurrentPath");
    }

    public override bool Execute()
    {
        GameObject enemy = blackboard.GetValue<GameObject>("TargetEnemy");

        Vector3 futureTargetPosition = GetFutureEnemyPosition(enemy);
        float angleToTargetPosition = GetAngleToTarget(futureTargetPosition);

        success = false;
        
        if (Mathf.Abs(angleToTargetPosition) >= shootPrecision)
        {
            if (angleToTargetPosition <= 0)
            {
                tankMovement.m_TurnInputValue = -1;
            }
            else if (angleToTargetPosition >= 0)
            {
                tankMovement.m_TurnInputValue = 1;
            }
        }
        else
        {
            tankMovement.m_TurnInputValue = 0;

            tankShooting.m_Fired = true;

            //float distanceToTargetEnemy = Vector3.Distance(tankShooting.m_FireTransform.position, futureTargetPosition);


            /*if (Physics.Raycast(tankShooting.m_FireTransform.position, tankShooting.m_FireTransform.forward,
                    out RaycastHit hit, distanceToTargetEnemy))
            {
                if (allies.Contains(hit.collider.gameObject))
                {
                    tankMovement.SetPath(pathToEnemy);
                    tankShooting.m_Fired = false;
                }
                else if (enemies.Contains(hit.collider.gameObject))
                {
                    tankShooting.m_Fired = true;
                    success = true;
                }
                else
                {
                    float distanceToObstacle = Vector3.Distance(tankShooting.m_FireTransform.position, hit.point);
                    float distanceToFutureEnemyPosition =
                        Vector3.Distance(tankShooting.m_FireTransform.position, futureTargetPosition);

                    if (distanceToObstacle > distanceToTargetEnemy)
                    {
                        tankShooting.m_Fired = true;
                        success = true;
                    }
                    else
                    {
                        tankMovement.SetPath(pathToEnemy);
                    }
                }
            }
            else
            {
                tankShooting.m_Fired = false;
            }*/

        }
        
        tankMovement.SetPath(new List<Vector3>());
        
        return base.Execute();
    }

    Vector3 GetFutureEnemyPosition(GameObject enemy)
    {
        Vector3 currentEnemyPosition = enemy.transform.position;

        float predictedShellVelocity = 0f;
        float timeForShellToReachEnemy = 0f;

        Vector3 predictedEnemyPosition = Vector3.zero;

        if (tankShooting)
        {
            predictedShellVelocity = (tankShooting.m_LaunchForce * tankShooting.m_Shell.mass) * Time.fixedDeltaTime;
            timeForShellToReachEnemy = Vector3.Distance(tankMovement.transform.position, currentEnemyPosition) / predictedShellVelocity;
        }

        Rigidbody enemyRigidbody = enemy.GetComponent<Rigidbody>();

        if (enemyRigidbody != null)
        {
            Vector3 enemyVelocity = enemyRigidbody.velocity;
            predictedEnemyPosition = currentEnemyPosition + enemyVelocity * timeForShellToReachEnemy;
        }

        return predictedEnemyPosition;
    }

    float GetAngleToTarget(Vector3 targetPosition)
    {
        return Vector3.SignedAngle(tankShooting.m_FireTransform.forward,
            targetPosition - tankShooting.m_FireTransform.position, Vector3.up);
    }

}
