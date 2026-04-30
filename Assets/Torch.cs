using UnityEngine;

public class Torch : MonoBehaviour
{
    public Light torchLight;
    public ParticleSystem flameParticles;

    private bool isLit = false;

    void Start()
    {
        // Ensure the torch starts unlit
        torchLight.enabled = false;
        flameParticles.Stop();
    }

    void Update()
    {
        // Check for player input to light the torch
        if (Input.GetKeyDown(KeyCode.E) && !isLit)
        {
            LightTorch();
        }
    }

    void LightTorch()
    {
        isLit = true;
        torchLight.enabled = true;
        flameParticles.Play();
    }
}
