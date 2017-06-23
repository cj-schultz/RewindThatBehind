using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class RewindController : MonoBehaviour
{
    public static bool isRewinding = false;

    public TimeBody[] rewindableObjects;
    public TwoPointOscilate[] oscillatingCubes;

    private VRTK_ControllerEvents controllerEvents;
    private VRTK_InteractGrab interactor;

    private void Awake()
    {
        controllerEvents = GetComponent<VRTK_ControllerEvents>();
        interactor = GetComponent<VRTK_InteractGrab>();
    }

    private void OnEnable()
    {
        controllerEvents.TouchpadPressed += HandleTouchpadPressed;
        controllerEvents.TouchpadReleased += HandleTouchpadRelease;        

        interactor.ControllerGrabInteractableObject += HandleObjectGrabbed;
        interactor.ControllerUngrabInteractableObject += HandleObjectDropped;

        controllerEvents.ButtonTwoPressed += HandleStartMenuPress;
    }

    private void OnDisable()
    {
        controllerEvents.TouchpadPressed -= HandleTouchpadPressed;
        controllerEvents.TouchpadReleased -= HandleTouchpadRelease;        

        interactor.ControllerGrabInteractableObject -= HandleObjectGrabbed;
        interactor.ControllerUngrabInteractableObject -= HandleObjectDropped;

        controllerEvents.ButtonTwoPressed -= HandleStartMenuPress;
    }
        
    private void HandleTouchpadPressed(object sender, ControllerInteractionEventArgs e)
    {        
        for (int i = 0; i < rewindableObjects.Length; i++)
        {
            // Only rewind if the object isn't grabbed
            if(!rewindableObjects[i].GetComponent<VRTK_InteractableObject>().IsGrabbed())
            {
                rewindableObjects[i].StartRewind();
            }                        
        }

        RewindController.isRewinding = true;                        
    }   

    private void HandleTouchpadRelease(object sender, ControllerInteractionEventArgs e)
    {        
        for (int i = 0; i < rewindableObjects.Length; i++)
        {
            rewindableObjects[i].StopRewind();
        }
        
        RewindController.isRewinding = false;
    }

    private void HandleObjectGrabbed(object sender, ObjectInteractEventArgs e)
    {
        if (e.target.GetComponent<TimeBody>())
        {
            e.target.GetComponent<TimeBody>().StopRewind();           
        }
    }

    private void HandleObjectDropped(object sender, ObjectInteractEventArgs e)
    {
        if (e.target.GetComponent<TimeBody>())
        {
            // This is here because VRTK swaps the kinematic state from the state it was picked up at. If we picked this up during 
            // rewind (when it is kinematic), then it would be kinematic again when dropped.
            e.target.GetComponent<Rigidbody>().isKinematic = false;

            if(RewindController.isRewinding)
            {
                e.target.GetComponent<TimeBody>().SetToUnrewindableMaterial();
            }
        }
    }

    private void HandleStartMenuPress(object sender, ControllerInteractionEventArgs e)
    {
        for (int i = 0; i < oscillatingCubes.Length; i++)
        {
            oscillatingCubes[i].Reset();
        }
    }
}
