using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Movement/A*")]
public class AStarMovement : PathMovement
{
    public override List<Vector3> CalculatePath(Vector3 startPosition, Vector3 targetPosition)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();
        Node startNode = Pathfinding.grid.GetNodeFromWorldPos(startPosition);
        Node targetNode = Pathfinding.grid.GetNodeFromWorldPos(targetPosition);

        Heap<Node> openSet = new Heap<Node>(Pathfinding.grid.MaxSize);
        HashSet<Node> closedSet = new HashSet<Node>();

        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet.RemoveFirstItem();

            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                sw.Stop();
                UnityEngine.Debug.Log($"Path found in: {sw.ElapsedMilliseconds}ms");
                return ConvertNodeToVector3(Pathfinding.RetracePath(startNode, targetNode));
            }

            foreach (Node neighbour in Pathfinding.grid.GetNeighbours(currentNode))
            {
                if (!neighbour.walkable || closedSet.Contains(neighbour)) { continue; }

                int newMovementCostToNeighbour = currentNode.gCost + Pathfinding.GetDistance(currentNode, neighbour);

                if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = Pathfinding.GetDistance(neighbour, targetNode);
                    neighbour.parentNode = currentNode;

                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                }
            }
        }
        return null;
    }

    public override void Init(Complete.TankMovement tankMovement)
    {
        tankMovement.m_NavMeshAgent.enabled = false;
    }
}
