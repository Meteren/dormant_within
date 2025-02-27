using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Cube : KeyItem
{
    public TextMeshProUGUI itemNumber;

    private new void Start()
    {
        base.Start();
        itemNumber = GetComponentInChildren<TextMeshProUGUI>();
    }

}
