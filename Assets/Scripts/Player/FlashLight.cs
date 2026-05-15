using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class FlashLight : MonoBehaviour
{
    [SerializeField] GameObject FlashLightObj;

    InputSystem_Actions inputActions;
    InputAction flashLightAction;

    [SerializeField] UnityEvent lightSwitchEvent;

    void OnEnable()
    {
        inputActions = new InputSystem_Actions();
        inputActions.Enable();
        flashLightAction = inputActions.Player.FlashLight;
    }

    void OnDisable()
    {
        inputActions.Disable();
    }

    void Update()
    {
        if (flashLightAction != null && flashLightAction.WasPressedThisFrame())
        {
            FlashLightObj.SetActive(!FlashLightObj.activeSelf); // Toggle the flashlight on and off
            lightSwitchEvent?.Invoke();
        }
    }
}
