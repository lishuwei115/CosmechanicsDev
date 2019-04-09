﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InteractWithInterface : MonoBehaviour
{

    public Interactable interactableObject;
    public float radius;
    public LayerMask interactableLayer;

    GameObject interactedObject;


    public void InteractWithObject()
    {
        if(interactedObject != null)
        {
            if(interactedObject.GetComponent<IInteractableTool>() != null)
            {
                interactedObject.GetComponent<IInteractableTool>().toolInteraction();
            }
        }
        else
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius, interactableLayer);
            //Debug.Log("InteractWithObject()");

            for (int i = 0; i < hitColliders.Length; i++)
            {
                if (hitColliders[i].GetComponent<RepairableObject>() != null)
                {
                    if (hitColliders[i].GetComponent<RepairableObject>().health != hitColliders[i].GetComponent<RepairableObject>().healthMax)
                    {
                        hitColliders[i].GetComponent<IInteractable>().InteractWith();
                        break;
                    }
                }
                else
                {
                    hitColliders[i].GetComponent<IInteractable>().InteractWith();
                    break;
                }

            }
        }
    }
        
    

    public void pickUpObject()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius, interactableLayer);
        //Debug.Log("Calling pickUpObject()");
        //Debug.Log(hitColliders.Length);
        if(interactedObject == null)
        {
            for (int i = 0; i < hitColliders.Length; i++)
            {
                //  Debug.Log("Calling object " + hitColliders[i]);
                if (hitColliders[i].GetComponent<PickUp>() != null)
                {
                    if (hitColliders[i].GetComponent<Battery>() != null)
                    {
                        hitColliders[i].GetComponent<Battery>().unPlugBattery();
                    }
                    hitColliders[i].GetComponent<PickUp>().pickMeUp(transform);
                    interactedObject = hitColliders[i].gameObject;
                    
                    break;
                }
            }
        }
        else
        {
            if(interactedObject.GetComponent<Battery>() != null)
            {
                for (int i = 0; i < hitColliders.Length; i++)
                {
                    
                    if(hitColliders[i].GetComponent<ChargingPort>() != null)
                    {
                        Debug.Log("Calling Plug in");
                        interactedObject.GetComponent<Battery>().PlugBattery(hitColliders[i].GetComponent<ChargingPort>().LockPosition, hitColliders[i].GetComponent<ChargingPort>().port);
                        break;
                    }
                }
            }
            interactedObject.GetComponent<PickUp>().putMeDown();
            interactedObject = null;
        }
    }


    public void callInteract()
    {
        if (interactableObject != null)
        {
            interactableObject.InteractWith();
        }
    }
    public void closeInteract()
    {
        interactableObject = null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    //public void grabObject()
    //{
    //    if(interactedObject != null)
    //    {
    //        interactedObject.transform.position = transform.position;
    //        interactedObject.transform.eulerAngles = transform.eulerAngles;
    //    }
    //}
    //
    //public void throwObject()
    //{
    //    if (interactedObject != null)
    //    {
    //        interactedObject.GetComponent<Rigidbody>().velocity = transform.forward * throwForce;
    //    }
    //}

    //public void InteractWith()
    //{
    //    Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius, interactableLayer);
    //    Debug.Log("Calling InteractWith()");
    //    Debug.Log(hitColliders.Length);
    //    for (int i = 0; i < hitColliders.Length; i++)
    //    {
    //        // Chache this later once the check becomes larger
    //        Debug.Log("Calling object " + i);
    //        if (hitColliders[i].GetComponent<Interactable>() != null)
    //        {
    //            Interactable testedInteractable = hitColliders[i].GetComponent<Interactable>();
    //
    //            if (testedInteractable.pickedUp == false)
    //            {
    //                if (interactableObject == null)
    //                {
    //                    Debug.Log("Setting interactable to " + hitColliders[i]);
    //                    interactableObject = testedInteractable;
    //                    return;
    //                }
    //            }
    //
    //        }
    //    }
    //}

}