using UnityEngine;

public class CheckArea : MonoBehaviour
{
    private ICollectible itemToBeCollected;
    private IInteractable interactedObject;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<ICollectible>(out ICollectible collectible))
            itemToBeCollected = collectible;
        if(other.TryGetComponent<PuzzleObject>(out PuzzleObject puzzleObject))
            GetComponentInParent<PlayerController>().interactedPuzzleObject = puzzleObject;
        if(other.TryGetComponent<IInteractable>(out IInteractable interactable))
            interactedObject = interactable;

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<ICollectible>(out ICollectible collectible))
            itemToBeCollected = null;
        if (other.TryGetComponent<PuzzleObject>(out PuzzleObject puzzleObject))
            GetComponentInParent<PlayerController>().interactedPuzzleObject = null;
        if (other.TryGetComponent<IInteractable>(out IInteractable interactable))
            interactedObject = null;
    }
    private void Update()
    {
        if (itemToBeCollected != null && Input.GetKeyDown(KeyCode.E))
            itemToBeCollected.OnCollect(GetComponentInParent<PlayerController>().inventory);
        if (interactedObject != null && Input.GetKeyDown(KeyCode.E))
            interactedObject.OnInteract();

    }
}
