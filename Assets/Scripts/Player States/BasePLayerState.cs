
using UnityEngine;

public class BasePLayerState : IState
{
    protected PlayerController playerController;
  
    public BasePLayerState(PlayerController playerController)
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
        return;
    }

   
}
