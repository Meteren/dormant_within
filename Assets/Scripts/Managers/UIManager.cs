using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    public GameObject itemCollectedPanel;
    public GameObject inventory;

    [Header("Conditions")]
    [SerializeField] private bool inventoryActive;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            inventoryActive = !inventoryActive;
            inventory.SetActive(inventoryActive);    
        }
          
    }
}
