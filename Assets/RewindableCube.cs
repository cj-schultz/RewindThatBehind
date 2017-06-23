using UnityEngine;
using VRTK;

public class RewindableCube : VRTK_InteractableObject
{
    [Header("Inherited Members")]
    public TimeBody timeBody;
    
    // @NOTE: This needs to have "hold to use" to be true
    public override void StartUsing(GameObject currentUsingObject)
    {
        base.StartUsing(currentUsingObject);
        
        timeBody.ToggleRewindable();                 
    }
}
