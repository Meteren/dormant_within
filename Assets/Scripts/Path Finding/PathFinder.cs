using System;
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

    public List<PathGrid> DrawPath(Vector3 positionToMove)
    {
        List<PathGrid> foundPath = FindPath(positionToMove);
        return foundPath;
    }

    private List<PathGrid> FindPath(Vector3 positionToMove)
    {
        List<PathGrid> gridList = ExtractGridsToList();
        List<PathGrid> openGrids = new List<PathGrid>() {centerGrid};
        HashSet<PathGrid> closedGrids = new HashSet<PathGrid>();
        Dictionary<PathGrid,PathGrid> cameFrom = new Dictionary<PathGrid,PathGrid>();
        PathGrid destinationGrid = GetClosestGridToDestination(positionToMove);

        if (destinationGrid != null)
            Debug.Log("Name of dest grid:" + destinationGrid.name);

        foreach(var grid in gridList)
        {
            grid.GScore = float.MaxValue;
            grid.FScore = float.MaxValue;
        }

        centerGrid.GScore = 0;
        centerGrid.FScore = centerGrid.GScore + centerGrid.CalculateDistance(destinationGrid.transform.position);

        while(openGrids.Count > 0)
        {
            //Debug.Log("Loop");
            PathGrid currentGrid = openGrids.OrderBy(g => g.FScore).First();

            if (currentGrid == destinationGrid)
                return ConstructPath(cameFrom,destinationGrid);

            openGrids.Remove(currentGrid);
            closedGrids.Add(currentGrid);

            List<PathGrid> neighbours = GetNeighbours(currentGrid);

            foreach(var neighbour in neighbours)
            {
                //Debug.Log($"{X}-{Y} isMovable: {isMovable}");
                if (closedGrids.Contains(neighbour) || !neighbour.isMovable)
                    continue;

                float tentativeGScore = currentGrid.GScore + currentGrid.CalculateDistance(neighbour.transform.position);

                if(tentativeGScore < neighbour.GScore)
                {
                    neighbour.GScore = tentativeGScore;
                    neighbour.FScore = neighbour.GScore + neighbour.CalculateDistance(destinationGrid.transform.position);
                    cameFrom[neighbour] = currentGrid;

                    if(!openGrids.Contains(neighbour))
                        openGrids.Add(neighbour);
                }
            }

        }
        Debug.Log("Ready to return null");
        return new List<PathGrid>();
    }

    private List<PathGrid> ConstructPath(Dictionary<PathGrid, PathGrid> cameFrom, PathGrid destinationGrid)
    {
        Debug.Log("Ready to construct path");
        PathGrid grid = destinationGrid;
        List<PathGrid> path = new List<PathGrid>();
        while(cameFrom.ContainsKey(grid))
        {
            Debug.Log("Construct");
            path.Add(grid);
            grid = cameFrom[grid];

        }
        path.Reverse();
        return path;
    }


    private List<PathGrid> GetNeighbours(PathGrid grid)
    {
        List<PathGrid> neighbours = new List<PathGrid>();
        for(int i = 0; i < 8; i++)
        {
            PathGrid neighbourGrid;
            if (IsInBoundaries(grid, i))
                neighbourGrid = pathGrids[grid.Y + directions[i, 1], grid.X + directions[i, 0]];
            else
                neighbourGrid = null;

            if(neighbourGrid != null)
            {
                neighbours.Add(neighbourGrid);
                //Debug.Log("Neighbour:" + neighbourGrid.name);
            }
                
        }

        return neighbours;
    }

    private bool IsInBoundaries(PathGrid grid, int i)
    {
        return grid.X + directions[i, 0] < pathGrids.GetLength(0) && grid.X + directions[i, 0] >= 0 &&
            grid.Y + directions[i, 1] < pathGrids.GetLength(1) && grid.Y + directions[i, 1] >= 0;   
    }

    private PathGrid GetClosestGridToDestination(Vector3 destination)
    {
        List<PathGrid> gridList = ExtractGridsToList();

        gridList.Sort((x, y) => 
        x.CalculateDistance(destination).CompareTo(y.CalculateDistance(destination)));
        //Debug.Log($"Closest grid {gridList[0].name}");

        return gridList[0];
    }

    private List<PathGrid> ExtractGridsToList()
    {
        List<PathGrid> gridList= new List<PathGrid>();
        for(int i = 0; i < pathGrids.GetLength(0); i++)
        {
            for(int j = 0; j < pathGrids.GetLength(1); j++)
            {
                gridList.Add(pathGrids[i, j]);
            }
        }

        return gridList;
    }

}
