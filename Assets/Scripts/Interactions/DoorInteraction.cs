using System;
using Unity.VisualScripting;
using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    public bool doorOpened = false;
    [SerializeField] int rotAngle;
    [SerializeField] bool ReverseRot;
    [SerializeField] float rotSpeed = 3f;
    public bool isLockedByPlanks;
    public bool isLockedByPadlock;
    public bool isLockedByKey;
    [SerializeField] GameObject Planks; // Reference to the planks GameObject
    [SerializeField] DialogueData lockedDoorDialogue; // Dialog shown when trying to open without a crowbar
    [SerializeField] DialogueData lockedPadlockDialogue; // Dialog shown when the padlock is locked
    [SerializeField] DialogueData lockedKeyDoorDialogue; // Dialog shown when the door is locked by a key

    Quaternion targetRotation;
    Quaternion closedRotation;
    bool isRotating;

    void Start()
    {
        closedRotation = transform.rotation;
        targetRotation = closedRotation;
        // Planks = transform.GetChild(1).gameObject; // Assuming the planks are a child of the door
    }

    void Update()
    {
        if (isRotating)
        {
            RotateDoor();
        }
    }

    public void ToggleDoor()
    {
        doorOpened = !doorOpened;
        DoorLockCheck();
        SetTargetRotation();
        isRotating = true;
    }

    void SetTargetRotation()
    {
        if (doorOpened)
        {
            targetRotation = closedRotation * Quaternion.Euler(0, ReverseRot ? -rotAngle : rotAngle, 0);
        }
        else
        {
            targetRotation = closedRotation;
        }
    }

    void RotateDoor()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotSpeed);

        if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
        {
            transform.rotation = targetRotation;
            isRotating = false;
        }
    }

    void DoorLockCheck()
    {
        PickUpObj playerPickup = FindAnyObjectByType<PickUpObj>();
        bool hasCrowbar = playerPickup != null && playerPickup.isHoldingCrowbar;
        bool hasKey = playerPickup != null && playerPickup.isHoldingKey;
        
        if (isLockedByPlanks)
        {
            if (hasCrowbar)
            {
                Destroy(Planks);
                isLockedByPlanks = false;
                doorOpened = false; // Prevent the door from opening immediately after removing planks
                // UnityEngine.Debug.Log("Door is locked by planks. Remove them to open the door.");
            }
            else
            {
                DialogueManager.instance.StartDialogue(lockedDoorDialogue, null);
                doorOpened = false;
            }
        }
        else if (isLockedByPadlock)
        {
            if (lockedPadlockDialogue != null)
            {
                DialogueManager.instance.StartDialogue(lockedPadlockDialogue, null);
            }
            else
            {
                UnityEngine.Debug.Log("Door is locked by a padlock. Enter the correct code to unlock it.");
            }
            doorOpened = false;
        }
        else if (isLockedByKey)
        {
            if(hasKey)
            {
                isLockedByKey = false;
                doorOpened = true; // Prevent the door from opening immediately after unlocking
                // UnityEngine.Debug.Log("Door is locked by a key. Use the key to unlock
            }
            else
            {
                // UnityEngine.Debug.Log("Door is locked by a key. Find the key to unlock it.");
                DialogueManager.instance.StartDialogue(lockedKeyDoorDialogue, null);
                doorOpened = false;
            }
        }
    }

    public void UnlockDoor()
    {
        isLockedByPlanks = false;
        isLockedByPadlock = false;
        isLockedByKey = false;
    }
}

