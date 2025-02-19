using TMPro;
using UnityEngine;

public abstract class Item : MonoBehaviour, ICollectible
{
    protected float distanceFromCam = 2;
    [SerializeField] protected string onFoundToSay;
    [SerializeField] protected float scaleOffset;
    [SerializeField] protected Collider coll;
    [SerializeField] protected float rotationSpeed;
    [Header("Conditions")]
    [SerializeField] private bool itemCollected;
    private void Update()
    {
        if (itemCollected)
        {
            transform.Rotate(0, Time.deltaTime * rotationSpeed, 0);
            if (Input.GetKeyDown(KeyCode.V))
            {
                //inventory handler will take place later instead
                //for now set inactive
                UIController.GetInstance.itemCollectedPanel.SetActive(false);
                Destroy(gameObject);
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

  
    private void ScaleToFitCamera()
    {
        Bounds bound = coll.bounds;
       
        Vector3 objectSizes = bound.max - bound.min;

        float objectSize = Mathf.Max(objectSizes.x, objectSizes.y, objectSizes.z);

        Debug.Log(objectSizes);

        float cameraView = 2.0f * Mathf.Tan(0.5f * Mathf.Deg2Rad * Camera.main.fieldOfView);

        float distance = objectSize / cameraView;

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

        UIController.GetInstance.itemCollectedPanel.GetComponentInChildren<TextMeshProUGUI>().text = onFoundToSay;
        UIController.GetInstance.itemCollectedPanel.SetActive(true);
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
