using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using static Functions;
using static XRGrabSetupLibrary;
using UnityEngine.InputSystem.HID;

public class GrababbleRuled : MonoBehaviour
{
    [SerializeField]
    FunctionName Function_1;

    [SerializeField]
    bool Negative1;

    [SerializeField, Range(0, 100)]
    float Scale1;

    [SerializeField, Range(0, 100)]
    float timesThrough1;

    [SerializeField]
    Vector3 Rotation1 = Vector3.zero;

    [SerializeField]
    FunctionName Function_2;

    [SerializeField]
    bool Negative2;

    [SerializeField, Range(0, 100)]
    float Scale2;

    [SerializeField, Range(0, 100)]
    float timesThrough2;

    [SerializeField]
    int NumPoints;

    [SerializeField]
    int OutsidePoints;

    [SerializeField]
    float OutsideDiameter;

    [SerializeField]
    Vector3 Rotation2 = Vector3.zero;


    [SerializeField]
    Vector3 offset;

    [SerializeField]
    Transform cylinderPrefab;

    [SerializeField]
    Transform pointPrefab;

    [SerializeField]
    Transform OutsidePrefab;

    [SerializeField, Range(0, 1)]
    float threadRadius;

    [SerializeField, Range(0, 1)]
    float pointSize;

    [SerializeField]
    bool showPoints;
    //to detect if any changes have been made
    private string serialNo;

    private string outsideSerial1;
    private string outsideSerial2;

    private Transform child1;

    private Transform child2;

    private Transform[] child1Skeleton = new Transform[0];
    
    private Transform[] child2Skeleton = new Transform[0];
    
    private Vector_Array surfaceGenerator;
    // Start is called before the first frame update
    void Start()
    {
        XrGrabSetup(transform);
        transform.GetComponent<Rigidbody>().useGravity = false;
        transform.GetComponent<Rigidbody>().isKinematic = true;

        //creating the first outside arch (director line) child
        child1 = new GameObject().transform;
        child1.SetParent(transform);
        XrGrabSetup(child1, true);
        child1.GetComponent<Rigidbody>().useGravity = false;
        child1.GetComponent<Rigidbody>().isKinematic = true;
        child1.localPosition = Vector3.zero;
        child1.localRotation = Quaternion.Euler(Rotation1);


        //creating the second outside arch (director line) child
        child2 = new GameObject().transform;
        child2.SetParent(transform);
        XrGrabSetup(child2, true);
        child2.GetComponent<Rigidbody>().useGravity = false;
        child2.GetComponent<Rigidbody>().isKinematic = true;
        child2.localPosition = offset;
        child2.localRotation = Quaternion.Euler(Rotation2);

        //Creating the ruling generator and feeding it the nescessary starting information
        surfaceGenerator = transform.AddComponent<Vector_Array>();
        surfaceGenerator.pointPrefab = pointPrefab;
        surfaceGenerator.pointSize = pointSize;
        surfaceGenerator.cylinderPrefab = cylinderPrefab;
        surfaceGenerator.threadRadius = threadRadius;
        surfaceGenerator.manual_Input = false;
        surfaceGenerator.NumPoints = NumPoints;
        surfaceGenerator.offset1 = Vector3.zero;
        surfaceGenerator.offset2 = offset;
        surfaceGenerator.Rotation1 = Rotation1;
        surfaceGenerator.Rotation2 = Rotation2;
        surfaceGenerator.Function_1 = Function_1;
        surfaceGenerator.Function_2 = Function_2;
        surfaceGenerator.Negative1 = Negative1;
        surfaceGenerator.Negative2 = Negative2;
        surfaceGenerator.Scale1 = Scale1;
        surfaceGenerator.Scale2 = Scale2;
        surfaceGenerator.timesThrough1 = timesThrough1;
        surfaceGenerator.timesThrough2 = timesThrough2;
        surfaceGenerator.XrGrabParent = true;
        surfaceGenerator.ShowPoints = showPoints;

        //initializing the serials to avoid unescessary recalculation
        serialNo = retSerial();
        outsideSerial1 = retSerial(true);
        outsideSerial2 = retSerial(false);

        //finding the functions the user has given us
        Function firstFunc = GetFunction(Function_1);
        Function secondFunc = GetFunction(Function_2);

        //creating the director curves
        outSideArchRender(firstFunc, 1, true);
        outSideArchRender(secondFunc, 2, true);
    }


    // Update is called once per frame
    void Update()
    {
        //checking if any changes have been made
        string newSerial = retSerial();
        if (newSerial == serialNo)
        {
            //if no changes have been made
            return;
        }
        serialNo = newSerial;

        //if something has updated that would affect how the outside arches need to be rendered re render them
        if(retSerial(true) != outsideSerial1)
        {
            outSideArchRender(GetFunction(Function_1), 1);
        }

        if (retSerial(false) != outsideSerial2)
        {
            outSideArchRender(GetFunction(Function_2), 2);
        }

        //passing needed information to the ruling maker
        surfaceGenerator.threadRadius = threadRadius;
        surfaceGenerator.manual_Input = false;
        surfaceGenerator.NumPoints = NumPoints;
        surfaceGenerator.offset1 = Quaternion.Inverse(transform.localRotation) * (child1.position - transform.position);
        surfaceGenerator.offset2 = Quaternion.Inverse(transform.localRotation) * (child2.position - transform.position);
        //still need to fix rotation, should probably manually calculate the function to do so
        surfaceGenerator.Rotation1 = (Quaternion.Inverse(transform.rotation) * child1.rotation).eulerAngles; // - (child1.rotation * transform.rotation).eulerAngles;
        surfaceGenerator.Rotation2 = (Quaternion.Inverse(transform.rotation) * child2.rotation).eulerAngles; // - (child2.rotation * transform.rotation).eulerAngles;
        surfaceGenerator.Function_1 = Function_1;
        surfaceGenerator.Function_2 = Function_2;
        surfaceGenerator.Scale1 = Scale1;
        surfaceGenerator.Scale2 = Scale2;
        surfaceGenerator.Negative1 = Negative1;
        surfaceGenerator.Negative2 = Negative2;
        surfaceGenerator.timesThrough1 = timesThrough1;
        surfaceGenerator.timesThrough2 = timesThrough2;
        

    }

    private string retSerial()
    {
        return "" + Function_1 + Negative1 + Scale1 + timesThrough1 + child1.localRotation + Function_2 + Negative2 + Scale2 + timesThrough2 + child2.localRotation + NumPoints + child1.localPosition + child2.localPosition + cylinderPrefab + pointPrefab + threadRadius + OutsidePoints;
    }

    private string retSerial(bool which)
    {
        return which ? "" + Function_1 + Negative1 + Scale1 + timesThrough1 + OutsidePoints : "" + Function_2 + Negative2 + Scale2 + timesThrough2 + OutsidePoints;
    }

    

    private void outSideArchRender(Transform child, Transform[] skeleton, Vector3[] points, int which)
    {

        //finding the length of the array before we potentially resize it
        int oldLength = which == 1 ? child1Skeleton.Length : child2Skeleton.Length;

        //Seeing if resizing is needed
        if (oldLength != OutsidePoints)
        {
            //find what xr component we are adding colliders to
            XRGrabInteractable childBoxes = child.GetComponent<XRGrabInteractable>();
            XRGrabInteractable wholeBoxes = transform.GetComponent<XRGrabInteractable>();
            //if the old array is too small
            if (OutsidePoints > oldLength)

            {
                //add more space
                //Array.Resize(ref skeleton, OutsidePoints);
                if (which == 1)
                {
                    Array.Resize(ref child1Skeleton, OutsidePoints);
                }
                else
                {
                    Array.Resize(ref child2Skeleton, OutsidePoints);
                }
                //create new points to fill the empty space in the array
                for (int i = oldLength; i + 1 < OutsidePoints; i++)
                {
                    //skeleton[i] = Instantiate(OutsidePrefab, child);
                    if (which == 1)
                    {
                        child1Skeleton[i] = Instantiate(OutsidePrefab, child);
                        childBoxes.colliders.Add(child1Skeleton[i].GetComponent<Collider>());
                        //wholeBoxes.colliders.Add(child1Skeleton[i].GetComponent<Collider>());
                    }
                    else
                    {
                        child2Skeleton[i] = Instantiate(OutsidePrefab, child);
                        childBoxes.colliders.Add(child2Skeleton[i].GetComponent<Collider>());
                       // wholeBoxes.colliders.Add(child2Skeleton[i].GetComponent<Collider>());
                    }

                }
            }
                
            //if the old array is too large
            if(OutsidePoints < oldLength)
            {
                //delete the points that will no longer be used
                for(int i = OutsidePoints; i + 1 < oldLength; i++)
                {
                    if (which == 1)
                    {
                        childBoxes.colliders.Remove(child1Skeleton[i].GetComponent<Collider>());
                       // wholeBoxes.colliders.Remove(child1Skeleton[i].GetComponent<Collider>());
                        Destroy(child1Skeleton[i].gameObject);
                    }
                    else
                    {
                        childBoxes.colliders.Remove(child2Skeleton[i].GetComponent<Collider>());
                       // wholeBoxes.colliders.Remove(child2Skeleton[i].GetComponent<Collider>());
                        Destroy(child2Skeleton[i].gameObject);
                    }

                }
                //shrink the array
                //Array.Resize(ref skeleton, OutsidePoints);
                if (which == 1)
                {
                    Array.Resize(ref child1Skeleton, OutsidePoints);
                }
                else
                {
                    Array.Resize(ref child2Skeleton, OutsidePoints);
                }
            }

                //unrigestering and re registering the colliders for the xr grabbable script
                childBoxes.interactionManager.UnregisterInteractable(childBoxes.GetComponent<IXRInteractable>());
                childBoxes.interactionManager.RegisterInteractable(childBoxes.GetComponent<IXRInteractable>());
                //wholeBoxes.interactionManager.UnregisterInteractable(childBoxes.GetComponent<IXRInteractable>());
               // wholeBoxes.interactionManager.RegisterInteractable(childBoxes.GetComponent<IXRInteractable>());
        }
        

        //calculating where the points should be
       

        for(int i = 0; i + 1< OutsidePoints; i++)
        {
            Transform nextOutside = which == 1 ? child1Skeleton[i] : child2Skeleton[i];
            nextOutside.localPosition = points[i] + (points[i + 1] - points[i])/2;
            nextOutside.localScale = new Vector3(OutsideDiameter, Vector3.Distance(points[i], points[i + 1])/2, OutsideDiameter);
            nextOutside.localRotation = Quaternion.LookRotation(points[i + 1], points[i]);
            
        }

        

        

    }

    private void outSideArchRender(Function func, int which, bool init)
    {

        Transform child = which == 1 ? child1 : child2;
        Vector3[] points = which == 1 ? func(OutsidePoints, Scale1, Negative1, timesThrough1, Vector3.zero, Quaternion.Euler(Rotation1)) : func(OutsidePoints, Scale2, Negative2, timesThrough2, Vector3.zero, Quaternion.Euler(Rotation2));

        //fixing the position of the second curve (needs to account for the offset given)
        if (which == 2 && init)
        {
            child2.localPosition += offset;
        }

        outSideArchRender(child, which == 1 ? child1Skeleton : child2Skeleton, points, which);
    }

    private void outSideArchRender(Function func, int which)
    {
        outSideArchRender(func, which, false);
    }
}
