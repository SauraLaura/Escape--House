using UnityEngine;

public class PickUpObj : MonoBehaviour
{
    [SerializeField] float rayDistance;
    [SerializeField] GameObject Player;
    [SerializeField] Transform holdPosition;
    [SerializeField] GameObject heldObj;
    [SerializeField] Rigidbody heldObjRb;
    [SerializeField] float throwForce = 500f;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
                if (heldObj != null)
                {
                    MoveObject();
                    StopClipping();
                    if(Input.GetKeyDown(KeyCode.E))
                    {
                        DropObject();
                    }
                }
                else if (heldObj == null && Input.GetKeyDown(KeyCode.E))
                {
                    RayDetection();
                }
        }
    }

    void RayDetection()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance, LayerMask.GetMask("Interactable")))
        {
            Debug.Log("Hit an Interactable object: " + hit.collider.name);
            PickUpObject(hit.transform.gameObject);
        }      
    }

    void PickUpObject(GameObject obj)
    {
        if(obj.GetComponent<Rigidbody>() != null)
        {
            heldObj = obj; // Store the reference to the held object
            heldObjRb = obj.GetComponent<Rigidbody>();
            heldObjRb.isKinematic = true; // Make the object kinematic to disable physics interactions
            heldObj.transform.position = holdPosition.position;
            obj.transform.SetParent(holdPosition); // Parent the object to the hold position
            heldObj.layer = LayerMask.NameToLayer("Interactable"); // Change the layer to "Interactable" to prevent raycast detection
            // Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), Player.GetComponent<Collider>(), true); 
        }
    }

    void DropObject()
    {
        //re-enable collision with player
        // Physics.IgnoreCollision(heldObj.GetComponent<Collider>(),Player.GetComponent<Collider>(), false);
        // heldObj.layer = 0; //object assigned back to default layer
        heldObjRb.isKinematic = false;
        heldObj.transform.parent = null; //unparent object
        heldObj = null; //undefine game object
        heldObjRb = null; //undefine rigidbody
    }
    void MoveObject()
    {
        //keep object position the same as the holdPosition position
        heldObj.transform.position = holdPosition.position;
    }

    void StopClipping() //function only called when dropping/throwing
    {
        var clipRange = Vector3.Distance(heldObj.transform.position, transform.position); //distance from holdPos to the camera
        //have to use RaycastAll as object blocks raycast in center screen
        //RaycastAll returns array of all colliders hit within the cliprange
        RaycastHit[] hits;
        hits = Physics.RaycastAll(transform.position, transform.TransformDirection(Vector3.forward), clipRange);
        //if the array length is greater than 1, meaning it has hit more than just the object we are carrying
        if (hits.Length > 1)
        {
            //change object position to camera position 
            heldObj.transform.position = transform.position + new Vector3(0f, -0.5f, 0f); //offset slightly downward to stop object dropping above player 
            //if your player is small, change the -0.5f to a smaller number (in magnitude) ie: -0.1f
        }
    }

     void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * rayDistance);
    }

}
