using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item, IEquippable
{
    [Header("Position To Anchor")]
    [SerializeField] private string positionName;

    [Header("Muzzle Point")]
    [SerializeField] private Transform muzzlePoint; 
    protected new void Start()
    {
        base.Start();
    }
    public virtual void Equip(PlayerController playerController)
    {
        Transform anchorPosition = FindPositionToAnchor(playerController.transform);
        if (anchorPosition != null)
        {
            transform.SetParent(anchorPosition);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = originalScale;
            SetCollidersActive(false);
            SetRigidBodyKinematic(true);
            transform.gameObject.SetActive(true);
            if(playerController.equippedItem == null)
                UIManager.instance.HandleIndicator($"{itemName} equipped.", 2f);
            playerController.equippedItem = this;
        }
        else
            UIManager.instance.HandleIndicator("Can't equip", 2f);       
      
    } 

    private Transform FindPositionToAnchor(Transform playerTransform)
    {
        Transform[] children = playerTransform.GetComponentsInChildren<Transform>();

        foreach (Transform child in children)
        {
            if(child.gameObject.name == positionName)
                return child;
        }

        return null;
    }

}
