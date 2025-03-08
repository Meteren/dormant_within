using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathFinder : MonoBehaviour
{   
    PathGrid[,] pathGrids;

    [Header("Reference")]
    [SerializeField] private BoxCollider reference;

    int gridLenght;

    int[,] directions;

    PathGrid centerGrid;

    private void Start()
    {
        directions = new int[8, 2]
        {
            {0,1},{1,0},{-1,0},{0,-1},{1,1},{-1,-1},{1,-1},{-1,1}
        };
    }

    public void InitPathGridTable(int gridRadius)
    {
        CreateGrids(gridRadius);
    }
    private void CreateGrids(int radius)
    {
        gridLenght = radius * 2 + 1;
        pathGrids = new PathGrid[gridLenght, gridLenght];
        Vector3 agentPosition = transform.position;
        Vector2 bottomSize = new Vector2(reference.size.x, reference.size.z);
        Debug.Log($"Size x: {bottomSize.x} - Size y: {bottomSize.y}");
        Vector3 startPosition = new Vector3(agentPosition.x - 
            (bottomSize.x * radius), agentPosition.y, agentPosition.z + (bottomSize.y * radius));
        float capturedX = startPosition.x;
        Debug.Log($"Size x: {bottomSize.x} - Size y: {bottomSize.y}");

        for(int i = 0; i < pathGrids.GetLength(0); i++)
        {
            for(int j = 0; j < pathGrids.GetLength(1); j++)
            {
                GameObject pathGridObject = new GameObject($"PathGrid {i}-{j}");
                PathGrid pathGrid = pathGridObject.AddComponent<PathGrid>();
                pathGrid.InitPathGrid(transform,reference,startPosition,j,i);
                pathGrids[i, j] = pathGrid;
                startPosition.x += bottomSize.x;
            }
            startPosition.z -= bottomSize.y;
            startPosition.x = capturedX;
        }

        centerGrid = pathGrids[radius, radius];

    }

    public void DrawPathAndMove(Vector3 positionToMove,float speed)
    {
        List<PathGrid> neighbours = GetNeighbours();
        List<PathGrid> availableNeighbours = neighbours.Where(x => x.isMovable).ToList();
        availableNeighbours.Sort((x, y) =>
        x.CalculateDistance(positionToMove).CompareTo(y.CalculateDistance(positionToMove)));
        PathGrid choosenPath = neighbours[0];
        Move(choosenPath, speed);
    }

    public void Move(PathGrid gridToMove, float speed) => 
        Vector3.MoveTowards(transform.position, gridToMove.transform.position, Time.deltaTime * speed);

    private List<PathGrid> GetNeighbours()
    {
        List<PathGrid> neighbours = new List<PathGrid>();
        for(int i = 0; i < 8; i++)
        {
            PathGrid pathGrid = pathGrids[centerGrid.X + directions[i, 0], centerGrid.Y + directions[i, 1]];
            neighbours.Add(pathGrid);
        }

        return neighbours;
    }

}
