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

    private Dictionary<Transform, Transform> intersectingRulings;
    private string serial;
    private Color oldColorMA;
    //for debugging
    //private Transform putThatThingBackWhereItCameFromOrSoHelpMe;

    // Start is called before the first frame update
    void Start()
    {
        intersectingRulings = new Dictionary<Transform, Transform>();

    }

    // Update is called once per frame
    void Update()
    {

        Plane planeForEachIntersection = new Plane(transform.rotation * Vector3.up, transform.position);

        foreach(KeyValuePair<Transform, Transform> pair in intersectingRulings)
        {
            Ray rayToFindIntersectionPoint = new Ray(pair.Key.position, pair.Key.rotation * Vector3.up);
            float distanceFromCenter;
            planeForEachIntersection.Raycast(rayToFindIntersectionPoint, out distanceFromCenter);
            Vector3 positionForIntersection = rayToFindIntersectionPoint.GetPoint(distanceFromCenter);

            if (false)
            {
                Material WorkingWith = pair.Key.GetComponent<Material>();
                oldColorMA = WorkingWith.color;
                WorkingWith.color = HighlightColor;
                pair.Value.GetComponent<MeshRenderer>().enabled = false;
            } else
            {
                pair.Value.GetComponent<MeshRenderer>().enabled = true;
                pair.Value.position = positionForIntersection;
                pair.Value.rotation = transform.rotation;
               // pair.Key.GetComponent<Material>().color = oldColorMA;
            }
            
            //Pair.Value
            }
    }

    private void OnTriggerEnter(Collider other)
    {

        if(other.transform.tag == tagToIntersect)
        {
            Transform newPoint = Instantiate(PointLinePrefab, transform);
            //figure out other setup stuff
            newPoint.localScale = new Vector3 (other.transform.localScale.x * (float) 1.1, Thickness, other.transform.localScale.z * (float) 1.1);
            intersectingRulings.Add(other.transform, newPoint);
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
        }
    }
}
