
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private enum RotationDirection
    {
        left, right,none
    }
    RotationDirection direction = RotationDirection.none;
    public Animator anim;
    public Rigidbody rb;
    private float turnSpeed = 100;

    public HashSet<string> inventory = new HashSet<string>();

    public PuzzleObject interactedPuzzleObject;

    IState baseState;

    [Header("Conditions")]
    public bool idle = true;
    public bool walk;
    public bool run;
    public bool aim;
    public bool walkBackwards;
    public bool shoot;
    public bool isPressingM2;

    [Header("Reference Point")]
    [SerializeField] private Transform reference;

    [Header("Equipped Item")]
    public IEquippable equippedItem;
    public Vector3 ForwardDirection {  get; private set; }

    StateMachine playerStateMachine = new StateMachine();
    void Start()
    {
        GameManager.instance.blackboard.SetValue("PlayerController", this);

        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        var idleState = new IdleState(this);
        var walkState = new WalkState(this);
        var runState = new RunState(this);
        var walkBackwardsState = new WalkBackwardsState(this);
        var aimState = new AimState(this);
        var shootState = new ShootState(this);

        baseState = idleState;

        Add(idleState, walkState, new FuncPredicate(() => walk));
        Add(idleState, runState, new FuncPredicate(() => run));
        Add(idleState, walkBackwardsState, new FuncPredicate(() => walkBackwards));
        Add(walkState, idleState, new FuncPredicate(() => idle));
        Add(walkState, runState, new FuncPredicate(() => run));
        Add(runState, walkState, new FuncPredicate(() => walk));
        Add(runState, idleState, new FuncPredicate(() => idle));
        Add(walkBackwardsState, idleState, new FuncPredicate(() => idle));

        Any(aimState, new FuncPredicate(() => aim));

        Add(aimState, shootState, new FuncPredicate(() => shoot));
        Add(aimState, idleState, new FuncPredicate(() => idle));
        Add(shootState, aimState, new FuncPredicate(() => aim));

        playerStateMachine.CurrentState = idleState;
        
    }
    
    private void Add(IState from, IState to, IPredicate condition)
    {
        playerStateMachine.Add(from, to, condition);
    }

    private void Any(IState to, IPredicate condition)
    {
        playerStateMachine.Any(to, condition);
    }

    private void FixedUpdate()
    {
        SetForwardDirection();
        Rotate();
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(1))
            isPressingM2 = false;
        SetRotationDirection();
        UpdateAnimations();
        playerStateMachine.Update();
    }

    private void SetRotationDirection()
    {
        if (Input.GetKey(KeyCode.D))
            direction = RotationDirection.right;
        else if (Input.GetKey(KeyCode.A))
            direction = RotationDirection.left;
        else
            direction = RotationDirection.none;
    }

    private void UpdateAnimations()
    {
        anim.SetBool("idle", idle);
        anim.SetBool("walk", walk);
        anim.SetBool("walkBackwards", walkBackwards);
        anim.SetBool("run", run);
        anim.SetBool("aim", aim);
        anim.SetBool("shoot", shoot);
    }

    public void SetForwardDirection()
    {
        ForwardDirection = reference.position - transform.position;
    }
    private void Rotate()
    {
        Turn(direction);
    }

    private void Turn(RotationDirection direction)
    {
        Vector3 rotation = transform.eulerAngles;
        if (direction == RotationDirection.left)
        {
            Debug.Log("Left");
            rotation.y -= turnSpeed * Time.deltaTime;
        }
        else if (direction == RotationDirection.right)
        {
            Debug.Log("Right");
            rotation.y += turnSpeed * Time.deltaTime;
        }
        else
        {
            Debug.Log("Stay");
            return;
        }
        transform.rotation = Quaternion.Euler(rotation);

    }

    private void ResetState()
    {
        idle = true;
        walkBackwards = aim = run = walk = false;
        playerStateMachine.CurrentState = baseState;
    }

    private void OnDisable()
    {
        ResetState();
    }


}
