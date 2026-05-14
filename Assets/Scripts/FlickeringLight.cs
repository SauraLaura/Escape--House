using System;
using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
    [SerializeField] Light flickeringLight;
    float minIntensity = 0f;
    [SerializeField] float maxIntensity;
    [SerializeField] float flickerSpeed;
    // [SerializeField] float cuurentIntensity;

    bool useMaxIntensity = false;

    void OnEnable()
    {
        InvokeRepeating("Flicker", 0f, flickerSpeed);
    }

    void OnDisable()
    {
        CancelInvoke("Flicker");
    }

    void Flicker()
    {
        if (flickeringLight != null)
        {
            flickeringLight.intensity = useMaxIntensity ? maxIntensity : minIntensity;
            useMaxIntensity = !useMaxIntensity;
        }
    }
}
