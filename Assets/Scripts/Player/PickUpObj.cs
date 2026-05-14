using System;
using System.Diagnostics;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.UI;

public class PickUpObj : MonoBehaviour
{
    [SerializeField] float rayDistance;
    [SerializeField] GameObject Player;
    [SerializeField] Transform holdPosition;
    [SerializeField] GameObject heldObj;
    [SerializeField] Rigidbody heldObjRb;

    InputSystem_Actions inputActions;
    InputAction interactAction;
    InputAction ObjPickUpNDrop;

    Ray ray;
    Vector3 originalLocalScale; // Store original scale when picking up
    
    
    public bool isHoldingCrowbar = false;
    public bool isHoldingKey = false;
    
    [SerializeField] Image crosshair;
    [SerializeField] bool isInteractableInSight = false;

    void OnEnable()
    {
        inputActions = new InputSystem_Actions();
        inputActions.Enable();
        interactAction = inputActions.Player.Interact;
        ObjPickUpNDrop = inputActions.Player.ObjPickDrop;
    }

    void OnDisable()
    {
        inputActions.Disable();
    }

    void Update()
    {
        if (ObjPickUpNDrop == null)
            return;

        UpdateInteractableSight();

        if (ObjPickUpNDrop.WasPressedThisFrame())
        {
            if (heldObj != null)
            {
                MoveObject();
                StopClipping();
                DropObject();
            }
            else
            {
                RayDetection();
            }
        }
        
        if(interactAction.WasPressedThisFrame())
        {
            DoorInteractionFunc();
            ButtonInteractionFunc();
            LeverInteractionFunc();
        }
        CrossHairChange();
    }

    void UpdateInteractableSight()
    {
        ray = new Ray(transform.position, transform.forward);
        int interactableMask = LayerMask.GetMask("Interactable", "Door", "Button", "Lever");
        isInteractableInSight = Physics.Raycast(ray, out _, rayDistance, interactableMask);
    }

    void RayDetection()
    {
        ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance, LayerMask.GetMask("Interactable")))
        {
            // Debug.Log("Hit an Interactable object: " + hit.collider.name);
            // Debug.Log("The held object : ", heldObj);
            PickUpObject(hit.transform.gameObject);
        }
    }

    void CrossHairChange()
    {
        float targetAlpha = isInteractableInSight ? 1f : 0.1f;
        crosshair.color = new Color(1f, 1f, 1f, targetAlpha);
    }

    void DoorInteractionFunc()
    {
        ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit2, rayDistance, LayerMask.GetMask("Door")))
        {
            // UnityEngine.Debug.Log("Hit a door: " + hit2.collider.name);
            DoorInteraction hitDoor = hit2.collider.GetComponentInParent<DoorInteraction>();
            if (hitDoor != null)
            {
                hitDoor.ToggleDoor();
            }
            isInteractableInSight = true;
        }
        isInteractableInSight = false;
    } 

    // void LetterInteractionFunc()
    // {
    //     ray = new Ray(transform.position, transform.forward);
    //     if (Physics.Raycast(ray, out RaycastHit hit3, rayDistance, LayerMask.GetMask("Letter")))
    //     {
    //         UnityEngine.Debug.Log("Hit a letter: " + hit3.collider.name);
    //         LetterInteraction hitLetter = hit3.collider.GetComponentInParent<LetterInteraction>();
    //         if (hitLetter != null)
    //         {
    //             hitLetter.DisplayLetter();
    //         }
    //     }
    // }

    void ButtonInteractionFunc()
    {
        ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit4, rayDistance, LayerMask.GetMask("Button")))
        {
            // UnityEngine.Debug.Log("Hit a button: " + hit4.collider.name);
            ButtonInteraction hitButton = hit4.collider.GetComponentInParent<ButtonInteraction>();
            if (hitButton != null)
            {
                hitButton.PressButton();
            }
            isInteractableInSight = true;
        }
        isInteractableInSight = false;
    }

    void LeverInteractionFunc()
    {
        ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit5, rayDistance, LayerMask.GetMask("Lever")))
        {
            // UnityEngine.Debug.Log("Hit a lever: " + hit5.collider.name);
            LeverInteraction hitLever = hit5.collider.GetComponentInParent<LeverInteraction>();
            if (hitLever != null)
            {
                hitLever.ToggleLever();
            }
            isInteractableInSight = true;
        }
        isInteractableInSight = false;
    }

    void PickUpObject(GameObject obj)
    {
        if(obj.GetComponent<Rigidbody>() != null)
        {
            heldObj = obj; // Store the reference to the held object
            heldObjRb = obj.GetComponent<Rigidbody>();
            originalLocalScale = obj.transform.localScale; // Store original scale before parenting
            heldObjRb.isKinematic = true; // Make the object kinematic to disable physics interactions
            heldObj.transform.position = holdPosition.position;
            obj.transform.SetParent(holdPosition); // Parent the object to the hold position
            heldObj.layer = LayerMask.NameToLayer("Interactable"); // Change the layer to "Interactable" to prevent raycast detection
            heldObj.transform.localRotation = new Quaternion(0f, 0f, 0f, 1f); //reset object rotation to prevent weird angles when picking up
            //heldObj.transform.localScale = new Vector3(heldObj.transform.localScale.x, heldObj.transform.localScale.y, heldObj.transform.localScale.z); //reset object scale to prevent weird scaling when picking up
            //heldObj.transform.localPosition = new Vector3(0f, 0f, 0f); //reset object position to holdPos position to prevent weird offsets when picking up
            // Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), Player.GetComponent<Collider>(), true); 
            if (heldObj.tag == "Crowbar")
            {
                // UnityEngine.Debug.Log("Picked up a crowbar!");
                isHoldingCrowbar = true;
            }
            else if (heldObj.tag == "Key")
            {
                // UnityEngine.Debug.Log("Picked up a key!");
                isHoldingKey = true;
            }
            // else
            // {
            //     isHoldingCrowbar = false;
            // }
        }
    }

    void DropObject()
    {
        //re-enable collision with player
        // Physics.IgnoreCollision(heldObj.GetComponent<Collider>(),Player.GetComponent<Collider>(), false);
        // heldObj.layer = 0; //object assigned back to default layer
        heldObj.transform.parent = null; //unparent object first
        heldObj.transform.localScale = originalLocalScale; // Restore original scale after unparenting
        heldObjRb.isKinematic = false;
        heldObjRb.AddForce(new Vector3(0f, 0f, -2f), ForceMode.Impulse); // Add an upward force to the object when picked up
        if (heldObj.tag == "Crowbar")
        {
            isHoldingCrowbar = false;
        }
        else if (heldObj.tag == "Key")
        {
            // UnityEngine.Debug.Log("Picked up a key!");
            isHoldingKey = false;
        }
        heldObj = null; //undefine game object
        heldObjRb = null; //undefine rigidbody
    }
    void MoveObject()
    {
        //keep object position the same as the holdPosition position
        heldObj.transform.position = holdPosition.position;
    }

    void StopClipping() //function only called when dropping/throwing
    {
        var clipRange = Vector3.Distance(heldObj.transform.position, transform.position); //distance from holdPos to the camera
        //have to use RaycastAll as object blocks raycast in center screen
        //RaycastAll returns array of all colliders hit within the cliprange
        RaycastHit[] hits;
        hits = Physics.RaycastAll(transform.position, transform.TransformDirection(Vector3.forward), clipRange);
        //if the array length is greater than 1, meaning it has hit more than just the object we are carrying
        if (hits.Length > 1)
        {
            //change object position to camera position 
            heldObj.transform.position = transform.position + new Vector3(0f, -0.5f, 0f); //offset slightly downward to stop object dropping above player 
            //if your player is small, change the -0.5f to a smaller number (in magnitude) ie: -0.1f
        }
    }

     void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * rayDistance);
    }
}
