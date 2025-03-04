
using UnityEngine;

public class BasePlayerState : IState
{
    protected PlayerController playerController;

  
    public BasePlayerState(PlayerController playerController)
    {
        this.playerController = playerController;
    }
    public virtual void OnStart()
    {
        playerController.rb.velocity = Vector3.zero;
    }

    public virtual void OnExit()
    {
        return;
    }

    public virtual void Update()
    {
        if (Input.GetMouseButtonDown(1) && playerController.equippedItem != null && !UIManager.instance.inventory.activeSelf)
        {
            playerController.aim = true;
            playerController.isPressingM2 = true;
        }
    }
   
}
