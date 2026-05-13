using UnityEngine;
using UnityEngine.Events;

public class PadlockInteraction : MonoBehaviour
{
    [Header("Combination")]
    [SerializeField] string combination = "1234";
    [SerializeField] bool resetOnWrongCode = true;
    [SerializeField] float wrongCodeResetDelay = 1f;

    [Header("Unlock Targets")]
    [SerializeField] DoorInteraction doorToOpen;
    [SerializeField] UnityEvent onUnlock;
    [SerializeField] UnityEvent onWrongCode;

    string currentEntry = "";
    bool unlocked;
    bool isResetting;

    public bool IsUnlocked => unlocked;
    public string CurrentEntry => currentEntry;

    public void AddDigit(int digit)
    {
        if (unlocked || isResetting)
            return;

        if (currentEntry.Length >= combination.Length)
        {
            ResetEntry();
        }

        currentEntry += digit.ToString();
        Debug.Log($"Padlock entry: {currentEntry}");

        if (currentEntry.Length >= combination.Length)
        {
            if (currentEntry == combination)
            {
                Unlock();
            }
            else
            {
                WrongCode();
            }
        }
    }

    void Unlock()
    {
        unlocked = true;
        currentEntry = combination;
        Debug.Log("Padlock unlocked.");

        if (doorToOpen != null)
        {
            doorToOpen.UnlockDoor();
            doorToOpen.ToggleDoor();
        }

        onUnlock?.Invoke();
    }

    void WrongCode()
    {
        Debug.Log("Padlock wrong code.");
        onWrongCode?.Invoke();

        if (resetOnWrongCode)
        {
            if (wrongCodeResetDelay <= 0f)
            {
                ResetEntry();
            }
            else
            {
                isResetting = true;
                Invoke(nameof(ResetEntry), wrongCodeResetDelay);
            }
        }
    }

    public void ResetEntry()
    {
        currentEntry = "";
        isResetting = false;
    }

    public void SetCombination(string newCombination)
    {
        combination = newCombination;
        ResetEntry();
        unlocked = false;
    }
}
