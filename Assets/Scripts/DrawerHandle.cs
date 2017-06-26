using UnityEngine;
using VRTK;

public class DrawerHandle : VRTK_InteractableObject
{
    [Header("Inherited Members")]
    public Transform closedPoint;
    public Transform maxOpenPoint;

    public Transform slidableDrawer;
    
    protected override void Update()
    {
        base.Update();

        if(IsUsing())
        {            
            Vector3 newPosition = new Vector3(slidableDrawer.transform.position.x, slidableDrawer.transform.position.y, GetUsingObject().transform.position.z);

            if(newPosition.z >= closedPoint.position.z && newPosition.z <= maxOpenPoint.position.z)
            {
                slidableDrawer.transform.position = newPosition;
            }            
        }
    }    
}
