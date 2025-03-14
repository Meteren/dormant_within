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

    private void Update()
    {
       
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            cam.enabled = true;
            cam.Priority = 1;
            
        }
           
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            cam.Priority = 0;            
            //ResetCam();

        }
            
    }

    private void ResetCam()
    {
        cam.transform.rotation = Quaternion.Euler(originalRotationVals);
        if(!isLookingAt)
            cam.LookAt = null;
        cam.enabled = false;
    }
   
}
