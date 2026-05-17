using System.Collections;
using UnityEngine;
using Unity.Cinemachine;

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

    // Camera shake settings
    [SerializeField] float cameraShakeAmplitude = 0.5f;
    [SerializeField] float cameraShakeFrequency = 10f;
    [SerializeField] float cameraShakeDuration = 0.2f;
    [SerializeField] CinemachineBasicMultiChannelPerlin cameraShakeComponent;

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
            PickUpObj.instance.doorOpenEvent?.Invoke();
        }
        else
        {
            targetRotation = closedRotation;
            PickUpObj.instance.doorCloseEvent?.Invoke();
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
                PickUpObj.instance.onLockedDoor?.Invoke();
                ApplyCameraShake();
                DialogueManager.instance.StartDialogue(lockedDoorDialogue, null);
                doorOpened = false;
            }
        }
        else if (isLockedByPadlock)
        {
            if (lockedPadlockDialogue != null)
            {
                PickUpObj.instance.onLockedDoor?.Invoke();
                ApplyCameraShake();
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
                PickUpObj.instance.onLockedDoor?.Invoke();
                ApplyCameraShake();
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

    /// Applies a camera shake effect when a locked door is attempted to be opened.
    private void ApplyCameraShake()
    {
        if (cameraShakeComponent != null)
        {
            StartCoroutine(ShakeCamera());
        }
    }

    /// Coroutine to apply camera shake for a specified duration.
    private IEnumerator ShakeCamera()
    {
        float elapsedTime = 0f;
        
        // Apply shake
        cameraShakeComponent.AmplitudeGain = cameraShakeAmplitude;
        cameraShakeComponent.FrequencyGain = cameraShakeFrequency;

        // Wait for the duration
        while (elapsedTime < cameraShakeDuration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Reset shake
        cameraShakeComponent.AmplitudeGain = 0f;
        cameraShakeComponent.FrequencyGain = 0f;
    }
}

