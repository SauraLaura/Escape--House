using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] float mouseSens = 150f;
    [SerializeField] Transform playerBody;
    float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen
    }

    void Update()
    {
        float lookX = Input.GetAxis("Mouse X") * mouseSens * Time.deltaTime;
        float lookY = Input.GetAxis("Mouse Y") * mouseSens * Time.deltaTime;

        xRotation -= lookY; // Invert the Y-axis for looking up and down
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Clamp
        
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f); // Apply the rotation to the camera
        playerBody.Rotate(Vector3.up * lookX);//To rotate the parent object which is the player.
    }
}
