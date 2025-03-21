using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy
{
    //later will be changed with spesific enemy types
    protected float rotationSpeed = 300f;
    protected Enemy enemy;
    protected List<PathGrid> path;
    protected bool initPlayerPath;
    protected PlayerController playerController => 
        GameManager.instance.blackboard.TryGetValue("PlayerController", 
            out PlayerController _playerController) ? _playerController : null;

    public BaseEnemy(Enemy enemy)
    {
        this.enemy = enemy; 
    }

    public bool TryPointAt(Vector3 toPoint)
    {
        Vector3 direction = toPoint - enemy.transform.position;
        direction.y = 0;
        Quaternion lookAt = Quaternion.LookRotation(direction);
        if (Quaternion.Angle(enemy.transform.rotation, lookAt) <= 1f)
        {

            enemy.transform.rotation = lookAt;
            return true;
        }      
        else
        {
            enemy.transform.rotation = Quaternion.RotateTowards(enemy.transform.rotation, lookAt, Time.deltaTime * rotationSpeed);
            return true;
        }
    }
}

public class PatrolStrategy : BaseEnemy, IStrategy
{    
    bool initPathConstruct;
    float patrolSpeed = 1f;
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
            enemy.walk = true;
            enemy.transform.position = 
                Vector3.MoveTowards(enemy.transform.position, path[0].transform.position, Time.deltaTime * patrolSpeed);

            TryPointAt(enemy.patrolPoints[patrolIndex].transform.position);

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
        enemy.walk = false;
        enemy.run = false;
        enemy.idle = true;
        yield return new WaitForSeconds(1f);
        path = enemy.pathFinder.DrawPath(enemy.transform.position, enemy.patrolPoints[0].position);
        initPathConstruct = true;
    }
}


public class ChaseStrategy : BaseEnemy, IStrategy
{

    float chaseSpeedInCloseRange = 1f;
    float chaseSpeedInLongRange = 2.7f;
    float distance = 5f;

    public ChaseStrategy(Enemy enemy) : base(enemy)
    {
    }
    public Node.NodeStatus Evaluate()
    {
        Debug.Log("ChaseStrategy");
        if (!initPlayerPath)
        {
            path = enemy.pathFinder.DrawPath(enemy.transform.position, playerController.transform.position);
            
            initPlayerPath = true;
        }
        if (!enemy.CanChase())
        {
            initPlayerPath = false;
            enemy.lastSeenPos = path[path.Count - 1].transform.position;
            Debug.Log($"Last seen pos: Y:{path[path.Count - 1].Y} - X:{path[path.Count - 1].X}");
            return Node.NodeStatus.SUCCESS;    
        }

        TryPointAt(playerController.centerPoint.transform.position);

        if (initPlayerPath)
        {
            if (Vector3.Distance(enemy.centerPoint.transform.position, playerController.centerPoint.transform.position) <= distance)
            {
                enemy.walk = true;
                enemy.run = false;
            }
            else
            {
                enemy.walk = false;
                enemy.run = true;
            }
                
            enemy.transform.position =
              Vector3.MoveTowards(enemy.transform.position, path[0].transform.position, Time.deltaTime * (enemy.run ? chaseSpeedInLongRange : chaseSpeedInCloseRange));
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

public class MoveToLastSeenPositionStrategy : BaseEnemy, IStrategy
{
    float moveSpeed = 1.7f;
    public MoveToLastSeenPositionStrategy(Enemy enemy) : base(enemy)
    {
    }

    public Node.NodeStatus Evaluate()
    {
        Debug.Log("MoveToLastSeen Strategy");

        if (!initPlayerPath)
        {
            path = enemy.pathFinder.DrawPath(enemy.transform.position, enemy.lastSeenPos);
            foreach (var pathgrid in path)
            {
                Debug.Log($"Y:{pathgrid.Y}--X:{pathgrid.X}");
            }
            initPlayerPath = true;
        }
        
        if (enemy.CanChase())
        {
            initPlayerPath = false;
            return Node.NodeStatus.SUCCESS;
        }

        TryPointAt(enemy.lastSeenPos);

        enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, path[0].transform.position, Time.deltaTime * moveSpeed);

        if (Vector3.Distance(enemy.transform.position, path[0].transform.position) <= 0.05f)
        {
            path = enemy.pathFinder.DrawPath(enemy.transform.position,enemy.lastSeenPos);
        }

        if (Vector3.Distance(enemy.transform.position, enemy.lastSeenPos) <= 0.05f)
        {
            initPlayerPath = false;
            return Node.NodeStatus.SUCCESS;
        }

        return Node.NodeStatus.RUNNING;
        
    }
}

public class AttackStrategy : BaseEnemy, IStrategy
{
    public AttackStrategy(Enemy enemy) : base(enemy)
    {
    }

    public Node.NodeStatus Evaluate()
    {
        throw new System.NotImplementedException();
    }
}
