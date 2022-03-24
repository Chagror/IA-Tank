using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "AI/Movement/NavMesh")]
public class NavMeshMovement : PathMovement
{
    public override List<Vector3> CalculatePath(Vector3 startPosition, Vector3 targetPosition)
    {
        NavMeshPath path = new NavMeshPath();

        if (!NavMesh.CalculatePath(startPosition, targetPosition, NavMesh.AllAreas,
                                path))
        {
            path = new NavMeshPath();
        }

        return new List<Vector3>(path.corners);
    }

    public override void Init(Complete.TankMovement tankMovement)
    {
        tankMovement.m_NavMeshAgent.enabled = true;
        tankMovement.m_NavMeshAgent.updatePosition = false;
        tankMovement.m_NavMeshAgent.updateRotation = false;
    }

}
