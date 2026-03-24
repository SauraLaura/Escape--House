using UnityEngine;

public class DoorInteraction : MonoBehaviour
{

    Animator DoorAnim;
    bool doorOpened = false;
    void Update()
    {
        Interact();
    }

    void Start()
    {
        DoorAnim = GetComponent<Animator>();
    }

    private void Interact()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.E) && !doorOpened)
        {
            Debug.Log("Door interacted with");
            OpenDoor();
        }

            else if (Input.GetKeyDown(KeyCode.E) && doorOpened)
            {
                Debug.Log("Door closed");
                CloseDoor();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // isInRange = false;
        }
    }

    private void OpenDoor()
    {
        DoorAnim.SetBool("OpenDoor", true);
        doorOpened = true;
    }
    private void CloseDoor()
    {
        DoorAnim.SetBool("CloseDoor", true);
        doorOpened = false;
    }
}

