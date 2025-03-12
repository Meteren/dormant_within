using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item, IEquippable, ICombinable
{
    [Header("Position To Anchor")]
    [SerializeField] private string positionName;

    [Header("Muzzle Point")]
    public Transform muzzlePoint;

    [Header("Range")]
    [SerializeField] private float weapontRange;

    [Header("Clip")]
    [SerializeField] private Clip clip;

    [Header("Layer To Hit")]
    [SerializeField] private LayerMask hitMask;
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
            if(playerController.equippedItem != this as IEquippable)
                UIManager.instance.HandleIndicator($"{itemName} equipped.", 2f);
            playerController.equippedItem = this;
        }
        else
            UIManager.instance.HandleIndicator("Can't equip", 2f);       
      
    }

    public virtual void Unequip()
    {
        transform.SetParent(null);
        gameObject.SetActive(false);
        
    }

    public void OnTryCombine(ItemRepresenter representer)
    {
        Clip clip = representer.representedItem as Clip;
        if (!this.clip.IsFull())
        {
            if (clip == null)
            {
                UIManager.instance.HandleIndicator($"Can't combine {ToString()} with {representer.representedItem.ToString()}", 2f);
                return;
            }
            if (!clip.isEmpty)
            {
                this.clip.IncreaseAmount(clip);
                UIManager.instance.HandleIndicator("Reloaded.", 2f);
            }      
            else
                UIManager.instance.HandleIndicator("Clip is empty. Can't reload.", 2f);
        }
        else
            UIManager.instance.HandleIndicator("Can't reload. Ammo is full.",2f);
       
    }
    public virtual RaycastHit Shoot()
    {
        Debug.Log("Shoot");
        Ray ray = new Ray(muzzlePoint.position, muzzlePoint.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, weapontRange,hitMask))
            Debug.DrawRay(muzzlePoint.position, muzzlePoint.forward * weapontRange, Color.red, 1f);
        else
        {
            Debug.DrawRay(muzzlePoint.position, muzzlePoint.forward * weapontRange, Color.green, 1f);
            hit = default;
        }      
        clip.DecreaseAmount(1);
        return hit;
    }

    public virtual int InflictDamage()
    {
        return 0;
    }

    public Clip GetClip() => clip;

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
