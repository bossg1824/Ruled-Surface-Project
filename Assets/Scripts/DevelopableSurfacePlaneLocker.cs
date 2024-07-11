using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.XR.Interaction.Toolkit;
using static XRGrabSetupLibrary;
using UnityEditor.Rendering;
using UnityEngine.Events;
using System;

public class DevelopableSurfacePlaneLocker : MonoBehaviour
{
    [SerializeField]
    public Vector3 TargetRot;

    [SerializeField]
    public Vector3 TargetPoint;

    [SerializeField]
    public float AllowedOffset;

    [SerializeField]
    public bool DifferingAllowances;

    [SerializeField]
    public bool DirectOnly;

    [SerializeField]
    public Vector3 DifferentAllowances;

    [SerializeField]
    public float DistanceAllowance;

    private XRGrabInteractable Grabs;

   /* [CustomEditor(typeof(DevelopableSurfacePlaneLocker))]
    public class DevelopableSurfacePlaneLockerEditor : Editor
    {
        SerializedProperty 
        public override void OnInspectorGUI ()
        {
            DrawDefaultInspector();
            DevelopableSurfacePlaneLocker DevSurPlaLok = (DevelopableSurfacePlaneLocker)target;

            EditorGUILayout.Vector3Field("Target Rotation", DevSurPlaLok.TargetRot);
           
        }
    } */

    void Start()
    {
        if(transform.GetComponent<XRGrabInteractable>() == null)
        {
            XrGrabSetup(transform, DirectOnly);
        }
        
        transform.GetComponent<XRGrabInteractable>().selectExited.AddListener( OnGrabEnd);
        transform.GetComponent<Rigidbody>().isKinematic = true;
        Grabs = transform.GetComponent<XRGrabInteractable>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnGrab()
    {

    }

    void  OnGrabEnd(UnityEngine.XR.Interaction.Toolkit.SelectExitEventArgs hm)
    {

        if (CalculateCurrentDifferenceRot())
        {
            Grabs.trackRotation = false;
            transform.localRotation = Quaternion.Euler(TargetRot);
        }
        if(Vector3.Distance(TargetPoint,transform.position) < DistanceAllowance)
        {
            transform.position = TargetPoint;
        }
    } 

    public bool CalculateCurrentDifferenceRot()
    {
        if (!DifferingAllowances)
        {
           return AllowedOffset < Quaternion.Angle(transform.rotation, Quaternion.Euler(TargetRot));
        }
        Vector3 difference = transform.localRotation.eulerAngles - TargetRot;
        return Mathf.Abs(difference.x) < DifferentAllowances.x && Mathf.Abs(difference.y) < DifferentAllowances.y && Mathf.Abs(DifferentAllowances.z) < DifferentAllowances.z;
     }

    public void Unlock()
    {
        Grabs.trackRotation = true;
    }
}
