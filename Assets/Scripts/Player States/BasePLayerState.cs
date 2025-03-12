
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BasePlayerState : IState
{
    protected PlayerController playerController;
    protected bool lockOn;
    float rotationSpeed = 7f;

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

    protected void AimAtClosest()
    {
        if(playerController.lockedEnemy.isDead || !playerController.enemiesInRange.Contains(playerController.lockedEnemy))
        {
            playerController.lockedEnemy = GetClosestEnemy();
            lockOn = false;
        }
          
        Weapon weapon = playerController.equippedItem as Weapon;
        Vector3 direction = playerController.lockedEnemy.transform.position - weapon.muzzlePoint.position;

        direction.y = 0f;

        if (direction.magnitude < 1f)
        {
            lockOn = false;
            return;
        }

        Quaternion lookAtDirection = Quaternion.LookRotation(direction);

        float angleDifference = Quaternion.Angle(playerController.transform.rotation, lookAtDirection);

        if (!lockOn)
        {
            Debug.Log("Lockin on");
            playerController.transform.rotation =
               Quaternion.Slerp(playerController.transform.rotation, lookAtDirection, Time.deltaTime * rotationSpeed);
            if(angleDifference <= 2f)
                lockOn = true;
        }
        else
        {
            playerController.transform.rotation =
               lookAtDirection;
            Debug.Log("Locked on");
        }
           
    }
    protected Enemy GetClosestEnemy()
    {
        Enemy closestEnemy = playerController.enemiesInRange.Aggregate((closest, next)
            => next.CalculatePriority(playerController) < closest.CalculatePriority(playerController) ? next : closest);
        return closestEnemy;
    }

}
