using UnityEngine;
using UnityEngine.InputSystem;

public class FlashLight : MonoBehaviour
{
    [SerializeField] GameObject FlashLightObj;

    InputSystem_Actions inputActions;
    InputAction flashLightAction;

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
        }
    }
}
