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
    [SerializeField] GameObject Planks; // Reference to the planks GameObject
    [SerializeField] DialogueData lockedDoorDialogue; // Dialog shown when trying to open without a crowbar

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

        if (isLockedByPlanks)
        {
            if (hasCrowbar)
            {
                Destroy(Planks);
                isLockedByPlanks = false;
                UnityEngine.Debug.Log("Door is locked by planks. Remove them to open the door.");
            }
            else
            {
                DialogueManager.instance.StartDialogue(lockedDoorDialogue, null);
            }
                // UnityEngine.Debug.Log("Door is locked by planks. You need a crowbar to remove them.");
                doorOpened = false; // Prevent the door from opening
        }
    }
}

