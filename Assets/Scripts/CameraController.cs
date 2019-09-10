using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CameraController : MonoBehaviour
{
    public float FieldOfView = 70f;
    public float sensitivity = 0.5f;
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
        if (!this.transform.parent)
        {
            gameObject.GetComponent<Camera>().enabled = false;
            gameObject.GetComponent<AudioListener>().enabled = false;
            return;
        }
        if (!this.transform.parent.GetComponent<Controls>().isLocalPlayer)
        {
            gameObject.GetComponent<Camera>().enabled = false;
            gameObject.GetComponent<AudioListener>().enabled = false;
            return;
        }
        character = this.transform.parent.gameObject;
        //Setting FOV
        myCamera = GetComponent<Camera>();
        if (myCamera)
        {
            myCamera.fieldOfView = FieldOfView;
        }
    }

    void Update()
    {
        if (!this.transform.parent)
        {
            return;
        }
        if (!this.transform.parent.GetComponent<Controls>().isLocalPlayer)
        {
            return;
        }

        var md = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        md = Vector2.Scale(md, new Vector2(sensitivity * smoothing, sensitivity * smoothing));
        // the interpolated float result between the two float values
        smoothV.x = Mathf.Lerp(smoothV.x, md.x, 1f / smoothing);
        smoothV.y = Mathf.Lerp(smoothV.y, md.y, 1f / smoothing);
        // incrementally add to the camera look
        mouseLook += smoothV;
        mouseLook.y = Mathf.Clamp(mouseLook.y, -90, 90);
        // vector3.right means the x-axis
        transform.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);
        character.transform.localRotation = Quaternion.AngleAxis(mouseLook.x, character.transform.up);
        
    }

}
