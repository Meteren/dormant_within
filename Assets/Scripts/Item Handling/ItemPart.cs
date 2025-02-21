using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class ItemPart : MonoBehaviour, IInspectable
{
    [SerializeField] protected string toSayOnInspect;

    public void OnInspect(TextMeshProUGUI toSay)
    {
        toSay.text = toSayOnInspect;
    }

    public virtual void OnInspectToDo()
    {
        return;
    }
}
