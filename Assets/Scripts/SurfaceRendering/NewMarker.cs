using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class NewMarker : MonoBehaviour
{

    public GameObject markerPrefab;
    public GameObject colliderPrefab;
    public float muSquared = 2.0f;
    public Vector3 localScale = new Vector3(0.04f, 0.04f, 0.04f);
    private GameObject marker;
    private GameObject collide;
    public int count = 0;
    List<GameObject> markers;
    List<GameObject> colliders;
    public int function;


    void Start()
    {
        // float offsetVal = (GridMetrics.PointsPerChunk / 2 - 0.5f) * (size / 100f);
        markers = new List<GameObject>();
        colliders = new List<GameObject>();
    }

    public void CreateSingularityMarkers()
    {
        // Calculate the coordinates of the singularities
        Vector3[] singularityPoints = CalculateSingularityPoints(muSquared);

        foreach (Vector3 point in singularityPoints)
        {
               Vector3 localPoint = Vector3.Scale(point, localScale);
            CreateMarker(localPoint);
        }
    }

    public void CreateMarker(Vector3 pos)
    {
        marker = Instantiate(markerPrefab, transform.position + pos, Quaternion.identity, transform);
        collide = Instantiate(colliderPrefab, transform.position + pos, Quaternion.identity, transform);
        markers.Add(marker);
        markers[0].SetActive(false);
        colliders.Add(collide);
        
        count++;
    }

    public void RevealMarkers()
    {
        for(int i = 0; i < count; i++)
        {
            colliders[i].SetActive(false);
            markers[i].SetActive(true);
        }
    }

    public void DestroyMarkers()
    {
        int ct = count;

        for (int i = 0; i < ct; i++)
        {
            Destroy(markers[i]);
            Destroy(colliders[i]);
        }
        markers.Clear();
        colliders.Clear();
        count = 0;
    }





    private Vector3[] CalculateSingularityPoints(float muSquared)
    {
        /*float sqrt2 = Mathf.Sqrt(2);
        float sqrtMinus1PlusMuSquared = Mathf.Sqrt(-1 + muSquared);
        float oneOverSqrt2 = 1.0f / sqrt2;*/
        Vector3[] singularityPoints = new Vector3[] { };

        if (function == 1)
        {
            Vector3[] singularity = { new Vector3(0, -1, 0) };
            singularityPoints = singularity;
        }
        else if (function == 2)
        {
            Vector3[] singularity = { new Vector3(0.33f, 0, 0.33f) };
            singularityPoints = singularity;
        }
        else
        {
            Vector3[] singularity = { new Vector3(0, 0, 0) };
            singularityPoints = singularity;

          /*
          new Vector3(sqrtMinus1PlusMuSquared,-1, 0 ),
          new Vector3(-sqrtMinus1PlusMuSquared,-1, 0 ),
          new Vector3(0,1, -sqrtMinus1PlusMuSquared),
          new Vector3(0, sqrtMinus1PlusMuSquared, 1),
          new Vector3(-oneOverSqrt2 * (-1 + muSquared),  0.5f * (1 - muSquared),0),
          new Vector3(oneOverSqrt2 * (-1 + muSquared), 0.5f * (1 - muSquared), 0),


          new Vector3(-0.5f * Mathf.Sqrt(1+muSquared + 2*sqrt2*sqrtMinus1PlusMuSquared),  -oneOverSqrt2 * sqrtMinus1PlusMuSquared, -0.5f * Mathf.Sqrt(1+muSquared - 2*sqrt2*sqrtMinus1PlusMuSquared)),


          new Vector3(-0.5f * Mathf.Sqrt(1+muSquared + 2*sqrt2*sqrtMinus1PlusMuSquared),  -oneOverSqrt2 * sqrtMinus1PlusMuSquared, 0.5f * Mathf.Sqrt(1+muSquared - 2*sqrt2*sqrtMinus1PlusMuSquared)),


          new Vector3(0.5f * Mathf.Sqrt(1+muSquared + 2*sqrt2*sqrtMinus1PlusMuSquared),  -oneOverSqrt2 * sqrtMinus1PlusMuSquared,-0.5f * Mathf.Sqrt(1+muSquared - 2*sqrt2*sqrtMinus1PlusMuSquared)),


          new Vector3(0.5f * Mathf.Sqrt(1+muSquared + 2*sqrt2*sqrtMinus1PlusMuSquared), -oneOverSqrt2 * sqrtMinus1PlusMuSquared, 0.5f * Mathf.Sqrt(1+muSquared - 2*sqrt2*sqrtMinus1PlusMuSquared)),


          new Vector3(-0.5f * Mathf.Sqrt(1+muSquared - 2*sqrt2*sqrtMinus1PlusMuSquared), oneOverSqrt2 * sqrtMinus1PlusMuSquared, -0.5f * Mathf.Sqrt(1+muSquared + 2*sqrt2*sqrtMinus1PlusMuSquared)),


          new Vector3(-0.5f * Mathf.Sqrt(1+muSquared - 2*sqrt2*sqrtMinus1PlusMuSquared), oneOverSqrt2 * sqrtMinus1PlusMuSquared, 0.5f * Mathf.Sqrt(1+muSquared + 2*sqrt2*sqrtMinus1PlusMuSquared)),


          new Vector3(0.5f * Mathf.Sqrt(1+muSquared - 2*sqrt2*sqrtMinus1PlusMuSquared), oneOverSqrt2 * sqrtMinus1PlusMuSquared, -0.5f * Mathf.Sqrt(1+muSquared + 2*sqrt2*sqrtMinus1PlusMuSquared)),


          new Vector3(0.5f * Mathf.Sqrt(1+muSquared - 2*sqrt2*sqrtMinus1PlusMuSquared), oneOverSqrt2 * sqrtMinus1PlusMuSquared, 0.5f * Mathf.Sqrt(1+muSquared + 2*sqrt2*sqrtMinus1PlusMuSquared)),


          new Vector3(0,   0.5f * (-1 + muSquared), -oneOverSqrt2 * (-1 + muSquared)),
          new Vector3(0, 0.5f * (-1 + muSquared),  oneOverSqrt2 * (-1 + muSquared))*/
        }
        return singularityPoints;
    }
}











