using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using System.Threading.Tasks;
using System.Diagnostics;
using Complete;


[RequireComponent(typeof(Grid))]
public class Pathfinding : MonoBehaviour
{
    public static Grid grid;
    public Transform seeker, target;

    [SerializeField]
    private PathMovement pathScriptable;

    private void Awake()
    {
        grid = GetComponent<Grid>();
    }

    private void Update()
    {
        /*Vector3 seekerPos = seeker.position;
        Vector3 targetPos = target.position;
        grid.SetPath(Task.Run(() => FindPathByAStar(seekerPos, targetPos)).Result);*/
    }

    /*private async Task<List<Node>> FindPathByAStar(Vector3 startPosition, Vector3 targetPosition)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();
        Node startNode = grid.GetNodeFromWorldPos(startPosition);
        Node targetNode = grid.GetNodeFromWorldPos(targetPosition);

        Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
        HashSet<Node> closedSet = new HashSet<Node>();

        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet.RemoveFirstItem();

            closedSet.Add(currentNode);

            if(currentNode == targetNode)
            {
                sw.Stop();
                UnityEngine.Debug.Log($"Path found in: {sw.ElapsedMilliseconds}ms");
                return RetracePath(startNode, targetNode);
            }

            foreach ( Node neighbour in grid.GetNeighbours(currentNode))
            {
                if(!neighbour.walkable || closedSet.Contains(neighbour)) { continue; }

                int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);

                if(newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parentNode = currentNode;

                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                }
            }
        }
        return null;
    }*/

    private async Task<List<Node>> FindPathByDijkstra(Vector3 startPosition, Vector3 targetPosition)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        Node startNode = grid.GetNodeFromWorldPos(startPosition);
        Node targetNode = grid.GetNodeFromWorldPos(targetPosition);

        List<Node> unvisitedNodes = new List<Node>();
        HashSet<Node> visitedNodes = new HashSet<Node>();

        //List initialization

        unvisitedNodes.Add(startNode);

        Node[,] nodesList = grid.GetGrid();

        for (int i = 0; i < grid.MaxSize; i++)
        {
            int xIndex = i % grid.gridSizeX;
            int yIndex = i / grid.gridSizeX;

            if (nodesList[xIndex, yIndex] != startNode) unvisitedNodes.Add(nodesList[xIndex, yIndex]);
        }

        //cost values initialization

        for (int i = 0; i < grid.MaxSize; i++)
        {
            int xIndex = i % grid.gridSizeX;
            int yIndex = i / grid.gridSizeX;
            nodesList[xIndex, yIndex].gCost = int.MaxValue;
        }

        startNode.gCost = 0;

        Node currentNode = startNode;

        while (unvisitedNodes.Count > 0) //while there are still nodes to visit
        {
            int lowestUnvisitedNodesCost = int.MaxValue;

            for(int i = 0; i < unvisitedNodes.Count; i++) //current node choosing
            {
               if(unvisitedNodes[i].gCost < lowestUnvisitedNodesCost)
               {
                    lowestUnvisitedNodesCost = unvisitedNodes[i].gCost;
                    currentNode = unvisitedNodes[i];
               }
            }

            List<Node> neighboursNodes = grid.GetNeighbours(currentNode);

            for(int i = 0; i < neighboursNodes.Count; i++) //neighbours examination
            {

                Node currentNeighBour = neighboursNodes[i];

                int distanceFromStartNode = GetDistance(startNode, currentNeighBour);

                if (!visitedNodes.Contains(currentNeighBour) && distanceFromStartNode < currentNeighBour.gCost && currentNeighBour.walkable)
                {
                    currentNeighBour.gCost = distanceFromStartNode;
                    currentNeighBour.parentNode = currentNode;

                    if(currentNeighBour == targetNode)
                    {
                        UnityEngine.Debug.Log($"Path found in: {sw.ElapsedMilliseconds}ms");
                        return RetracePath(startNode, targetNode);
                    }
                }
            }

            visitedNodes.Add(currentNode);
            unvisitedNodes.Remove(currentNode);
        }

        return null;
    }


    public static List<Node> RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while(currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parentNode;
        }

        path.Reverse();

        return path;
    }

    public static int GetDistance(Node nodeA, Node nodeB)
    {
        int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if(distX > distY)
        {
            return 14 * distY + 10 * (distX - distY);
        }
        return 14 * distX + 10 * (distY-distX);
    }
}
