using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DrawerCutoff : MonoBehaviour
{
    public Material drawerCutoffMat;

    public Transform zCutoffPoint;

    void Update()
    {
        drawerCutoffMat.SetFloat("_ZCutoff", zCutoffPoint.position.z);
    }
}
