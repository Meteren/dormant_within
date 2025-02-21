using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private List<Grid> inventoryGrids;

    public bool TryGetGrid(ItemRepresenter representer, out Grid gridToGet)
    {
        Vector2 representerScreenPoint = representer.rectTransform.position;
        foreach(var grid in inventoryGrids)
        {
            RectTransform gridRect = grid.GetComponent<RectTransform>();
            if(RectTransformUtility.ScreenPointToLocalPointInRectangle(gridRect, representerScreenPoint, null, out Vector2 localPoint) 
                && gridRect.rect.Contains(localPoint))
            {
                gridToGet = grid;
                return true ;
            }
            
        }
        gridToGet = default;

        return false;
    }

    public bool TryAttachCollectedItemToGrid(Item item)
    {
        if(TryGetEmptyGrid(out Grid grid))
        {
            GameObject itemRepresenter = new GameObject(item.name);
            itemRepresenter.AddComponent<CanvasRenderer>();
            itemRepresenter.AddComponent<RectTransform>();
            itemRepresenter.AddComponent<Image>();
            ItemRepresenter representer = itemRepresenter.AddComponent<ItemRepresenter>();
            representer.InitRepresenter(item,grid,this);
            return true;
        }
        else
        {
            //show a message to indicate that inventory is full
            return false;
        }
        
    }

    public bool TryGetEmptyGrid(out Grid emptyGrid)
    {
        foreach(var grid in inventoryGrids)
        {
            if(grid.Representer == null)
            {
                emptyGrid = grid;
                return true;
            }
                
        }
        emptyGrid = default;
        return false;
    }
}
