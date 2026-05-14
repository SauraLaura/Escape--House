using UnityEngine;
using UnityEngine.Events;

public class LeverInteraction : MonoBehaviour
{
    [SerializeField] int rotAngle = 90;
    // [SerializeField] bool ReverseRot = false;
    [SerializeField] float rotSpeed = 3f;

    [Header("Lever Events")]
    public UnityEvent onPulled;

    Quaternion targetRotation;
    Quaternion restRotation;
    bool isRotating;
    bool leverPulled;

    void Start()
    {
        restRotation = transform.rotation;
        targetRotation = restRotation;
    }

    void Update()
    {
        if (isRotating)
        {
            RotateLever();
        }
    }

    public void ToggleLever()
    {
        if (isRotating)
            return;

        leverPulled = !leverPulled;
        SetTargetRotation();
        isRotating = true;
        // Debug.Log("Lever " + gameObject.name + " has been pulled!");
        onPulled?.Invoke();
    }

    void SetTargetRotation()
    {
        if (leverPulled)
        {
            targetRotation = restRotation * Quaternion.Euler(rotAngle,0,0);
        }
        else
        {
            targetRotation = restRotation;
        }
    }

    void RotateLever()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotSpeed);

        if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
        {
            transform.rotation = targetRotation;
            isRotating = false;
        }
    }
}
