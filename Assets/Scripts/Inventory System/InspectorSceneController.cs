using UnityEngine;
using UnityEngine.UI;

public class InspectorScene : MonoBehaviour
{
    public Camera inspectorCam;
    public Transform objectToBeInspected;
    private Vector3 lastMousePosition;
    [SerializeField] private RawImage rawImage;
    [SerializeField] private float rotationSpeed;
    [Header("Conditions")]
    [SerializeField] private bool isPressed;

    private void Start()
    {
        rawImage = GetComponent<RawImage>();
    }

    private void Update()
    {
        if (IsMouseInBoundaries())
        {
            if (Input.GetMouseButtonDown(0))
            {
                isPressed = true;
                lastMousePosition = Input.mousePosition;
            }
        }

        if (Input.GetMouseButtonUp(0))
            isPressed = false;

        if (isPressed && objectToBeInspected != null)
        {
            Vector3 deltaMousePos = (Input.mousePosition - lastMousePosition);

            objectToBeInspected.Rotate(Vector3.up, -1 * deltaMousePos.x * rotationSpeed * Time.deltaTime, Space.World);
            objectToBeInspected.Rotate(Vector3.right, deltaMousePos.y * rotationSpeed * Time.deltaTime, Space.World);
            objectToBeInspected.Rotate(Vector3.forward, deltaMousePos.z * rotationSpeed * Time.deltaTime, Space.World);

            lastMousePosition = Input.mousePosition;

        }


    }

    private bool IsMouseInBoundaries()
    {
        RectTransform rectTransform = rawImage.rectTransform;

        return RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, Input.mousePosition, null, out Vector2 localMousePos)
            && rectTransform.rect.Contains(localMousePos);
    }
}
