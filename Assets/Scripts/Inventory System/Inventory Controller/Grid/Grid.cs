using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    private ItemRepresenter representer;
    private RectTransform rectTransform;
    public ItemRepresenter Representer { get { return representer; } }
    public RectTransform RectTransform { get { return rectTransform; } }
    //public int gridIndex;
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    public void AttachRepresenter(ItemRepresenter itemRepresenter) => this.representer = itemRepresenter;
    public void DetachRepresenter() => representer = null;
}
