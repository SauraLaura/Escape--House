using UnityEngine;

public class hatchController : MonoBehaviour
{
    [SerializeField] int rotAngle = 90;
    [SerializeField] float rotSpeed = 3f;

    Quaternion targetRotation;
    Quaternion closedRotation;
    bool isRotating;
    bool hatchOpen;

    void Start()
    {
        closedRotation = transform.rotation;
        targetRotation = closedRotation;
    }

    void Update()
    {
        if (isRotating)
        {
            RotateHatch();
        }
    }

    public void ToggleHatch()
    {
        if (isRotating)
            return;

        hatchOpen = !hatchOpen;
        SetTargetRotation();
        isRotating = true;
    }

    void SetTargetRotation()
    {
        if (hatchOpen)
        {
            targetRotation = closedRotation * Quaternion.Euler(rotAngle, 0, 0);
        }
        else
        {
            targetRotation = closedRotation;
        }
    }

    void RotateHatch()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotSpeed);

        if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
        {
            transform.rotation = targetRotation;
            isRotating = false;
        }
    }
}
