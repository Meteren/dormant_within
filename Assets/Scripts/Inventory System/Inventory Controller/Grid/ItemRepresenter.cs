using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemRepresenter : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [HideInInspector] public RectTransform rectTransform;
    [SerializeField] private InventoryController inventoryController;
    private Image image; 
    public Item representedItem;
    public InventoryGrid attachedGrid;
    Vector2 size = new Vector2(177, 132);
    Vector3 offset = Vector3.zero;
    float storedFov;
    public Vector3 itemInspectorCamScale;
    Vector3 clipTextOffset = new Vector3(-20, 10, 0);
    TextMeshProUGUI clipText;

    private void Update()
    {
        if(representedItem is Weapon weapon)
            clipText.text = weapon.GetClip().currentAmount.ToString();
    }
    public void InitRepresenter(Item item, InventoryGrid gridToAttach,InventoryController inventoryController,float fov)
    {
        this.storedFov = fov;
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        this.inventoryController = inventoryController;
        rectTransform.sizeDelta = size;
        image.sprite = item.itemImage;
        itemInspectorCamScale = item.transform.localScale;
        representedItem = item;
        attachedGrid = gridToAttach;
        attachedGrid.AttachRepresenter(this);
        transform.SetParent(attachedGrid.transform);
        rectTransform.localPosition = Vector3.zero;
        SetAmmoIndicatorIfNeeded(item,image);

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Clicked");
            Vector2 clickPoint = eventData.position;
            Vector2 currentPoint = transform.position;
            offset = currentPoint - clickPoint;
            transform.SetParent(transform.parent.parent);
        }

        if (Input.GetMouseButtonDown(1))
            inventoryController.gridMenu.InitGridMenu(this);
       
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (Input.GetMouseButton(0))
        {
            rectTransform.position = Input.mousePosition + offset;
        }

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(inventoryController.TryGetGrid(this,out InventoryGrid grid) && grid.Representer == null)
        {
            attachedGrid.DetachRepresenter();
            transform.SetParent(grid.transform);
            grid.AttachRepresenter(this);
            attachedGrid = grid;
        }
        else
        {
            transform.SetParent(attachedGrid.transform);
        }

        rectTransform.localPosition = Vector3.zero;
    }

    private void SetAmmoIndicatorIfNeeded(Item item,Image image)
    {
        if (item.TryGetComponent<Weapon>(out Weapon weapon))
        {
            GameObject clipTextObject = new GameObject($"{item.gameObject.name} Clip");
            clipTextObject.AddComponent<CanvasRenderer>();
            clipTextObject.AddComponent<RectTransform>();
            clipText = clipTextObject.AddComponent<TextMeshProUGUI>();
            clipTextObject.transform.SetParent(transform);
            clipText.alignment = TextAlignmentOptions.Center;
            RectTransform cliptextObjectRect = clipTextObject.GetComponent<RectTransform>();
            cliptextObjectRect.transform.position = new Vector3(image.transform.position.x + size.x / 2, 
                image.transform.position.y - size.y / 2, image.transform.position.z) + clipTextOffset;
            clipText.text = $"{weapon.GetClip().currentAmount}";

        }
    }

}
