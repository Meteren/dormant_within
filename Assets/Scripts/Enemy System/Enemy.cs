using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int healthAmount;
    public virtual void OnDamage(int damage)
    {
        healthAmount -= damage;
        Debug.Log($"Amount of damage inflicted: {damage} - Remained health: {healthAmount}");

        if(healthAmount <= 0)
            Destroy(gameObject);
    }
}
