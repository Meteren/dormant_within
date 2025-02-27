
using UnityEngine;
public class IdleState : BasePLayerState
{

    public IdleState(PlayerController playerController) : base(playerController)
    {
    }

    public override void OnStart()
    {
        base.OnStart();

    }

    public override void OnExit()
    {
        base.OnExit();
        playerController.idle = false;
    }

    public override void Update()
    {

        base.Update();
        Debug.Log("Idle");
        
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift))
            playerController.run = true;
        else if (Input.GetKey(KeyCode.W))
            playerController.walk = true;
        else if(Input.GetKey(KeyCode.S))
            playerController.walkBackwards = true;

    }
}

public class WalkState : BasePLayerState
{

    float walkSpeed = 3f;
    public WalkState(PlayerController playerController) : base(playerController)
    {
    }

  
    public override void OnStart()
    {
        base.OnStart();
    }

    public override void OnExit()
    {
        base.OnExit();
        playerController.walk = false;
    }


    public override void Update()
    {
        base.Update();
        Debug.Log("Walk");
        if(Input.GetKeyUp(KeyCode.W))
            playerController.idle = true;
        if (Input.GetKeyDown(KeyCode.LeftShift))
            playerController.run = true;

        playerController.rb.velocity = new Vector3(playerController.ForwardDirection.x * walkSpeed,
               playerController.rb.velocity.y, playerController.ForwardDirection.z * walkSpeed);
    }
}

public class WalkBackwardsState : BasePLayerState
{
    float backwardsSpeed = 1.5f;
    public WalkBackwardsState(PlayerController playerController) : base(playerController)
    {
    }
    public override void OnStart()
    {
        base.OnStart();
    }
    public override void OnExit()
    {
        base.OnExit();
        playerController.walkBackwards = false;
    }

    public override void Update()
    {
        base.Update();
        Debug.Log("WalkBackwards");
        if (Input.GetKeyUp(KeyCode.S))
            playerController.idle = true;
        playerController.rb.velocity = new Vector3(playerController.ForwardDirection.x * backwardsSpeed * -1,
            playerController.rb.velocity.y, playerController.ForwardDirection.z * backwardsSpeed * -1);
    }
}

public class RunState : BasePLayerState
{
    float runSpeed = 6f;
    public RunState(PlayerController playerController) : base(playerController)
    {
    }
    public override void OnStart()
    {
        base.OnStart();
    }

    public override void OnExit()
    {
        base.OnExit();
        playerController.run = false;   
    }

    public override void Update()
    {
        base.Update();
        Debug.Log("Run");
        if (Input.GetKeyUp(KeyCode.W))
            playerController.idle = true;
        else if(Input.GetKeyUp(KeyCode.LeftShift))
            playerController.walk = true;
        playerController.rb.velocity = new Vector3(playerController.ForwardDirection.x * runSpeed,
              playerController.rb.velocity.y, playerController.ForwardDirection.z * runSpeed);
    }
}

