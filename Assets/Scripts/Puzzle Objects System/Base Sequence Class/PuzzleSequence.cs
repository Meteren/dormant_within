using Cinemachine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PuzzleSequence : MonoBehaviour, IInteractable
{
    //jump to this cam to show player the sequence on OnInteract() or init this sequence on Init()
    [Header("Camera")]
    [SerializeField] protected CinemachineVirtualCamera sequenceCam;
    [SerializeField] protected LayerMask sequenceCameraLayer;
    [SerializeField] protected LayerMask originalCameraLayer;
    protected List<object> itemEntries;
    protected List<KeyItem> requiredItems;
    protected bool sequenceCanBeActivated = false;
    protected bool onInpsect = false;
    public virtual void TryInit(List<object> itemEntries, List<KeyItem> requiredItems)
    {
        return;
    }
    
    public virtual void OnInteract()
    {
        return;
    } 
    public ItemEntry<KeyItem> GetKeyItemAs<T>() where T : KeyItem
    {
        var selectedEntry = 
            itemEntries.Where(x => x is ItemEntry<KeyItem> entry && 
            entry.ValType == typeof(T)).FirstOrDefault() as ItemEntry<KeyItem>; 

        return selectedEntry;
    }

    public ItemEntry<KeyItem> GetKeyItemAs<T>(int index) where T : KeyItem
    {
        ItemEntry<KeyItem> selectedEntry = itemEntries[index] is ItemEntry<KeyItem> entry 
            && entry.ValType == typeof(T) ? entry : null;
        return selectedEntry;
    }
    public virtual void AssignItems()
    {
        return;
    }

    protected void InitItems(List<object> itemEntries, List<KeyItem> requiredItems)
    {
        this.itemEntries = itemEntries;
        this.requiredItems = requiredItems;
    }

    protected bool CanBeInit()
    {
        if(itemEntries.Count == requiredItems.Count) return true;
        else return false;
    } 

    protected void AssignCameraLayer()
    {
        //control camera here
    }

    protected void HandleSequenceCamPriority(int priority)
    {
        sequenceCam.Priority = priority;
    }

}
