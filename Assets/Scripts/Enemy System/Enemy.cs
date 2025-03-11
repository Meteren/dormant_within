using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int healthAmount;
    [SerializeField] private PathFinder pathFinder;
    [Header("LOS")]
    [SerializeField] private int radius;

    [Header("Patrol Points")]
    [SerializeField] private List<Transform> patrolPoints;

    bool pathInProgress;

    int patrolIndex;

    int pathIndex;

    float speed = 5f;

    List<PathGrid> path;
    private void Start()
    {
        pathFinder = GetComponent<PathFinder>();
        pathFinder.InitPathGridTable(radius);
        StartCoroutine(St());
    }

    private void Update()
    {
        if (pathInProgress)
        {
            transform.position = Vector3.MoveTowards(transform.position, path[pathIndex].transform.position, Time.deltaTime * speed);

            if (Vector3.Distance(transform.position, path[pathIndex].transform.position) <= 0.05f)
            {
                if (pathIndex < path.Count - 1)
                    pathIndex++;
                else
                {
                    if (patrolIndex < patrolPoints.Count - 1)
                        patrolIndex++;
                    else
                        patrolIndex = 0;
                    PathGrid startGrid = path[path.Count - 1];
                    path = pathFinder.DrawPath(startGrid, patrolPoints[patrolIndex].position);
                    pathIndex = 0;
                }
            }

        }         
         
    }
    public virtual void OnDamage(int damage)
    {
        healthAmount -= damage;
        Debug.Log($"Amount of damage inflicted: {damage} - Remained health: {healthAmount}");

        if(healthAmount <= 0)
            Destroy(gameObject);
    }

    private IEnumerator St()
    {
        yield return new WaitForSeconds(7f);
        Debug.Log("Path initted");
        path = pathFinder.DrawPath(pathFinder.centerGrid,patrolPoints[patrolIndex].position);
        Debug.Log("Path count:" + path.Count);
        pathInProgress = true;
        foreach (var pathGrid in path)
            Debug.Log("Path name:" + pathGrid.name);
        pathInProgress = true;

    }
}
