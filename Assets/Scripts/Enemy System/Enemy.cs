using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    PlayerController playerConyroller => 
        GameManager.instance.blackboard.TryGetValue("PlayerController", out PlayerController _controller) ? _controller : null;
    [SerializeField] private int healthAmount;
    [SerializeField] private PathFinder pathFinder;

    [Header("LOS")]
    [SerializeField] private int radius;

    [Header("Patrol Points")]
    [SerializeField] private List<Transform> patrolPoints;

    bool pathInProgress;

    int patrolIndex;

    int pathIndex;

    float speed = 3.5f;

    List<PathGrid> path;

    [Header("Conditions")]
    public bool isDead;
    private void Start()
    {
        pathFinder = GetComponent<PathFinder>();
        pathFinder.InitPathGridTable(radius);
        StartCoroutine(St());
    }

    /*private void Update()
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
                    path = pathFinder.DrawPath(transform.position, patrolPoints[patrolIndex].position);
                    pathIndex = 0;
                }
                List<PathGrid> newPath = pathFinder.DrawPath(transform.position, patrolPoints[patrolIndex].position);
                if (ShouldPathChange(newPath))
                {
                    path = newPath;
                    pathIndex = 0;
                }
              
            }

        }      
         
    }*/

    private void FixedUpdate()
    {

        if (pathInProgress)
        {
            transform.position = Vector3.MoveTowards(transform.position, path[pathIndex].transform.position, Time.deltaTime * speed);

            if (Vector3.Distance(transform.position, path[0].transform.position) <= 0.05f)
            {
                if (pathIndex < path.Count - 1)
                {

                }     
                else
                {
                    if (patrolIndex < patrolPoints.Count - 1)
                        patrolIndex++;
                    else
                        patrolIndex = 0;
                }
                List<PathGrid> newPath = pathFinder.DrawPath(transform.position, patrolPoints[patrolIndex].position);
                if (ShouldPathChange(newPath))
                {
                    path = newPath;
                    Debug.Log("Path changed");
                }

            }

        }
    }
    public virtual void OnDamage(int damage)
    {
        healthAmount -= damage;
        Debug.Log($"Amount of damage inflicted: {damage} - Remained health: {healthAmount}");

        if(healthAmount <= 0)
        {
            isDead = true;
            if(playerConyroller.enemiesInRange.Contains(this))
                playerConyroller.enemiesInRange.Remove(this);   
            Destroy(gameObject);
        }
            
    }

    private IEnumerator St()
    {
        yield return new WaitForSeconds(7f);
        //Debug.Log("Path initted");
        path = pathFinder.DrawPath(transform.position,patrolPoints[patrolIndex].position);
        Debug.Log("Path count:" + path.Count);
        pathInProgress = true;
        foreach (var pathGrid in path)
            Debug.Log("Path name:" + pathGrid.name);

    }

    private bool ShouldPathChange(List<PathGrid> newPath)
    {
        if (newPath == null || newPath.Count == 0 || path == null || path.Count == 0)
            return false;

        return newPath[0] != path[0];
    }

    public float CalculatePriority(PlayerController p_controller) =>
        Vector3.Distance(p_controller.transform.position, transform.position);
}
