using System.Collections.Generic;
using UnityEngine;
public class MatchCubesToPlaceSequence : PuzzleSequence
{

    int entryIndex = 0;
    [SerializeField]private Cube cubeOne;
    [SerializeField]private Cube cubeTwo;
    [SerializeField]private Cube cubeThree;

    [Header("Text")]
    [SerializeField] private string onNotActivatedToSay;

    private List<Cube> assignedItems = new List<Cube>();

    private void Update()
    {
        if (onInpsect && Input.GetKeyDown(KeyCode.Q))
            HandleSequenceCamPriority(0);
    }

    public override void TryInit(List<object> itemEntries, List<KeyItem> requiredItems)
    {
        InitItems(itemEntries,requiredItems);
        AssignItems();
        if (CanBeInit())
        {
            Debug.Log("Puzzle Started");
            HandleSequenceCamPriority(2);
            onInpsect = true;
            sequenceCanBeActivated = true;
            //init the puzzle sequence write the logic of this puzzle
        }
    }

    public override void OnInteract()
    {
        if (!sequenceCanBeActivated)
        {
            onInpsect = true;
            HandleSequenceCamPriority(2);
            UIManager.instance.HandleIndicator(onNotActivatedToSay);
        }   
        else
            TryInit(itemEntries,requiredItems);
    }

    public override void AssignItems()
    {
        if (!assignedItems.Contains(cubeOne))
        {
            cubeOne = GetKeyItemAs<Cube>(entryIndex).Value as Cube;
            assignedItems.Add(cubeOne);
        }
        else if (!assignedItems.Contains(cubeTwo))
        {
            cubeTwo = GetKeyItemAs<Cube>(entryIndex).Value as Cube;
            assignedItems.Add(cubeTwo);
        }
        else if (!assignedItems.Contains(cubeThree))
        {
            cubeThree = GetKeyItemAs<Cube>(entryIndex).Value as Cube;
            assignedItems.Add(cubeThree);
        }
        if (requiredItems.Count == entryIndex + 1)
            return;
        entryIndex++;
        UIManager.instance.HandleIndicator("Item Placed");
    }

}
