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
    [SerializeField] GameObject Planks; // Reference to the planks GameObject
    [SerializeField] DialogueData lockedDoorDialogue; // Dialog shown when trying to open without a crowbar
    [SerializeField] DialogueData lockedPadlockDialogue; // Dialog shown when the padlock is locked

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
        if (isLockedByPlanks)
        {
            PickUpObj playerPickup = FindAnyObjectByType<PickUpObj>();
            bool hasCrowbar = playerPickup != null && playerPickup.isHoldingCrowbar;

            if (hasCrowbar)
            {
                Destroy(Planks);
                isLockedByPlanks = false;
                UnityEngine.Debug.Log("Door is locked by planks. Remove them to open the door.");
                doorOpened = false; // Prevent the door from opening immediately after removing planks
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
    }

    public void UnlockDoor()
    {
        isLockedByPlanks = false;
        isLockedByPadlock = false;
    }
}

