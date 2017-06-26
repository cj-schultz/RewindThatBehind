using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawerPortal : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("IgnorePortal"))
        {     
            // Entering the portal
            if(other.transform.position.z >= transform.position.z)
            {
                other.gameObject.AddComponent<DrawerItemInPortal>();

                other.GetComponent<Rigidbody>().isKinematic = true;
            }            
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("IgnorePortal"))
        {            
            // Exiting the portal
            if (other.transform.position.z >= transform.position.z)
            {
                Destroy(other.gameObject.GetComponent<DrawerItemInPortal>());

                other.GetComponent<Rigidbody>().isKinematic = false;
            }            
        }
    }
}
