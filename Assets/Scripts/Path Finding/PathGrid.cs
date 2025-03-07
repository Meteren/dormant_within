using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;

public class PathGrid : MonoBehaviour
{
    public int X {  get; private set; }
    public int Y { get; private set; }

    Transform agentTransform;

    public bool isMovable;

    List<Renderer> objectsOccupyingGrid = new List<Renderer>();

    Vector3 offset;

    private void Update()
    {
        Debug.Log($"{X}-{Y} isMovable: {isMovable}");
        transform.rotation = Quaternion.identity;
        transform.position = agentTransform.position + offset;

        if (objectsOccupyingGrid.Count == 0)
            isMovable = true;
        else
            isMovable = false;
    }

    public void InitPathGrid(Transform agentTransform,BoxCollider boxCollider,Vector3 position, int x, int y)
    {
        this.X = x;
        this.Y = y;
        this.agentTransform = agentTransform;
        transform.position = position;
        offset = transform.position - agentTransform.position;
        gameObject.layer = LayerMask.NameToLayer("PathGrid");
        isMovable = true;
        BoxCollider collider = transform.AddComponent<BoxCollider>();
        collider.isTrigger = true;
        Vector3 collWorldSize = boxCollider.size;
        collWorldSize.Scale(agentTransform.lossyScale);
        collider.size = collWorldSize;
        transform.SetParent(agentTransform);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger enter");
        Debug.Log($"Trigger enter object: {other.name}");
        if (other.TryGetComponent<Renderer>(out Renderer renderer))
            objectsOccupyingGrid.Add(renderer);

            
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Trigger exit");
        Debug.Log($"Trigger exit object: {other.name}");
        if (other.TryGetComponent<Renderer>(out Renderer renderer))
            objectsOccupyingGrid.Remove(renderer);
    }

    public float CalculateDistance(Vector3 positionToMove) => Vector3.Distance(transform.position, positionToMove);

}
