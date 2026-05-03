using UnityEngine;

public class FlashLight : MonoBehaviour
{
    [SerializeField] GameObject FlashLightObj;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            FlashLightObj.SetActive(!FlashLightObj.activeSelf); // Toggle the flashlight on and off
        }
    }
}
