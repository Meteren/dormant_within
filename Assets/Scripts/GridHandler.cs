using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridHandler : MonoBehaviour
{
    List<PathGrid> gridsCollided = new List<PathGrid>();
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<PathGrid>(out PathGrid grid))
        {
            gridsCollided.Add(grid);    
        }
    }

    private void OnDisable()
    {
        GetComponent<Collider>().isTrigger = true;
        Debug.Log($"GridHandler grids: {gridsCollided.Count}");
        foreach(var grid in gridsCollided)
            grid.isMovable = true;
    }


}
