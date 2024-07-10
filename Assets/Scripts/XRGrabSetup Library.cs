using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRGrabSetupLibrary
{
    //XrGrabSetup(futureGrab)
    // Transform futureGrab : transfrom that will have an XRGrabInteractable script after this function
    // bool directInteractionOnly : whether or not the object can only be grabbed with direct interaction
    //
    //Takes a transform and adds a grab interactible if none are present and sets the desired options (in function)
    public static void XrGrabSetup(Transform futureGrab, bool directInteractionOnly)
    {
        //if the object itself is null do nothing
        if (futureGrab == null)
        {
            return;
        }

        XRGrabInteractable grabComponent;
        //if the object has no XRGrabbable component
        if (futureGrab.GetComponents<XRGrabInteractable>().Length == 0)
        {
            //create a XRGrab component
            grabComponent = futureGrab.AddComponent<XRGrabInteractable>();
        }
        else //if a component already exits
        {
            //modifiy the existing component instead
            grabComponent = futureGrab.GetComponent<XRGrabInteractable>();
        }

        //setting the desired options for the grab interactable
        grabComponent.useDynamicAttach = true;
        grabComponent.movementType = XRBaseInteractable.MovementType.VelocityTracking;
        if (directInteractionOnly)
        {
            grabComponent.interactionLayers = InteractionLayerMask.GetMask("Direct Interaction");
        }
    }
    public static void XrGrabSetup(Transform futureGrab)
    {
        XrGrabSetup(futureGrab, false);
    }
}
