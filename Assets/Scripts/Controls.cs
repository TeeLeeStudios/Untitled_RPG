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

    public int WalkSpeed;
    public int RunSpeed;
    public float JumpHeight;

    private bool Run;

    [SerializeField]
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        transform = GetComponent<Transform>();
        rend = GetComponentInChildren<Renderer>();
        animator = GetComponentInChildren<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        if (JumpHeight == 0) JumpHeight = 4;
        if (RunSpeed == 0) RunSpeed = 6;
        if (WalkSpeed == 0) WalkSpeed = 4;
        Run = false;

    }
    
    // Update is called once per frame
    void Update()
    {
        
        if(!isLocalPlayer)
        {

            return;
        }

        #region Movement
        //Get X and Z inputs into a Vec2 for ez access
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Move(input);

        //Enable or disable run
        if (Input.GetButtonDown("Sprint"))
        {
            Debug.Log("Pressed Sprint Button!");
            ToggleRun();
        }

        //Y movement

        if (Input.GetButtonDown("Jump"))
        {
            if (_isGrounded())
            {
                rb.AddForce(0, JumpHeight, 0, ForceMode.Impulse);
                Debug.Log("Jumping");
            }
        }

        //Animation Controller
        float AnimatePercent = ((Run) ? 1 : 0.5f) * input.magnitude;
        animator.SetFloat("MoveAnimation",AnimatePercent);

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

    void Move(Vector2 input)
    {
        //Set float Speed_ to either run/walk speed using ? bool operator
        float Speedx = ((Run) ? RunSpeed : WalkSpeed) * Time.deltaTime * input.x;
        float Speedz = ((Run) ? RunSpeed : WalkSpeed) * Time.deltaTime * input.y;
        //Finally set the new transform
        transform.Translate(Speedx, 0, Speedz);
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

    bool ToggleRun()
    {
        if (Run)
        {
            Run = false;
            return Run;
        }
        Run = true;
        return Run;
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
