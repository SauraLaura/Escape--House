using System;
using Unity.VisualScripting;
using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    public bool doorOpened = false;
    [SerializeField] int rotAngle;
    [SerializeField] bool ReverseRot;
    [SerializeField] float rotSpeed = 3f;

    Quaternion targetRotation;
    Quaternion closedRotation;
    bool isRotating;

    void Start()
    {
        closedRotation = transform.rotation;
        targetRotation = closedRotation;
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
}

