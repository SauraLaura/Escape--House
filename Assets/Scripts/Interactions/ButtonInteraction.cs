using UnityEngine;
using UnityEngine.Events;

public class ButtonInteraction : MonoBehaviour
{
    [Header("Padlock Support")]
    public bool isPadlockButton;
    [Tooltip("Digit to send to the linked padlock.")]
    public int padlockDigit;
    [Tooltip("Padlock that receives this button press.")]
    public PadlockInteraction padlockTarget;

    [Header("Button Events")]
    public UnityEvent onPressed;

    public void PressButton()
    {
        if (isPadlockButton && padlockTarget != null)
        {
            padlockTarget.AddDigit(padlockDigit);
            Debug.Log($"Padlock button {padlockDigit} pressed.");
            PickUpObj.instance.buttonPressEvent?.Invoke();
            return;
        }
        Debug.Log("Button " + gameObject.name + " has been pressed!");
        onPressed?.Invoke();
    }
}
