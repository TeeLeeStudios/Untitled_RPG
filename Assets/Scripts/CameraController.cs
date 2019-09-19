using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CameraController : MonoBehaviour
{
    public float FieldOfView = 70f;
    public float sensitivity = 3f;
    public float smoothing = 4.0f;
    // the character is the capsule
    public GameObject character;
    // get the incremental value of mouse moving
    private Vector2 mouseLook;
    // smooth the mouse moving
    private Vector2 smoothV;
    private Camera myCamera;

    void Start()
    {
        //If the camera has no parent then turn off the camera
        if (!this.transform.parent)
        {
            gameObject.GetComponent<Camera>().enabled = false;
            gameObject.GetComponent<AudioListener>().enabled = false;
            return;
        }
        //If the camera's parent is not our player then turn off the camera
        if (!this.transform.parent.GetComponent<Controls>().isLocalPlayer)
        {
            gameObject.GetComponent<Camera>().enabled = false;
            gameObject.GetComponent<AudioListener>().enabled = false;
            return;
        }
        //Verified that this is our character's Camera from ^ so let's set it
        character = this.transform.parent.gameObject;

        //Setting FOV so that it can be managed by player settings later
        myCamera = GetComponent<Camera>();
        if (myCamera)
        {
            myCamera.fieldOfView = FieldOfView;
        }
    }

    void Update()
    {
        //Confirm this is our player first
        if (!this.transform.parent)
        {
            return;
        }
        if (!this.transform.parent.GetComponent<Controls>().isLocalPlayer)
        {
            return;
        }
        
        //1st Person Camera Controls
        var md = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        md = Vector2.Scale(md, new Vector2(sensitivity * smoothing, sensitivity * smoothing));
        // the interpolated float result between the two float values
        smoothV.x = Mathf.Lerp(smoothV.x, md.x, 1f / smoothing);
        smoothV.y = Mathf.Lerp(smoothV.y, md.y, 1f / smoothing);
        // incrementally add to the camera look
        mouseLook += smoothV;
        //Let's make sure our player cannot exceed 90 degrees up or down with the camera
        mouseLook.y = Mathf.Clamp(mouseLook.y, -90, 90);
        //Apply the calculated camera movement
        // vector3.right means the x-axis
        transform.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);
        character.transform.localRotation = Quaternion.AngleAxis(mouseLook.x, character.transform.up);
        
    }

}
