
using System.Collections;
using UnityEngine;
public class IdleState : BasePlayerState
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

public class WalkState : BasePlayerState
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

public class WalkBackwardsState : BasePlayerState
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

public class RunState : BasePlayerState
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

public class AimState : BasePlayerState
{
    float transitionDuration = 0.2f;
    public AimState(PlayerController playerController) : base(playerController)
    {
    }

    public override void OnStart()
    {
        base.OnStart();
    }

    public override void OnExit()
    {
        base.OnExit();
        playerController.aim = false;
    }

    public override void Update()
    {
        Debug.Log("Aim state");
        if (Input.GetMouseButtonDown(0))
        {
            playerController.shoot = true;
        }

        if (Input.GetMouseButtonUp(1) || !playerController.isPressingM2)
            GameManager.instance.StartCoroutine(WaitTransition());
    }

    private IEnumerator WaitTransition()
    {
        yield return new WaitForSeconds(transitionDuration);
        playerController.idle = true;
    }
}

public class ShootState : BasePlayerState
{
    AnimatorStateInfo stateInfo;
    public ShootState(PlayerController playerController) : base(playerController)
    {
    }
 
    public override void OnStart()
    {
        base.OnStart();
       
    }

    public override void OnExit()
    {
        base.OnExit();
        playerController.shoot = false;
    }

    public override void Update()
    {
        Debug.Log("Shoot State");
        base.Update();
        stateInfo = playerController.anim.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("shooting"))
            if (stateInfo.normalizedTime >= 1)
                playerController.aim = true;

    }
}

