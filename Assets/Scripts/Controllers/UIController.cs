using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : Singleton<UIController>
{
    public GameObject itemCollectedPanel;

    private void Update()
    {
        if (GetInstance != null)
            Debug.Log("not Null");
    }
}
