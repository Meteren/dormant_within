using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageArea : MonoBehaviour
{
    [SerializeField] private int damageAmount;
    private void OnTriggerEnter(Collider other)
    {
        if(GetComponentInParent<PlayerController>() == null)
        {
            if (other.TryGetComponent<PlayerController>(out PlayerController playerController))
                playerController.OnTakeDamage(damageAmount);
        }
        else
        {
            if (other.TryGetComponent<Enemy>(out Enemy enemy))
                enemy.OnDamage(damageAmount);
                                
        }
          
    }
}
