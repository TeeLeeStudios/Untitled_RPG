using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Select : NetworkBehaviour
{
    private new Transform transform;
    private new Camera camera;
    [SerializeField] private Material selectionMaterial;
    [SerializeField] private Material defaultMeterial;
    private string SelectableTag = "Selectable";
    private bool toggle = false;

    private Transform _Selection;

    // Start is called before the first frame update
    void Start()
    {
       if (!isLocalPlayer)
        {
            return;
        }
        transform = GetComponent<Transform>();
        camera = GetComponentInChildren<Camera>();
        if (!selectionMaterial)
        {
            Debug.Log("NO SELECTION MATERIAL SELECTED");
        }
        if (!defaultMeterial)
        {
            Debug.Log("NO DEFAULT MATERIAL SELECTED");
        }

    }

    // Update is called once per frame
    void Update()
    {
        //Verify this is ours to control
        if (!isLocalPlayer)
        {
            return;
        }

        if (_Selection != null)
        {
            Renderer hitRend = _Selection.GetComponent<Renderer>();
            if (toggle == false)
            {
                hitRend.material = defaultMeterial;
                _Selection = null;
            }
        }
        //User pressed left Mouse button
        if (Input.GetMouseButtonDown(0))
        {
            if (toggle == false)
            {
                toggle = true;
            }
            else if(toggle == true){
                toggle = false;
            }
            //Let's cast a ray
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray,out hit))
            {
                //Let's get what we hit
                Transform hitTrans = hit.transform;
                //If this is an object that is selectable
                if (hitTrans.CompareTag(SelectableTag))
                {
                    //Let's get the renderer component so we can change the material
                    Renderer hitRend = hitTrans.GetComponent<Renderer>();
                    //Verify we have a renderer and change the material to the selection material
                    if (hitRend != null)
                    {
                        if (toggle == true)
                        {
                            hitRend.material = selectionMaterial;
                        }
                    }
                    _Selection = hitTrans;
                }
            }

        }
    }
}
