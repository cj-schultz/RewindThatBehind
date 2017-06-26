using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class DrawerContainer : MonoBehaviour
{
    public Transform drawerBottom;
    public Shader drawerCutoffShader;

    private Dictionary<GameObject, Shader> itemsInDrawer;

    void Awake()
    {        
        itemsInDrawer = new Dictionary<GameObject, Shader>();
    }

    void OnTriggerEnter(Collider other)
    {                
        if (other.gameObject.layer != LayerMask.NameToLayer("IgnorePortal") && other.GetComponent<DrawerItemInPortal>() == null)
        {            
            MeshRenderer otherMesh = other.GetComponent<MeshRenderer>();

            if(otherMesh)
            {
                // Save the objects original material and then set it to the drawer cutoff
                itemsInDrawer.Add(other.gameObject, other.GetComponent<MeshRenderer>().material.shader);
                otherMesh.material.shader = Shader.Find("Custom/DrawerCutoff");

                // Disable shadows on the mesh
                otherMesh.receiveShadows = false;
                otherMesh.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

                // @NOTE: Parenting is a cheap solution to keeping the objects in the container when occluded
                other.transform.SetParent(transform);
            }            
        }        
    }

    void OnTriggerExit(Collider other)
    {           
        if (other.gameObject.layer != LayerMask.NameToLayer("IgnorePortal") && other.GetComponent<DrawerItemInPortal>() == null)
        {
            if (other.transform.root.gameObject.layer == LayerMask.NameToLayer("IgnorePortal"))
            {
                return;
            }
            
            MeshRenderer otherMesh = other.GetComponent<MeshRenderer>();

            if(otherMesh)
            {
                // Set the object back to its original material
                otherMesh.material.shader = itemsInDrawer[other.gameObject];
                itemsInDrawer.Remove(other.gameObject);

                // Enable shadows on the mesh
                //otherMesh.receiveShadows = true;
                //otherMesh.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;

                other.transform.SetParent(null);
            }            
        }
    }
}