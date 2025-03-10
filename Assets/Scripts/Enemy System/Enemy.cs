using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int healthAmount;
    [SerializeField] private PathFinder finder;
    [Header("LOS")]
    [SerializeField] private int radius;

    [Header("Patrol Points")]
    [SerializeField] private Transform destination;

    bool pathInProgress;

    int patrolIndex;

    int pathIndex;

    float speed = 2f;

    List<PathGrid> path;
    private void Start()
    {
        finder = GetComponent<PathFinder>();
        finder.InitPathGridTable(radius);
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
        path = finder.DrawPath(destination.position);
        Debug.Log("Path count:" + path.Count);
        pathInProgress = true;
        foreach (var pathGrid in path)
            Debug.Log("Path name:" + pathGrid.name);
        pathInProgress = true;

    }
}
