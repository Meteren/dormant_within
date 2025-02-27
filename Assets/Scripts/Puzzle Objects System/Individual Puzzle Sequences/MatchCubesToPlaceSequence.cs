using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MatchCubesToPlaceSequence : PuzzleSequence
{

    int entryIndex = 0;

    [SerializeField] private Cube cubeOne;
    [SerializeField] private Cube cubeTwo;
    [SerializeField] private Cube cubeThree;
    [SerializeField] private Cube cubeFour;

    [Header("Placemenets")]
    [SerializeField] private List<Transform> placements;

    [Header("Text")]
    [SerializeField] private string onNotActivatedToSay;

    private List<Cube> assignedItems = new List<Cube>();

    private void Update()
    {
        if (onInpsect && Input.GetKeyDown(KeyCode.Q))
        {
            SequenceCamHandler(0);
            onInpsect = false;
        }
            
    }
    public override void TryInit(List<object> itemEntries, List<KeyItem> requiredItems)
    {
        KeyItem itemToBePlaced = null;
        InitItems(itemEntries, requiredItems);
        if (itemEntries.Count != entryIndex)
        {
            itemToBePlaced = AssignItemsReturnIfNeeded();
        }   
        if (itemToBePlaced != null)
        {
            StartCoroutine(PlaceItem(placements[entryIndex].transform.position, itemToBePlaced));
            Debug.Log("Initted");
        }
            
        if (CanBeInit())
        {
            Debug.Log("Puzzle Started");
            SequenceCamHandler(2);
            onInpsect = true;
            sequenceCanBeActivated = true;
            //init the puzzle sequence write the logic of this puzzle
        }
    }
    public override void SequenceLogic()
    {
        base.SequenceLogic();
    }

    public override void OnInteract()
    {
        if (!sequenceCanBeActivated)
        {
            onInpsect = true;
            SequenceCamHandler(2);
            UIManager.instance.HandleIndicator(onNotActivatedToSay,2f);
        }   
        else
            TryInit(itemEntries,requiredItems);
    }

    public override KeyItem AssignItemsReturnIfNeeded()
    {
        KeyItem reference;
        if (!assignedItems.Contains(cubeOne))
        {
            cubeOne = GetKeyItemAs<Cube>(entryIndex).Value as Cube;
            assignedItems.Add(cubeOne);
            reference = cubeOne;
        }
        else if (!assignedItems.Contains(cubeTwo))
        {
            cubeTwo = GetKeyItemAs<Cube>(entryIndex).Value as Cube;
            assignedItems.Add(cubeTwo);
            reference = cubeTwo;
        }
        else if (!assignedItems.Contains(cubeThree))
        {
            cubeThree = GetKeyItemAs<Cube>(entryIndex).Value as Cube;
            assignedItems.Add(cubeThree);
            reference = cubeThree;
        }
        else if (!assignedItems.Contains(cubeFour))
        {
            cubeFour = GetKeyItemAs<Cube>(entryIndex).Value as Cube;
            assignedItems.Add(cubeFour);
            reference = cubeFour;
        }
        else
        {
            reference = null;
        }
         
        return reference;

    }

    public IEnumerator PlaceItem(Vector3 position, KeyItem itemToBePlaced)
    {
        
        SequenceCamHandler(2);
        itemToBePlaced.transform.position = position;
        itemToBePlaced.ResetState();
        float y = sequenceCam.transform.eulerAngles.y;
        HandlePuzzleItemPhysics(itemToBePlaced);
        SetItemRotation(itemToBePlaced);
        entryIndex++;
        UIManager.instance.HandleIndicator("Item Placed",1.5f); 
        yield return new WaitForSecondsRealtime(1.5f);
        if(!sequenceCanBeActivated)
            SequenceCamHandler(0);
    }

}
