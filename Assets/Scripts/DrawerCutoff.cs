using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

//[ExecuteInEditMode]
public class DrawerCutoff : MonoBehaviour
{    
    public Material drawerCutoffMat;

    public Transform zCutoffPoint;
    public Transform frontOfDrawerPoint;

    [Header("Colliders to resize")]
    public BoxCollider[] drawerCollidersToCutoff;
    public BoxCollider drawerContainer;

    void Start()
    {
        // These two points should only be offset in Z
        Assert.IsTrue(zCutoffPoint.position.x == frontOfDrawerPoint.position.x);
        Assert.IsTrue(zCutoffPoint.position.y == frontOfDrawerPoint.position.y);        
    }

    void Update()
    {
        drawerCutoffMat.SetFloat("_ZCutoff", zCutoffPoint.position.z);

        for (int i = 0; i < drawerCollidersToCutoff.Length; i++)
        {
            BoxCollider wall = drawerCollidersToCutoff[i];
            wall.size = new Vector3(Vector3.Distance(zCutoffPoint.position, frontOfDrawerPoint.position) / wall.transform.localScale.x, wall.size.y, wall.size.z);

            float wallXPosition;
            wallXPosition = 0.5f - (wall.size.x / 2); 
            wall.center = new Vector3(wallXPosition, wall.center.y, wall.center.z);
        }

        drawerContainer.size = new Vector3(Vector3.Distance(zCutoffPoint.position, frontOfDrawerPoint.position) / drawerContainer.transform.localScale.x, drawerContainer.size.y, drawerContainer.size.z);

        float xPosition;
        xPosition = 0.5f - (drawerContainer.size.x / 2);
        drawerContainer.center = new Vector3(xPosition, drawerContainer.center.y, drawerContainer.center.z);
    }
}
