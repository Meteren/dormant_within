using UnityEngine;
using UnityEngine.UI;

public class InspectorScene : MonoBehaviour
{
    public Camera inspectorCam;
    public Transform objectToBeInspected;
    private Vector3 lastMousePosition;
    private RawImage rawImage;
    [SerializeField] private float rotationSpeed;
    [Header("Conditions")]
    [SerializeField] private bool isPressed;

    private void Start()
    {
        rawImage = GetComponent<RawImage>();
    }

    private void Update()
    {
        if (CastRayAndCheckIfInBoundaries(out Ray ray))
        {

            if (Input.GetMouseButtonDown(0))
            {
                isPressed = true;
                lastMousePosition = Input.mousePosition;
                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Collide))
                {
                    Debug.Log($"Hitted object: {hit.transform.name}");
                    if (hit.transform.TryGetComponent<IInteractable>(out IInteractable interactedPart))
                    {
                        interactedPart.OnOnteract();
                    }
                }
            }

        }

        if (Input.GetMouseButtonUp(0))
            isPressed = false;

        if (isPressed && objectToBeInspected != null)
        {
            Vector3 deltaMousePos = (Input.mousePosition - lastMousePosition);

            objectToBeInspected.Rotate(Vector3.up, -1 * deltaMousePos.x * rotationSpeed * Time.deltaTime, Space.World);
            objectToBeInspected.Rotate(Vector3.right, deltaMousePos.y * rotationSpeed * Time.deltaTime, Space.World);

            lastMousePosition = Input.mousePosition;

        }

    }

    private bool CastRayAndCheckIfInBoundaries(out Ray ray)
    {
        RectTransform rectTransform = rawImage.rectTransform;

        bool isInBoundaries = RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, Input.mousePosition, null, out Vector2 localMousePos)
            && rectTransform.rect.Contains(localMousePos);

        Vector2 normalizedUV = new Vector2(
            Mathf.InverseLerp(-rectTransform.rect.width / 2, rectTransform.rect.width / 2, localMousePos.x),
            Mathf.InverseLerp(-rectTransform.rect.height / 2, rectTransform.rect.height / 2, localMousePos.y)
        );

        Vector3 viewportPoint = new Vector3(normalizedUV.x, normalizedUV.y, 0);
      
        ray = inspectorCam.ViewportPointToRay(viewportPoint);
      

        return isInBoundaries;
    }
}
