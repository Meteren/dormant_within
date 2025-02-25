using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class PuzzleObject : MonoBehaviour
{
    public HashSet<string> requiredItemNames = new HashSet<string>();
    [SerializeField] protected List<Item> requiredItems = new List<Item>();
    protected HashSet<string> inventory => GameManager.instance.blackboard.TryGetValue("PlayerController", 
        out PlayerController controller) ? controller.inventory : null;

    protected GameObject indicator => UIManager.instance.indicatorText;
    private void Start()
    {
        ExtractItems();
    }
    private void ExtractItems()
    {
        foreach (var item in requiredItems)
            requiredItemNames.Add(item.ToString());

    }

    protected bool ContainsItem(string name)
    {
        if (requiredItemNames.Contains(name)) return true;
        else return false;

    }

    protected void DeleteRepresenter(ItemRepresenter representer)
    {
        representer.attachedGrid.DetachRepresenter(); 
        Destroy(representer.gameObject);
    }

 
    public virtual void ApplyPuzzleLogic(ItemRepresenter representer)
    {
        Debug.Log("Don't have any sequence.");
    }

}


public class RequireItemAndSequencePuzzleObject : PuzzleObject
{
    [SerializeField] private PuzzleSequence puzzleSequence;
    public override void ApplyPuzzleLogic(ItemRepresenter representer)
    {
        puzzleSequence.Init();
    }
}


public class RequireSequencePuzzleObject : PuzzleObject
{
    public override void ApplyPuzzleLogic(ItemRepresenter item)
    {

    }
}

public class PuzzleSequence : MonoBehaviour, IInteractable
{
    //jump to this cam to show player the sequence on OnInteract() or init this sequence on Init()
    [SerializeField] private CinemachineVirtualCamera sequenceCam;
    public void Init()
    {
        
    }
    public void OnInteract()
    {
        return;
    }
}
