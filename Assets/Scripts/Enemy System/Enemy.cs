using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    PlayerController playerConyroller => 
        GameManager.instance.blackboard.TryGetValue("PlayerController", out PlayerController _controller) ? _controller : null;
    [SerializeField] private int healthAmount;

    [Header("PathFinder")]
    public PathFinder pathFinder;

    [Header("LOS")]
    [SerializeField] private int gridRadius;
    [SerializeField] private float checkAreaRadius;

    [Header("Patrol Points")]
    public List<Transform> patrolPoints;

    [Header("Player Mask")]
    [SerializeField] private LayerMask playerMask;
    [Header("Ray Mask")]
    [SerializeField] private LayerMask rayMask;

    [Header("Center")]
    public Transform centerPoint;

    bool pathInProgress;

    int patrolIndex;

    int pathIndex;

    float speed = 3.5f;

    List<PathGrid> path;
    BehaviourTree enemyBehaviourTree;
    float yOffset = 5f;

    [Header("Conditions")]
    public bool walk;
    public bool run;
    public bool idle;
    public bool isDead;

    [Header("Last Seen Position")]
    public Vector3 lastSeenPos;

    protected Animator enemyAnimator;
    private void Start()
    {
        enemyAnimator = GetComponent<Animator>();
        playerMask = LayerMask.GetMask("Player");
        pathFinder = GetComponent<PathFinder>();
        pathFinder.InitPathGridTable(gridRadius);

        enemyBehaviourTree = new BehaviourTree();

        SortedSelectorNode mainSelector = new SortedSelectorNode("MainSelector");
        enemyBehaviourTree.AddChild(mainSelector);

        var patrolStrategy = new Leaf("PatrolStrategy", new PatrolStrategy(this),20);
        

        SequenceNode chaseSequence = new SequenceNode("ChaseSequnce", 10);

        var chaseCondition = new Leaf("ChaseCondition", new Condition(() => CanChase()));
        var chaseStrategy = new Leaf("ChaseStrategy", new ChaseStrategy(this),10);
        var moveToLastSeenPosStrategy = new Leaf("MoveToLastSeenPos", new MoveToLastSeenPositionStrategy(this));

        chaseSequence.AddChild(chaseCondition);
        chaseSequence.AddChild(chaseStrategy);
        chaseSequence.AddChild(moveToLastSeenPosStrategy);

        mainSelector.AddChild(chaseSequence);
        mainSelector.AddChild(patrolStrategy);

        //StartCoroutine(St());
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

    private void Update()
    {
        SetAnimations();
    }

    private void FixedUpdate()
    {
        enemyBehaviourTree.Evaluate();
        /*if (pathInProgress)
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

        }*/
    }

    protected void SetAnimations()
    {
        enemyAnimator.SetBool("idle",idle);
        enemyAnimator.SetBool("walk", walk);
        enemyAnimator.SetBool("run", run);
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

    public bool CanChase()
    {
        bool playerDetected = Physics.CheckSphere(transform.position, checkAreaRadius, playerMask);
        if (playerDetected)
        {
            Debug.Log("Collided with player");
            if (IsInLineOfSight())
                return true;
        }
           
        return false;
            
    }

    private bool IsInLineOfSight()
    {
        Vector3 rayDirection = playerConyroller.centerPoint.position - transform.position;
        Ray ray = new Ray(transform.position, rayDirection);
        if (Physics.Raycast(ray, out RaycastHit hit, checkAreaRadius,rayMask,QueryTriggerInteraction.Ignore))
        {
            Debug.DrawRay(transform.position, rayDirection * checkAreaRadius,Color.red);
            if (hit.transform.GetComponent<PlayerController>() != null)
            {
                Debug.Log("Player is in los");
                return true;
            }
                
        }
           
        return false;

    }

    private IEnumerator St()
    {
        yield return new WaitForSeconds(7f);
        //Debug.Log("Path initted");
        path = pathFinder.DrawPath(transform.position,patrolPoints[patrolIndex].position);
        //Debug.Log("Path count:" + path.Count);
        pathInProgress = true;
        /*foreach (var pathGrid in path)
            Debug.Log("Path name:" + pathGrid.name);*/

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
