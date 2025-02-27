
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

    [Header("Conditions")]
    public bool idle = true;
    public bool walk;
    public bool run;
    public bool aim;
    public bool walkBackwards;
    
    [Header("Reference Point")]
    [SerializeField] private Transform reference;
    public Vector3 ForwardDirection {  get; private set; }

    StateMachine playerStateMachine = new StateMachine();
    void Start()
    {
        GameManager.instance.blackboard.SetValue("PlayerController", this);
        PlayerController controller = GameManager.instance.blackboard.TryGetValue("PlayerController",
            out PlayerController _controller) ? _controller : null;
        if (controller == null)
            Debug.Log("Controller null");
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        var idleState = new IdleState(this);
        var walkState = new WalkState(this);
        var runState = new RunState(this);
        var walkBackwardsState = new WalkBackwardsState(this);

        Add(idleState, walkState, new FuncPredicate(() => walk));
        Add(idleState, runState, new FuncPredicate(() => run));
        Add(idleState, walkBackwardsState, new FuncPredicate(() => walkBackwards));
        Add(walkState, idleState, new FuncPredicate(() => idle));
        Add(walkState, runState, new FuncPredicate(() => run));
        Add(runState, walkState, new FuncPredicate(() => walk));
        Add(runState, idleState, new FuncPredicate(() => idle));
        Add(walkBackwardsState, idleState, new FuncPredicate(() => idle));

        playerStateMachine.CurrentState = idleState;
        
    }
    
    private void Add(IState from, IState to, IPredicate condition)
    {
        playerStateMachine.AddNode(from, to, condition);
    }

    private void FixedUpdate()
    {
        SetForwardDirection();
        Rotate();
    }

    void Update()
    {
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


}
