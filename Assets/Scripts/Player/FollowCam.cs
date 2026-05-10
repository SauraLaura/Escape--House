using UnityEngine;

public class FollowCam : MonoBehaviour
{
    [SerializeField]Transform camTrans;
    void Update()
    {
        // gameObject.transform.position = new Vector3(camTrans.position.x, camTrans.position.y, gameObject.transform.position.z);
    }
}
