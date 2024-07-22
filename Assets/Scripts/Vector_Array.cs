using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Mathf;
using static Functions;
using UnityEngine.UIElements;
using Unity.VisualScripting;
using System;
using System.Data;
using UnityEngine.XR.Interaction.Toolkit;

public class Vector_Array : MonoBehaviour
{
    [SerializeField]
    public bool manual_Input;

    [SerializeField]
    public FunctionName Function_1;

    [SerializeField]
    public bool Negative1;

    [SerializeField, Range(0, 100)]
    public float Scale1;

    [SerializeField, Range(0, 100)]
    public float timesThrough1;

    [SerializeField]
    public Vector3 Rotation1;

    public Vector3 offset1;

    [SerializeField]
    public FunctionName Function_2;

    [SerializeField]
    public bool Negative2;

    [SerializeField, Range(0,100)]
    public float Scale2;

    [SerializeField, Range(0, 100)]
    public float timesThrough2;

    [SerializeField] 
    public int NumPoints;

    [SerializeField]
    public Vector3 Rotation2;
    
    
    [SerializeField]
    public Vector3 offset2;

    [SerializeField]
    public Transform cylinderPrefab;

    [SerializeField]
    public Transform pointPrefab;

    [SerializeField]
    public Transform planePrefab;

    [SerializeField, Range(0,1)]
    public float threadRadius;

    [SerializeField, Range(0,1)]
    public float pointSize;

    [SerializeField]
    public Vector3[] firstPoints;

    [SerializeField]
    public Vector3[] secondPoints;

    [SerializeField]
    public bool XrGrabParent;

    [SerializeField]
    public string tagForRulings = "Ruling";

    [SerializeField]
    public bool ShowPoints;

    private Transform[] points;

    private Transform[] lines;

    private int MaxLength;

    private string serialNo;

    private string func1Serial;

    private string func2Serial;
    // Start is called before the first frame update
    void Start()
    {
        if(!manual_Input)
        {
            Function first = GetFunction(Function_1);
            firstPoints = first(NumPoints, Scale1, Negative1, timesThrough1, offset1, Quaternion.Euler(Rotation1));
            Function second = GetFunction(Function_2);
            secondPoints = second(NumPoints, Scale2, Negative2, timesThrough2, offset2, Quaternion.Euler(Rotation2));
        }

        serialNo = retSerial();
        func1Serial = retSerial(true);
        func2Serial = retSerial(false);
        //using the smaller of the lengths of each list to ensure there are two points for every line
        MaxLength = firstPoints.Length < secondPoints.Length ? firstPoints.Length : secondPoints.Length;

        if (ShowPoints)
        {
            points = new Transform[MaxLength * 2];
        }
        lines = new Transform[MaxLength];
        //scaling for the points (cubes)
        var newscale = Vector3.one * pointSize;

        //a scaling vector utilize by the lines
        var scale = Vector3.one;

        //changing the thickness of each line
        scale.x = scale.z = threadRadius;
        //
        XRGrabInteractable addingTo = transform.GetComponent<XRGrabInteractable>();
        if (transform.GetComponent<XRGrabInteractable>() == null)
        {
            XrGrabParent = false;
        }

        for (int i = 0; i < MaxLength; i++)
        {
            //Grabbing the points from each of the lists
            Vector3 first = firstPoints[i];
            Vector3 second = secondPoints[i];   

            //Creating the line and two ending points
            Transform cyl = Instantiate(cylinderPrefab);
            if (ShowPoints)
            {
                Transform point1 = Instantiate(pointPrefab);
                Transform point2 = Instantiate(pointPrefab);

                points[2 * i] = point1;
                points[(2 * i) + 1] = point2;

                //Setting the parent of each point
                point1.SetParent(transform);
                point2.SetParent(transform);

                //Setting the position of each point
                point1.localPosition = first;
                point2.localPosition = second;
                point1.localScale = newscale;
                point2.localScale = newscale;
            }
            cyl.tag = "Ruling";
            lines[i] = cyl;
            
            //Changing the length of each line (treating the y scale as the distance)
            scale.y = Vector3.Distance(first, second)/2;

            //Calculating the needed rotation for the line to point from point 1 to point 2
            Vector3 targetDir = first - second;
            Quaternion lookDir = Quaternion.LookRotation(targetDir);

            //setting the line's scale, parent, rotational direction
            cyl.localScale = scale;
            cyl.SetParent(transform);
            cyl.localRotation = lookDir;

            //fixing the rotational direction (x was off by 90 degrees, need to look into)
            cyl.localEulerAngles = new Vector3(cyl.localEulerAngles.x + 90, cyl.localEulerAngles.y, cyl.localEulerAngles.z);
            //
            cyl.localPosition = second - (second - first)/2;

            if (XrGrabParent)
            {
                addingTo.colliders.Add(cyl.GetComponent<Collider>());
            } //if GrabParent
        }//for maxLength
        if(XrGrabParent)
        {
            addingTo.interactionManager.UnregisterInteractable(addingTo.GetComponent<IXRInteractable>());
            addingTo.interactionManager.RegisterInteractable(addingTo.GetComponent<IXRInteractable>());
        }
    }//Start

    // Update is called once per frame
    void Update()
    {


        string newser = retSerial();
        if (serialNo == newser)
        { 
            return;
        }
        Debug.Log("Recalculating");
        serialNo = newser;
        
        if (!manual_Input)
        {
            string newFirstSer = retSerial(true);
            if (newFirstSer != func1Serial)
            {
                Function first = GetFunction(Function_1);
                firstPoints = first(NumPoints, Scale1, Negative1, timesThrough1, offset1, Quaternion.Euler(Rotation1));
                func1Serial = newFirstSer;
            }
            string newSecondSer = retSerial(false);
            if (newSecondSer != func2Serial)
            {
                Function second = GetFunction(Function_2);
                secondPoints = second(NumPoints, Scale2, Negative2, timesThrough2, offset2, Quaternion.Euler(Rotation2));
                func2Serial = newSecondSer;
            }
        }

        if (transform.GetComponent<XRGrabInteractable>() == null)
        {
            Debug.Log("Invalid XRGrab");
            XrGrabParent = false;
        } 


        MaxLength = firstPoints.Length < secondPoints.Length ? firstPoints.Length : secondPoints.Length;

        var scale = Vector3.one;
        var newscale = Vector3.one * pointSize;

        //changing the thickness of each line
        scale.x = scale.z = threadRadius;

        int oldLength = lines.Length;
        if(NumPoints > oldLength)
        {
            Array.Resize(ref lines, NumPoints);
            if (ShowPoints)
            {
                Array.Resize(ref points, 2 * NumPoints);
            }
            XRGrabInteractable addingTo = transform.GetComponent<XRGrabInteractable>();
            for (int i = oldLength; i < NumPoints; i++)
            {
                lines[i] = Instantiate(cylinderPrefab, transform);
                lines[i].tag = tagForRulings;
                if (ShowPoints)
                {
                    points[2 * i] = Instantiate(pointPrefab, transform);
                    points[2 * i + 1] = Instantiate(pointPrefab, transform);
                }
                
                if (XrGrabParent)
                {
                    addingTo.colliders.Add(lines[i].GetComponent<Collider>());
                }
            }
            if (XrGrabParent)
            {
                addingTo.interactionManager.UnregisterInteractable(addingTo.GetComponent<IXRInteractable>());
                addingTo.interactionManager.RegisterInteractable(addingTo.GetComponent<IXRInteractable>());
            }
        } 
        if (NumPoints < lines.Length)
        {
            for(int i = NumPoints; i < oldLength; i++)
            {
                Destroy(lines[i].gameObject);
                if (ShowPoints)
                {
                    Destroy(points[2 * i].gameObject);
                    Destroy(points[2 * i + 1].gameObject);
                }
            }
            Array.Resize(ref lines, NumPoints);
            if (ShowPoints)
            {
                Array.Resize(ref points, 2 * NumPoints);
            }
        }
        for (int i = 0; i < MaxLength; i++)
            {
                //Grabbing the points from each of the lists
                Vector3 first = firstPoints[i];
                Vector3 second = secondPoints[i];

                Transform cyl = lines[i];



            //Changing the length of each line (treating the y scale as the distance)
            scale.y = Vector3.Distance(first, second) / 2;

                //Calculating the needed rotation for the line to point from point 1 to point 2
                Vector3 targetDir = first - second;
                Quaternion shift = Quaternion.Euler(new Vector3(90, 0, 0));
                Quaternion lookDir = Quaternion.LookRotation(targetDir) * shift;

            //setting the line's scale, parent, rotational direction
                cyl.localScale = scale;
                cyl.localRotation = lookDir;
                
                //
                cyl.localPosition = second - (second - first) / 2;

            if (ShowPoints)
            {
                Transform point1 = points[2 * i];
                Transform point2 = points[(2 * i) + 1];

                //Setting the position of each point
                point1.localPosition = first;
                point2.localPosition = second;
                point1.localScale = newscale;
                point2.localScale = newscale;
                point1.localRotation = lookDir;
                point2.localRotation = lookDir;

            }

        }
    }
    
    public Color getRulingColor()
    {
        return cylinderPrefab.GetComponent<Renderer>().sharedMaterial.color;
    }
    public void changeRulingColor(Color changeTo)
    {
        foreach (Transform t in lines)
        {
            Debug.Log("Changing Color");
            t.GetComponent<Renderer>().material.color = changeTo;
        }
    }
    private string retSerial()
    {
        return manual_Input ? "" + cylinderPrefab + pointPrefab + threadRadius + OtherFuncs.ReturnArray(firstPoints) + OtherFuncs.ReturnArray(secondPoints) : 
            "" + Function_1 + Negative1 + Scale1 + timesThrough1 + Rotation1 + Function_2 + Negative2 + Scale2 + timesThrough2 + Rotation2 + NumPoints + offset1 + offset2 + cylinderPrefab + pointPrefab + threadRadius;
    }

    private string retSerial(bool first)
    {
        return first ? "" + Function_1 + NumPoints + Scale1 + Negative1 + timesThrough1 + offset1 + Rotation1: "" + Function_2 + NumPoints + Scale2 + Negative2 + timesThrough2 + offset2 + Rotation2;
    }
}
