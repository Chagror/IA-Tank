using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Behavior Tree/Task/Start Shooting")]
public class BTTaskStartShooting : BTTask
{
    private Complete.TankMovement tankMovement;
    private Complete.TankShooting tankShooting;
    private List<GameObject> allies;
    private List<GameObject> enemies;
    private float shootPrecision;
    public override void Init(BTController newController)
    {
        base.Init(newController);
        tankMovement = controller.GetComponent<Complete.TankMovement>();
        tankShooting = controller.GetComponent<Complete.TankShooting>();
        shootPrecision = blackboard.GetValue<float>("ShootPrecision");
        allies = blackboard.GetValue<List<GameObject>>("Allies");
        enemies = blackboard.GetValue<List<GameObject>>("Enemies");
    }

    public override bool Execute()
    {
        
        GameObject enemy = blackboard.GetValue<GameObject>("TargetEnemy");

        Vector3 futureTargetPosition = GetFutureEnemyPosition(enemy);
        float angleToTargetPosition = GetAngleToTarget(futureTargetPosition);

        success = false;

        Debug.LogWarning($"{controller.GetComponent<Complete.TankMovement>().tankTeam.teamName} {controller.behaviorTree.name} : {angleToTargetPosition}/{shootPrecision}");

        if (Mathf.Abs(angleToTargetPosition) >= shootPrecision)
        {
            if (angleToTargetPosition <= 0)
            {
                tankMovement.m_TurnInputValue = -0.7f;
            }
            else if (angleToTargetPosition >= 0)
            {
                tankMovement.m_TurnInputValue = .7f;
            }
        }

        tankShooting.m_Fired = true;
        success = true;
        /*else
        {
            tankMovement.m_TurnInputValue = 0;

            float distanceToTargetEnemy = Vector3.Distance(tankShooting.m_FireTransform.position, futureTargetPosition);

            RaycastHit hit;

            tankShooting.m_Fired = true;
            success = true;*/
        /*if (Physics.Raycast(tankShooting.m_FireTransform.position, tankShooting.m_FireTransform.forward,
                out hit, distanceToTargetEnemy))
        {
            Debug.LogWarning($"if : {hit.collider.name}");
            if (allies.Contains(hit.collider.gameObject))
            {
                tankShooting.m_Fired = false;
            }
            else if (enemies.Contains(hit.collider.gameObject))
            {
                tankShooting.m_Fired = true;
                success = true;
            }
            else
            {
                tankShooting.m_Fired = true;
                success = true;
                /*float distanceToObstacle = Vector3.Distance(tankShooting.m_FireTransform.position, hit.point);
                float distanceToFutureEnemyPosition =
                    Vector3.Distance(tankShooting.m_FireTransform.position, futureTargetPosition);

                tankShooting.m_Fired = false;

                if (distanceToObstacle > distanceToTargetEnemy)
                {
                    tankShooting.m_Fired = true;
                    success = true;
                }
            }
        }
        else
        {
            Debug.LogWarning($"else : {hit}");
            tankShooting.m_Fired = false;
        }*/

    //}
        
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
            predictedEnemyPosition = currentEnemyPosition + enemyVelocity * timeForShellToReachEnemy * Time.fixedDeltaTime;
        }

        return predictedEnemyPosition;
    }

    float GetAngleToTarget(Vector3 targetPosition)
    {
        return Vector3.SignedAngle(tankShooting.m_FireTransform.forward,
            targetPosition - tankShooting.m_FireTransform.position, Vector3.up);
    }
}
