using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

/* TODO:
 * Utilize a serial to avoid rerendering, perhaps by storing the grabbed ruled surfaces in an array
 * 
 */
public class DevelopableSurfaceIntersector : MonoBehaviour
{
    //for the highlight
    [SerializeField]
    public Transform PointLinePrefab;

    //how thick the highlight is (y thickness)
    [SerializeField]
    public float Thickness;

    //What tag to make highlights with intersections
    [SerializeField]
    public string tagToIntersect = "Ruling";

    //What color the created game objects should be
    [SerializeField]
    public Color HighlightColor;

    //how much extra diameter to have each created gameobject to have in relation to the line that is being 'highlighted'
    [SerializeField]
    public float extraDiameter;

    //the allowed degree discrepency between the plane and a ruling to be considered parallel
    [SerializeField]
    public float AllowedDegreeOffset;

    //Dict to store all intersecting rulings and the highlighting gameobject
    private Dictionary<Transform, Transform> intersectingRulings;

    //Currently unused but could use to make sure rerendering is not burdensome (currently it isn't, but may want to make it even less so)
    private string serial;

    //color that the rulings were before they became highlighted, currently only supports one color per intersector
    private Color oldColorMA;
    //if the aformentioned color has been filled, fills when the first valid intersection happens
    private bool oldColorFound = false;

    // Start is called before the first frame update
    void Start()
    {
        //creating the dictonary
        intersectingRulings = new Dictionary<Transform, Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        //Creating a mathematical representation of the developable surface intersector
        Plane planeForEachIntersection = new Plane(transform.rotation * Vector3.up, transform.position);

        //for every intersecting ruling (pair of rulings and highlight object)
        foreach(KeyValuePair<Transform, Transform> pair in intersectingRulings)
        {
            //create the 'normal' vector for each ray traveling in the direction of the intersecting ruling
            Vector3 RayNormals = pair.Key.rotation * Vector3.up; 
            //the middle of the rulings' line
            Ray rayToFindIntersectionPoint = new Ray(pair.Key.position, RayNormals);

            //how far the line is from the plane (the center of the ruling that is)
            float distanceFromCenter;
            //calculating this distance
            planeForEachIntersection.Raycast(rayToFindIntersectionPoint, out distanceFromCenter);
            //extrapolating a vector from this distance (finding the point the middle of the line and the plane would/do intersect)
            Vector3 positionForIntersection = rayToFindIntersectionPoint.GetPoint(distanceFromCenter);

            //if the ruling is within the allowed parallel discrepency
            if (CheckIfParallel(rayToFindIntersectionPoint))
            {
                Material WorkingWith = pair.Key.GetComponent<Renderer>().material;
                //highlight the entire ruling
                WorkingWith.color = HighlightColor;
                //don't show the highlighting game object
                pair.Value.GetComponent<Renderer>().enabled = false;

              //if not parallel
            } else {
                //make sure the highlight is visible
                pair.Value.GetComponent<Renderer>().enabled = true;
                //position it at the intersection of the ruling and the plane
                pair.Value.position = positionForIntersection;
                //match the rotation to the plane's
                pair.Value.rotation = transform.rotation;
                //make sure the intersecting ruling has it's (the) original color
                pair.Key.GetComponent<Renderer>().material.color = oldColorMA;

                //calculating new rays for X and Z scale
                Ray rayForXScale = new Ray(pair.Key.position +  pair.Key.rotation * new Vector3(pair.Key.localScale.x/2, 0, 0), RayNormals);
                //same process as the center line
                planeForEachIntersection.Raycast(rayForXScale, out float distanceFromX);
                float XScale = Vector3.Distance(positionForIntersection, rayForXScale.GetPoint(distanceFromX));

                Ray rayForZScale = new Ray(pair.Key.position + pair.Key.rotation * new Vector3(0, 0, pair.Key.localScale.z / 2), RayNormals);
                planeForEachIntersection.Raycast(rayForZScale, out float distanceFromZ);
                float ZScale = Vector3.Distance(positionForIntersection, rayForZScale.GetPoint(distanceFromZ));

                //Setting the scale of the highlight to account for differing cross-sections to differences in rotation
                pair.Value.localScale = new Vector3(XScale * 2 + extraDiameter, Thickness, ZScale * 2 + extraDiameter);
            }
         }
    }

    private bool CheckIfParallel(Ray line)
    {
        //is the line's angle to the plane's normal vector within the allowed discrepency (90 degrees means the plane and the line are perfectly parallel)
        return (Mathf.Abs(90 - Vector3.Angle(transform.rotation * Vector3.up, line.direction)) < AllowedDegreeOffset);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == tagToIntersect)
        {
            Transform newPoint = Instantiate(PointLinePrefab, transform);
            newPoint.localScale = new Vector3 (other.transform.localScale.x + extraDiameter, Thickness, other.transform.localScale.z + extraDiameter);
            //making sure the highlight has the correct color
            newPoint.GetComponent<Renderer>().material.color = HighlightColor;
            //adding the ruling highlight pair to the dictonary
            intersectingRulings.Add(other.transform, newPoint);
            //if we have not found an original color yet
            if (!oldColorFound)
            {
                //setting the original color to the intersecting ruling's color
                oldColorMA = other.GetComponent<Renderer>().material.color;
                //marking that we have found an (the) original color
                oldColorFound = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (intersectingRulings.ContainsKey(other.transform))
        {
            //The highlight corresponding to the leaving ruling
            Transform pointValue;
            //Grabbing said gameobject from the dictonary
            intersectingRulings.TryGetValue(other.transform, out pointValue);
            //Destroying the highlight
            Destroy(pointValue.gameObject);
            //removing the ruling highlight pair from the dictonary
            intersectingRulings.Remove(other.transform);
            //assuring the ruling's color is *the* original color
            other.GetComponent<Renderer>().material.color = oldColorMA;
        }
    }
}
