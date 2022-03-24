using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Behavior Tree/Task/AvoidShells")]
public class BTTaskAvoidShells : BTTask
{
    private float detectionRange;
    private float shellAvoidanceTolerance;
    public LayerMask shellLayerMask;
    private float dodgeDistance;

    public override void Init(BTController newController)
    {
        base.Init(newController);
        detectionRange = blackboard.GetValue<float>("RangeDetectShells");
        shellAvoidanceTolerance = blackboard.GetValue<float>("ShellAvoidanceTolerance");
        dodgeDistance = blackboard.GetValue<float>("DodgeDistance");
    }

    public override bool Execute()
    {
        Complete.TankMovement tankMovement = controller.GetComponent<Complete.TankMovement>();
        Vector3 tankPosition = tankMovement.transform.position;

        Ray detectionRay = new Ray();

        List<RaycastHit> hits = new List<RaycastHit>(Physics.SphereCastAll(detectionRay, detectionRange, Mathf.Infinity, shellLayerMask));
        
        if (hits.Count != 0)
        {
            GameObject nearestShell = null;
            float shortestShellDistance = Mathf.Infinity;
            
            foreach (var hit in hits)
            {
                if (hit.distance < shortestShellDistance)
                {
                    nearestShell = hit.collider.gameObject;
                }
            }

            if (nearestShell != null)
            {
                float dot = Vector3.Dot(nearestShell.transform.position,
                    nearestShell.transform.position - tankPosition);

                if (dot >= shellAvoidanceTolerance)
                {
                    float angleFromShell = Vector3.SignedAngle(tankPosition - nearestShell.transform.position,
                        nearestShell.transform.forward, Vector3.up);

                    Vector3 dodgeDirection;
                    Vector3 dodgeDestination;
                    
                    if (nearestShell != blackboard.GetValue<GameObject>("CurrentAvoidedShell"))
                    {
                        blackboard.SetValue("CurrentAvoidedShell", nearestShell);
                        
                        if (angleFromShell < 0)
                        {
                            dodgeDirection = -nearestShell.transform.right;
                            dodgeDestination = tankPosition + dodgeDirection * dodgeDistance;
                            dodgeDestination = tankMovement.GetNearestWalkablePosition(dodgeDestination);

                        }
                        else
                        {
                            dodgeDirection = -nearestShell.transform.right;
                            dodgeDestination = tankPosition + dodgeDirection * dodgeDistance;
                            dodgeDestination = tankMovement.GetNearestWalkablePosition(dodgeDestination);
                        }

                        List<Vector3> dodgePath = tankMovement.GetCurrentPathMovement()
                            .CalculatePath(tankMovement.transform.position, dodgeDestination);
                        
                        blackboard.SetValue("CurrentPath", dodgePath);
                        
                        tankMovement.SetPath(dodgePath);

                        success = true;
                        
                        return base.Execute();
                    }
                }
            }
        }

        success = false;
        
        return base.Execute();
    }
}
