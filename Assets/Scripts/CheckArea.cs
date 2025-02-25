using UnityEngine;

public class CheckArea : MonoBehaviour
{
    public ICollectible itemToBeCollected;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<ICollectible>(out ICollectible collectible))
            itemToBeCollected = collectible;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<ICollectible>(out ICollectible collectible))
            itemToBeCollected = null;
    }
    private void Update()
    {
        if (itemToBeCollected != null && Input.GetKeyDown(KeyCode.E))
            itemToBeCollected.OnCollect();
    }

}
