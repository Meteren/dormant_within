using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nose : MonoBehaviour,IInteractable
{
    public void OnOnteract()
    {
        Debug.Log("This is a nose.");
    }
}
