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
        // Make this the scale of the drawer length
        transform.localScale = new Vector3(drawerBottom.localScale.x, transform.localScale.y, transform.localScale.z);
        transform.position = new Vector3(transform.position.x, transform.position.y, drawerBottom.position.z);

        itemsInDrawer = new Dictionary<GameObject, Shader>();
    }

    void OnTriggerEnter(Collider other)
    {                
        if (other.gameObject.layer != LayerMask.NameToLayer("IgnorePortal"))
        {
            Debug.Log(other.gameObject + " Enter");

            // It shouldn't already be in the dictionary
            Assert.IsTrue(!itemsInDrawer.ContainsKey(other.gameObject));            

            // Save the objects original material and then set it to the drawer cutoff
            itemsInDrawer.Add(other.gameObject, other.GetComponent<MeshRenderer>().material.shader);
            other.GetComponent<MeshRenderer>().material.shader = Shader.Find("Custom/DrawerCutoff");            
        }        
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("IgnorePortal"))
        {
            Debug.Log(other.gameObject + " Exit");

            // It should be in the dictionary
            Assert.IsTrue(itemsInDrawer.ContainsKey(other.gameObject));         

            // Set the object back to its original material
            other.GetComponent<MeshRenderer>().material.shader = itemsInDrawer[other.gameObject];
            itemsInDrawer.Remove(other.gameObject);
                            
        }
    }
}