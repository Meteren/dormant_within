using TMPro;
using UnityEngine;

public class GridMenu : MonoBehaviour
{
    public RectTransform rectTransform;
    [Header("Item")]
    public ItemRepresenter representer;
    [Header("Inspector Scene Controller")]
    public InspectorSceneController inspectorSceneController;

    public PlayerController controller => GameManager.instance.blackboard.TryGetValue("PlayerController", 
        out PlayerController _controller) ? _controller : null;

    public void InitGridMenu(ItemRepresenter representer)
    {
        gameObject.SetActive(true);
        Vector2 center = representer.rectTransform.position;
        rectTransform.position = new Vector2(center.x + rectTransform.rect.width / 2, center.y - rectTransform.rect.height / 2);
        this.representer = representer;
    }

    public void OnPressEquipButton()
    {
        Debug.Log("Item Equipped");
    }
    public void OnPressUseButton()
    {
        if (controller != null)
            if (controller.interactedPuzzleObject != null)
                controller.interactedPuzzleObject.ApplyPuzzleLogic(representer);
            else
                UIManager.instance.HandleIndicator("Can't use this item here.",2f);
    }

    public void OnPressInspectButton()
    {
        Item gameObjectToBeInspected = inspectorSceneController.objectToBeInspected;
        if (gameObjectToBeInspected != null)
            if(gameObjectToBeInspected.gameObject.activeSelf)
                gameObjectToBeInspected.gameObject.SetActive(false);
        Item item = representer.representedItem;
        Debug.Log("Inspect button pressed");
        item.transform.SetParent(inspectorSceneController.inspectorCam.transform);
        item.transform.localPosition = new Vector3(0, 0, item.distanceFromCam);
        item.transform.localRotation = Quaternion.identity;
        item.rb.velocity = Vector3.zero;
        inspectorSceneController.objectToBeInspected = item;
        inspectorSceneController.CleareInspectionText();
        item.gameObject.SetActive(true);
        inspectorSceneController.inspectorCam.fieldOfView = item.cameraFOVLookingAtObject;
        gameObject.SetActive(false);
    }
    public void OnPressDropButton()
    {
        inspectorSceneController.CleareInspectionText();
        inspectorSceneController.objectToBeInspected = null;
        representer.representedItem.OnDrop();
        representer.attachedGrid.DetachRepresenter();
        Destroy(representer.gameObject);
        gameObject.SetActive(false);

    }

    private void OnDisable()
    {
        gameObject.SetActive(false);
    }
}
