using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PathMovement : ScriptableObject
{
    public Enums.ControlType movementType;
    public virtual List<Vector3> CalculatePath(Vector3 startPosition, Vector3 targetPosition)
    {
        return null;
    }

    protected List<Vector3> ConvertNodeToVector3(List<Node> nodes)
    {
        List<Vector3> positions = new List<Vector3>();
        Pathfinding.grid.SetPath(nodes);
        nodes.ForEach((node) => positions.Add(node.worldPosition));
        return positions;
    }

    public virtual void Init(Complete.TankMovement tankMovement)
    {

    }
}
