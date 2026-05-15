using Unity.VisualScripting;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    
    AudioSource audioSource;
    // [SerializeField] float volumeCrowbarDrop = 1f;
    // [SerializeField] float volumeKeyDrop = 1f;

    [SerializeField] AudioClip padlockUnlockSound;
    [SerializeField] float volumePadlockUnlock = 1f;
    [SerializeField] AudioClip padlockWrongSound;
    [SerializeField] float volumePadlockWrong = 1f;
    [SerializeField] AudioClip padlockButtonPressSound;
    [SerializeField] float volumePadlockButtonPress = 1f;

    [SerializeField] AudioClip doorOpenSound;
    [SerializeField] float volumeDoorOpen = 1f;
    [SerializeField] AudioClip doorCloseSound;
    [SerializeField] float volumeDoorClose = 1f;

    [SerializeField] AudioClip crowbarPickupSound;
    [SerializeField] float volumeCrowbarPickup = 1f;
    // [SerializeField] AudioClip crowbarDropSound;
    [SerializeField] AudioClip keyPickupSound;
    [SerializeField] float volumeKeyPickup = 1f;
    // [SerializeField] AudioClip keyDropSound;

    [SerializeField] AudioClip lockedDoorSound;
    [SerializeField] float volumeLockedDoor = 1f;
    [SerializeField] AudioClip lightSwitchSound;
    [SerializeField] float volumeLightSwitch = 1f;

    [SerializeField] AudioClip leverPullSound;
    [SerializeField] float volumeLeverPull = 1f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PadlockUnlocked()
    {
        // Play padlock unlocked sound effect
        audioSource.PlayOneShot(padlockUnlockSound, volumePadlockUnlock);
    }

    public void PadlockButtonPressed()
    {
        // Play padlock button press sound effect (if you have one)
        audioSource.PlayOneShot(padlockButtonPressSound, volumePadlockButtonPress);
    }

    public void PadlockWrong()
    {
        // Play padlock wrong sound effect
        audioSource.PlayOneShot(padlockWrongSound, volumePadlockWrong);
    }

    public void DoorOpened()
    {
        // Play door opened sound effect
        audioSource.PlayOneShot(doorOpenSound, volumeDoorOpen);
    }

    public void DoorClosed()
    {
        // Play door closed sound effect
        audioSource.PlayOneShot(doorCloseSound, volumeDoorClose);
    }

    public void KeyPickedUp()
    {
        // Play key picked up sound effect
        audioSource.PlayOneShot(keyPickupSound, volumeKeyPickup);
    }

    // public void KeyDropped()
    // {
    //     // Play key dropped sound effect
    //     audioSource.PlayOneShot(keyDropSound, volume);
    // }

    public void CrowbarPickedUp()
    {
        // Play crowbar picked up sound effect
        audioSource.PlayOneShot(crowbarPickupSound, volumeCrowbarPickup);
    }

    // public void CrowbarDropped()
    // {
    //     // Play crowbar dropped sound effect
    //     audioSource.PlayOneShot(crowbarDropSound, volume);
    // }

    public void LockedDoor()
    {
        // Play locked door sound effect
        audioSource.PlayOneShot(lockedDoorSound, volumeLockedDoor);
    }

    public void LightSwitchPressed()
    {
        // Play light switch sound effect
        audioSource.PlayOneShot(lightSwitchSound, volumeLightSwitch);
    }

    public void LeverPulled()
    {
        // Play lever pull sound effect
        audioSource.PlayOneShot(leverPullSound, volumeLeverPull);
    }
}
