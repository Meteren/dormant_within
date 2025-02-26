using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    public GameObject itemCollectedPanel;
    public GameObject inventory;
    public GameObject indicatorText;

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
    private IEnumerator IndicatorCoroutine(string text)
    {
        indicatorText.SetActive(true);
        indicatorText.GetComponent<TextMeshProUGUI>().text = text;
        yield return new WaitForSeconds(2f);
        indicatorText.SetActive(false);
    }

    public void HandleIndicator(string text)
    {
        StartCoroutine(IndicatorCoroutine(text));
    }

    public void HandleInventory(bool activate)
    {
        inventoryActive = activate;
        inventory.SetActive(inventoryActive);
    }
}
