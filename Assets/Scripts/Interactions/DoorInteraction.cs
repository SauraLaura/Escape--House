using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    public Animator DoorAnim;
    public bool doorOpened = false;


    void Start()
    {
        DoorAnim = GetComponent<Animator>();
    }

    public void OpenDoor()
    {
        DoorAnim.SetBool("OpenDoor", true);
        doorOpened = true;
    }
    public void CloseDoor()
    {
        DoorAnim.SetBool("OpenDoor", false);
        doorOpened = false;
    }
    
}

