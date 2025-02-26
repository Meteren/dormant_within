using TMPro;
using UnityEngine;

public class LockedObject : PuzzleObject, IInteractable
{
    [SerializeField] private string onInteractText;
    private bool isItUnlocked = false;
    public override void ApplyPuzzleLogic(ItemRepresenter representer)
    {
        if (!ContainsItem(representer.representedItem.ToString()))
        {
            UIManager.instance.HandleIndicator("Can't use this item here");
            return;
        }
        UIManager.instance.HandleInventory(false);
        UIManager.instance.HandleIndicator("Object unlocked the door.");
        DeleteRepresenter(representer);
        isItUnlocked = true;
    }

    public void OnInteract()
    {
        if (!isItUnlocked)
            UIManager.instance.HandleIndicator(onInteractText);
        else
        {
            Debug.Log("Door opened and passed to the new area.");
            Destroy(gameObject);
        }
            
    }
}
