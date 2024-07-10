using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class DevelopableSurfaceIntersector : MonoBehaviour
{
    [SerializeField]
    public Transform PointLinePrefab;

    [SerializeField]
    public float Thickness;

    [SerializeField]
    public string tagToIntersect = "Ruling";

    [SerializeField]
    public Color HighlightColor;

    [SerializeField]
    public float extraRadius;

    [SerializeField]
    public float AllowedDegreeOffset;
    private Dictionary<Transform, Transform> intersectingRulings;
    private string serial;
    private Color oldColorMA;
    private bool oldColorFound = false;
    //for debugging
    //private Transform putThatThingBackWhereItCameFromOrSoHelpMe;

    // Start is called before the first frame update
    void Start()
    {
        intersectingRulings = new Dictionary<Transform, Transform>();
        PointLinePrefab.GetComponent<Renderer>().material.color = HighlightColor;

    }

    // Update is called once per frame
    void Update()
    {

        Plane planeForEachIntersection = new Plane(transform.rotation * Vector3.up, transform.position);

        foreach(KeyValuePair<Transform, Transform> pair in intersectingRulings)
        {
            Vector3 RayNormals = pair.Key.rotation * Vector3.up; 
            Ray rayToFindIntersectionPoint = new Ray(pair.Key.position, RayNormals);
            float distanceFromCenter;
            planeForEachIntersection.Raycast(rayToFindIntersectionPoint, out distanceFromCenter);
            Vector3 positionForIntersection = rayToFindIntersectionPoint.GetPoint(distanceFromCenter);

            if (CheckIfParallel(rayToFindIntersectionPoint))
            {
                Material WorkingWith = pair.Key.GetComponent<Renderer>().material;
                WorkingWith.color = HighlightColor;
                pair.Value.GetComponent<Renderer>().enabled = false;
            } else
            {
                pair.Value.GetComponent<Renderer>().enabled = true;
                pair.Value.position = positionForIntersection;
                pair.Value.rotation = transform.rotation;
                pair.Key.GetComponent<Renderer>().material.color = oldColorMA;


                Ray rayForXScale = new Ray(pair.Key.position +  pair.Key.rotation * new Vector3(pair.Key.localScale.x/2, 0, 0), RayNormals);
                planeForEachIntersection.Raycast(rayForXScale, out float distanceFromX);
                float XScale = Vector3.Distance(positionForIntersection, rayForXScale.GetPoint(distanceFromX));

                Ray rayForZScale = new Ray(pair.Key.position + pair.Key.rotation * new Vector3(0, 0, pair.Key.localScale.z / 2), RayNormals);
                planeForEachIntersection.Raycast(rayForZScale, out float distanceFromZ);
                float ZScale = Vector3.Distance(positionForIntersection, rayForZScale.GetPoint(distanceFromZ));

                pair.Value.localScale = new Vector3(XScale * 2 + extraRadius, Thickness, ZScale * 2 + extraRadius);
            }
            
            //Pair.Value
            }
    }

    private bool CheckIfParallel(Ray line)
    {
        return (Mathf.Abs(90 - Vector3.Angle(transform.rotation * Vector3.up, line.direction)) < AllowedDegreeOffset);
    }
    private void OnTriggerEnter(Collider other)
    {

        if(other.transform.tag == tagToIntersect)
        {
            Transform newPoint = Instantiate(PointLinePrefab, transform);
            //figure out other setup stuff
            newPoint.localScale = new Vector3 (other.transform.localScale.x + extraRadius, Thickness, other.transform.localScale.z + extraRadius);
            newPoint.GetComponent<Renderer>().material.color = HighlightColor;
            intersectingRulings.Add(other.transform, newPoint);
            if (!oldColorFound)
            {
                Debug.Log("Changing Color");
                oldColorMA = other.GetComponent<Renderer>().material.color;
                oldColorFound = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (intersectingRulings.ContainsKey(other.transform))
        {
            Transform pointValue;
            intersectingRulings.TryGetValue(other.transform, out pointValue);
            Destroy(pointValue.gameObject);
            intersectingRulings.Remove(other.transform);
            other.GetComponent<Renderer>().material.color = oldColorMA;
        }
    }
}
