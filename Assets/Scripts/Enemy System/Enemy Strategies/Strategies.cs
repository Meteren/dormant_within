using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy
{
    //later will be changed with spesific enemy types
    protected Enemy enemy;
    protected List<PathGrid> path;
    protected PlayerController playerController => 
        GameManager.instance.blackboard.TryGetValue("PlayerController", 
            out PlayerController _playerController) ? _playerController : null;

    public BaseEnemy(Enemy enemy)
    {
        this.enemy = enemy; 
    }
}

public class PatrolStrategy : BaseEnemy, IStrategy
{    
    bool initPathConstruct;
    float patrolSpeed = 1.5f;
    int patrolIndex;

    bool coroutineStarted;
    public PatrolStrategy(Enemy enemy) : base(enemy)
    {
    }
    public Node.NodeStatus Evaluate()
    {
        Debug.Log("PatrolStrategy");
        if (!coroutineStarted)
        {
            coroutineStarted = true;
            enemy.StartCoroutine(InitConstruction());
        } 
            
        if (enemy.CanChase())
        {
            initPathConstruct = false;
            coroutineStarted = false;
            return Node.NodeStatus.SUCCESS;
        }

        if (initPathConstruct)
        {
            enemy.transform.position = 
                Vector3.MoveTowards(enemy.transform.position, path[0].transform.position, Time.deltaTime * patrolSpeed);

            if (Vector3.Distance(enemy.transform.position, path[0].transform.position) <= 0.05f)
            {
                if (0 >= path.Count - 1)
                {
                    if (patrolIndex < enemy.patrolPoints.Count - 1)
                        patrolIndex++;
                    else
                        patrolIndex = 0;
                }
                List<PathGrid> newPath = enemy.pathFinder.DrawPath(enemy.transform.position, enemy.patrolPoints[patrolIndex].position);
                path = newPath;
                Debug.Log("Path changed");

            }
           
        }
        return Node.NodeStatus.RUNNING;

    }
    private IEnumerator InitConstruction()
    {
        yield return new WaitForSeconds(1f);
        path = enemy.pathFinder.DrawPath(enemy.transform.position, enemy.patrolPoints[0].position);
        initPathConstruct = true;
    }
}


public class ChaseStrategy : BaseEnemy, IStrategy
{

    float chaseSpeed = 1.7f;
    bool initPlayerPath;
    public ChaseStrategy(Enemy enemy) : base(enemy)
    {
    }
    public Node.NodeStatus Evaluate()
    {
        Debug.Log("ChseStrategy");
        if (!initPlayerPath)
        {
            path = enemy.pathFinder.DrawPath(enemy.transform.position, playerController.transform.position);
            initPlayerPath = true;
        }
        if (!enemy.CanChase())
        {
            initPlayerPath = false;
            return Node.NodeStatus.FAILURE;    
        }
            
        if (initPlayerPath)
        {
            enemy.transform.position =
              Vector3.MoveTowards(enemy.transform.position, path[0].transform.position, Time.deltaTime * chaseSpeed);
            if (Vector3.Distance(enemy.transform.position, path[0].transform.position) <= 0.05f)
            {
                List<PathGrid> newPath = enemy.pathFinder.DrawPath(enemy.transform.position, playerController.transform.position);
                path = newPath;
                Debug.Log("Path changed");
            }
        }
       
        return Node.NodeStatus.RUNNING;
    }


}
