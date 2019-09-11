using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Controls : NetworkBehaviour
{

    private Rigidbody rb;
    private new Transform transform;
    private Renderer rend;
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
        rend = GetComponentInChildren<Renderer>();
        Cursor.lockState = CursorLockMode.Locked;
        if (JumpHeight == 0) JumpHeight = 5;
    }
    
    // Update is called once per frame
    void Update()
    {
        
        if(!isLocalPlayer)
        {

            return;
        }

        #region Movement
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
        #endregion

        #region DebugTools
        //Right click to check renderer bounds.
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("Center: " + GetCharacterBounds("center"));
            Debug.Log("Extents: " + GetCharacterBounds("extents"));
            Debug.Log("Max: " + GetCharacterBounds("max"));
            Debug.Log("Min: " + GetCharacterBounds("min"));
            Debug.Log("Size: " + GetCharacterBounds("size"));
        }

    #endregion

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
        //Get the lowest point on the Renderer inside the Player GameObject (will we referred to as GO)
        //and add 0.2f to the Y axis to account for y offset (Player GO may be inside the ground)
        Ray ray = new Ray(GetCharacterBounds("min") + new Vector3(0,0.2f,0), Vector3.up * -1);

        if (Physics.Raycast(ray, out RaycastHit hit, 0.4f))
        {
            Debug.Log("Hit: " + hit.transform.name);
            return true;
        }

        Debug.Log("Nothing was hit");
        return false;
    }

    //A way to get the bounding volumes of the Renderer. 
    Vector3 GetCharacterBounds(string type)
    {
        switch (type)
        {
            //The closest approximate center to a renderer's bounds. More precise than transform.position
            case "center":
                return rend.bounds.center;
            case "extents":
                return rend.bounds.extents;
            case "max":
                return rend.bounds.max;
            case "min":
                return rend.bounds.min;
            case "size":
                return rend.bounds.size;
            default:
                return Vector3.zero;
        }
       
    }
}
