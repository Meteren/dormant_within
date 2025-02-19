using UnityEngine;

public class CheckArea : MonoBehaviour
{
    public ICollectible itemCollected;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<ICollectible>(out ICollectible collectible))
            itemCollected = collectible;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<ICollectible>(out ICollectible collectible))
            itemCollected = null;
    }
    private void Update()
    {
        if (itemCollected != null && Input.GetKeyDown(KeyCode.E))
            itemCollected.OnCollect();
    }

}
