using TMPro;
using UnityEngine;
public abstract class Item : MonoBehaviour, ICollectible, IInspectable
{
    [HideInInspector] public float distanceFromCam = 2;

    [SerializeField] protected float scaleOffset;
    [SerializeField] protected Collider coll;
    [SerializeField] protected float rotationSpeed;

    [Header("Conditions")]
    [SerializeField] private bool itemCollected;

    [Header("Texts")]
    [SerializeField] protected string onFoundToSay;
    [SerializeField] protected string onInspectToSay;

    [Header("Item Image")]
    public Sprite itemImage;

    [Header("Inventory Controller")]
    [SerializeField] private InventoryController inventoryController;
    private void Update()
    {
        if (itemCollected)
        {
            transform.Rotate(0, Time.deltaTime * rotationSpeed, 0);
            if (Input.GetKeyDown(KeyCode.Q))
            {
               
                if (inventoryController.TryAttachCollectedItemToGrid(this))
                {
                    //indicate that item is collected via UIManager
                    UIManager.GetInstance.itemCollectedPanel.SetActive(false);
                    gameObject.SetActive(false);
                }
                else
                {

                }
                    
            }
        }
            
    }
    public void OnCollect()
    {
        AttachObjectToCamera();
        ScaleToFitCamera();
        InitRotation();
        HandleItemCheckPanel();
    }
    public void OnInspect(TextMeshProUGUI toSay)
    {
        toSay.text = onInspectToSay;
    }

    private void ScaleToFitCamera()
    {
        Bounds bound = coll.bounds;
       
        Vector3 objectSizes = bound.max - bound.min;

        float objectSize = Mathf.Max(objectSizes.x, objectSizes.y, objectSizes.z);

        float cameraView = Mathf.Tan(0.5f * Mathf.Deg2Rad * Camera.main.fieldOfView);

        float distance = (objectSize * 0.5f) / cameraView;

        float scaleFactor = distanceFromCam / distance;

        transform.localScale *= scaleFactor * scaleOffset;

    }
    private void AttachObjectToCamera()
    {
        transform.parent = Camera.main.transform;
        transform.localPosition = new Vector3(0, 0, distanceFromCam);
        transform.localRotation = Quaternion.identity;
    }

    private void InitRotation() => itemCollected = true;


    public void HandleItemCheckPanel()
    {

        UIManager.GetInstance.itemCollectedPanel.GetComponentInChildren<TextMeshProUGUI>().text = onFoundToSay;
        UIManager.GetInstance.itemCollectedPanel.SetActive(true);
    }
    private void OnValidate()
    {
        scaleOffset = Mathf.Clamp01(scaleOffset);
    }
    public virtual void SpecialBehaviour()
    {
        return;
    }

}
