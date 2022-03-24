using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public static Grid instance;

    public LayerMask nonWalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    public static Node[,] grid;

    //Debug purpose only
    public List<Node> path;

    float nodeDiameter;

    public Dictionary<Node, Transform> nodeBlockedDynamicly = new Dictionary<Node, Transform>();

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);

        instance = this;
    }

    public int MaxSize
    {
        get { return gridSizeX * gridSizeY; }
    }

    public int gridSizeX, gridSizeY;

    public Node[,] GetGrid()
    {
        return grid;
    }

    public void SetPath(List<Node> newPath)
    {
        path = newPath;
    }

    private void Start()
    {
        //Init all variables
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

        CreateGrid();
    }

    private void Update()
    {
        List<Node> nodesToRemove = new List<Node>();
        foreach (var keyValuePair in nodeBlockedDynamicly)
        {
            if(GetNodeFromWorldPos(keyValuePair.Value.position) != keyValuePair.Key) 
            {
                keyValuePair.Key.walkable = true;
                nodesToRemove.Add(keyValuePair.Key);
            }
        }

        foreach (var nodeToRemove in nodesToRemove)
        {
            nodeBlockedDynamicly.Remove(nodeToRemove);
        }
    }

    public void AddDynamiclyUnwalkableNode(Node node, Transform tr)
    {
        nodeBlockedDynamicly.Add(node, tr);
        node.walkable = false;
    }

    private void CreateGrid()
    {
        //Create a grid and set the bottom left angle position
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                //Get the position of the current node
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);

                //Check if the node is walkable
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, nonWalkableMask));

                //Create the node
                grid[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }

    public Node GetNodeFromWorldPos(Vector3 worldPos)
    {
        float percentX = (worldPos.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPos.z + gridWorldSize.y / 2) / gridWorldSize.y;

        //The percent shouldn't be above 1 and under 0
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        return grid[x, y];
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

        if(grid != null)
        {
            foreach (Node node in grid)
            {
                Gizmos.color = (node.walkable) ? Color.green : Color.red;
                if(path != null)
                {
                    if (path.Contains(node))
                    {
                        Gizmos.color = Color.black;
                    }
                }
                Gizmos.DrawCube(node.worldPosition, Vector3.one * (nodeDiameter-.1f));
            }
        }
    }

    public List<Node> GetNeighbours(Node baseNode)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if(x==0 && y == 0) { continue; }

                int checkX = baseNode.gridX + x;
                int checkY = baseNode.gridY + y;

                if(checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }
}
