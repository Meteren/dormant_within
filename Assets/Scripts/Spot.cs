using Cinemachine;
using UnityEngine;

public class Spot : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cam;
    [SerializeField] private Vector3 originalRotationVals;
    [SerializeField] private GameObject lookAt;
    [SerializeField] private float lookAtDistanceToCam;
    [SerializeField] private Vector3 capturedPosition = Vector3.zero;
    float rotationSpeed = 500;
    [Header("Conditions")]
    [SerializeField] private bool isLookingAt;
    [SerializeField] private bool shouldLookAt;
    void Start()
    {
        cam = GetComponentInChildren<CinemachineVirtualCamera>();
    }

    private void FixedUpdate()
    {
        Vector3 deltaPos = lookAt.transform.position - capturedPosition;
        if (shouldLookAt && !isLookingAt)
            CheckAndRotateIfNeeded(deltaPos);
        capturedPosition = lookAt.transform.position;
    }
    private void Update()
    {
       
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            cam.Priority = 1;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            cam.Priority = 0;            
            ResetCam();

        }
            
    }
    private void CheckAndRotateIfNeeded(Vector3 deltaPos)
    {
        Vector3 rotationDegrees = cam.transform.eulerAngles;

        if (CalculateDistanceToRotate() < lookAtDistanceToCam)
        {
            Debug.Log("Calculating");
            Vector2 xzFlat = new Vector2(deltaPos.x, deltaPos.z);
            float rotationDirection = Mathf.Sign(Vector3.Dot(cam.transform.forward, deltaPos)); 

            rotationDegrees.x += rotationDirection * xzFlat.magnitude * rotationSpeed * Time.deltaTime;
        }

        cam.transform.rotation = Quaternion.Euler(rotationDegrees);
    }

    private float CalculateDistanceToRotate()
    {
        float distance = Vector3.Distance(lookAt.transform.position, cam.transform.position);
        return distance;
    }

    private void ResetCam()
    {
        cam.transform.rotation = Quaternion.Euler(originalRotationVals);
        if(!isLookingAt)
            cam.LookAt = null;
    }
   
}
