using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Controls : NetworkBehaviour
{

    private Rigidbody rb;
    private new Transform transform;
    private Vector3 Direction;
    public int Speed;
    public float JumpHeight;
    private float zAxis;
    private float xAxis;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        transform = GetComponent<Transform>();
        Cursor.lockState = CursorLockMode.Locked;
        if (JumpHeight == 0) JumpHeight = 8;
    }
    
    // Update is called once per frame
    void Update()
    {
        
        if(!isLocalPlayer)
        {

            return;
        }
        
        //X and Z movement
        zAxis = Input.GetAxis("Vertical") * Speed * Time.deltaTime;
        xAxis = Input.GetAxis("Horizontal") * Speed * Time.deltaTime;
        transform.Translate(xAxis, 0, zAxis);

        //Y movement

        if (Input.GetButtonDown("Jump"))
        {
            if (_isGrounded())
            {
                rb.AddForce(0, JumpHeight, 0, ForceMode.Impulse);
                Debug.Log("Jumping");
            }
        }

        //Toggle the Cursor lock
        if (Input.GetButtonDown("CursorLock")) ToggleCursorLock();

    }

    void ToggleCursorLock()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Debug.Log("Cursor is now unlocked");
            return;
            
        }
        
        if (Cursor.lockState == CursorLockMode.None)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Debug.Log("Cursor is now locked");
            return;
        }
    }

    
    bool _isGrounded()
    {
        //Create a new Ray
        Ray ray = new Ray(transform.position, Vector3.up * -1);
        if (Physics.Raycast(ray, out RaycastHit hit, transform.localScale.y + 0.2f))
        {
            Debug.Log("Hit: " + hit);
            return true;
        }

        Debug.Log("Nothing was hit");
        return false;
    }

}
