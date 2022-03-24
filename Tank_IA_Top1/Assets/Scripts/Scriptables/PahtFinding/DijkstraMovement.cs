using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Movement/Dijkstra")]
public class DijkstraMovement : PathMovement
{
    public override List<Vector3> CalculatePath(Vector3 startPosition, Vector3 targetPosition)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        Node startNode = Pathfinding.grid.GetNodeFromWorldPos(startPosition);
        Node targetNode = Pathfinding.grid.GetNodeFromWorldPos(targetPosition);

        List<Node> unvisitedNodes = new List<Node>();
        HashSet<Node> visitedNodes = new HashSet<Node>();

        //List initialization

        unvisitedNodes.Add(startNode);

        Node[,] nodesList = Pathfinding.grid.GetGrid();

        for (int i = 0; i < Pathfinding.grid.MaxSize; i++)
        {
            int xIndex = i % Pathfinding.grid.gridSizeX;
            int yIndex = i / Pathfinding.grid.gridSizeX;

            if (nodesList[xIndex, yIndex] != startNode) unvisitedNodes.Add(nodesList[xIndex, yIndex]);
        }

        //cost values initialization

        for (int i = 0; i < Pathfinding.grid.MaxSize; i++)
        {
            int xIndex = i % Pathfinding.grid.gridSizeX;
            int yIndex = i / Pathfinding.grid.gridSizeX;
            nodesList[xIndex, yIndex].gCost = int.MaxValue;
        }

        startNode.gCost = 0;

        Node currentNode = startNode;

        while (unvisitedNodes.Count > 0) //while there are still nodes to visit
        {
            int lowestUnvisitedNodesCost = int.MaxValue;

            for (int i = 0; i < unvisitedNodes.Count; i++) //current node choosing
            {
                if (unvisitedNodes[i].gCost < lowestUnvisitedNodesCost)
                {
                    lowestUnvisitedNodesCost = unvisitedNodes[i].gCost;
                    currentNode = unvisitedNodes[i];
                }
            }

            List<Node> neighboursNodes = Pathfinding.grid.GetNeighbours(currentNode);

            for (int i = 0; i < neighboursNodes.Count; i++) //neighbours examination
            {

                Node currentNeighBour = neighboursNodes[i];

                int distanceFromStartNode = Pathfinding.GetDistance(startNode, currentNeighBour);

                if (!visitedNodes.Contains(currentNeighBour) && distanceFromStartNode < currentNeighBour.gCost && currentNeighBour.walkable)
                {
                    currentNeighBour.gCost = distanceFromStartNode;
                    currentNeighBour.parentNode = currentNode;

                    if (currentNeighBour == targetNode)
                    {
                        UnityEngine.Debug.Log($"Path found in: {sw.ElapsedMilliseconds}ms");
                        return ConvertNodeToVector3(Pathfinding.RetracePath(startNode, targetNode));
                    }
                }
            }

            visitedNodes.Add(currentNode);
            unvisitedNodes.Remove(currentNode);
        }

        return null;
    }

    public override void Init(Complete.TankMovement tankMovement)
    {
        tankMovement.m_NavMeshAgent.enabled = false;
    }
}
