using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : Weapon
{
    public override int InflictDamage()
    {
        Debug.Log("Damage Inflicted");
        return 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        
    }
}
