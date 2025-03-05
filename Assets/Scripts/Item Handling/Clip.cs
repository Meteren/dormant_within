using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clip : Item
{
    [Header("Clip Amount")]
    public int currentAmount;
    [SerializeField] private int maxAmount;

    [Header("Conditions")]
    public bool isAttached;
    public bool isEmpty;

    private new void Update()
    {    
        if (isAttached)
            GetComponent<Collider>().enabled = false;
        else
            base.Update();
    }

    public void IncreaseAmount(int increaseValue)
    {
        if(currentAmount == maxAmount)
        {
            //indicate that no increment will be taking place
        }
        currentAmount += increaseValue;
        if(currentAmount > maxAmount)
            currentAmount = maxAmount;

    }
    public void DecreaseAmount(int decreaseValue)
    {
        currentAmount -= decreaseValue;
        if(currentAmount <= 0)
        {
            currentAmount = 0;
            isEmpty = true;
        }
            
    }
}
