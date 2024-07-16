using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Marker : MonoBehaviour
{

    private int function = 0;
    public GameObject markerPrefab;
    public float muSquared = 2.0f;
    private List<Chunk> markers;
    private Chunk marker;
    private float size = 1.0f;
    private float scale = 16.5f;
    private float otherSize = 0.08f;
    //public Vector3 localScale = new Vector3(0.2f, 0.2f, 0.2f);

    void Start()
    {
        float offsetVal = (GridMetrics.PointsPerChunk / 2 - 0.5f) * (size / 100f);
        markers = new List<Chunk>();
    }

     public void CreateSingularityMarkers()
    {
        // Calculate the coordinates of the singularities
        Vector3[] singularityPoints = CalculateSingularityPoints(muSquared);

        // Instantiate markers at the singularity points
        foreach (Vector3 point in singularityPoints)
        {
         //   Vector3 localPoint = Vector3.Scale(point, localScale);
            CreateMarker(point);
        }
    }

        public void CreateMarker(Vector3 pos)
    {
        marker = Instantiate(markerPrefab, transform.position + pos, Quaternion.identity, transform).GetComponent<Chunk>();
        marker.size = size;
        marker.otherSize = otherSize;
        markers.Add(marker);
        Render(markers.Count-1);
    }

    public void DestroyMarkers()
    {
        int ct = markers.Count;
        for (int i=0; i<ct; i++) 
        {
            Destroy(markers[i].gameObject);
        }
        markers.Clear();
    }

  

    private void Render(int i)
    {
        NoiseGenerator ng = markers[i].GetComponent<NoiseGenerator>();
        ng.scale = scale;
        ng.size = size;
        ng.function = function;
        ng.otherSize = otherSize;
        ng.Offset = markers[i].transform.position - transform.position;
        ng.muSquared = 2;
        markers[i].Render();
    }

    public int Count()
    {
        return markers.Count;
    }



  



    private Vector3[] CalculateSingularityPoints(float muSquared)
    {
        float sqrt2 = Mathf.Sqrt(2);
        float sqrt1PlusMuSquared = Mathf.Sqrt(1 + muSquared);
        float sqrtMinus1PlusMuSquared = Mathf.Sqrt(-1 + muSquared);
        float oneOverSqrt2 = 1.0f / sqrt2;

        // Calculate the coordinates of the singularities based on the provided information
        Vector3[] singularityPoints = new Vector3[]
        {
            new Vector3(sqrtMinus1PlusMuSquared, 0, -1),
            new Vector3(-sqrtMinus1PlusMuSquared, 0, -1),
            new Vector3(0, -sqrtMinus1PlusMuSquared, 1),
            new Vector3(0, sqrtMinus1PlusMuSquared, 1),
            new Vector3(-oneOverSqrt2 * (-1 + muSquared), 0, oneOverSqrt2 * (1 - muSquared)),
            new Vector3(oneOverSqrt2 * (-1 + muSquared), 0, -oneOverSqrt2 * (1 - muSquared)),
            new Vector3(oneOverSqrt2 * sqrt1PlusMuSquared + sqrt2 * sqrtMinus1PlusMuSquared, oneOverSqrt2 * sqrt1PlusMuSquared - sqrt2 * sqrtMinus1PlusMuSquared, -sqrtMinus1PlusMuSquared / sqrt2),
            new Vector3(-oneOverSqrt2 * sqrt1PlusMuSquared - sqrt2 * sqrtMinus1PlusMuSquared, -oneOverSqrt2 * sqrt1PlusMuSquared + sqrt2 * sqrtMinus1PlusMuSquared, -sqrtMinus1PlusMuSquared / sqrt2),
            new Vector3(oneOverSqrt2 * sqrt1PlusMuSquared - sqrt2 * sqrtMinus1PlusMuSquared, oneOverSqrt2 * sqrt1PlusMuSquared + sqrt2 * sqrtMinus1PlusMuSquared, sqrtMinus1PlusMuSquared / sqrt2),
            new Vector3(-oneOverSqrt2 * sqrt1PlusMuSquared + sqrt2 * sqrtMinus1PlusMuSquared, -oneOverSqrt2 * sqrt1PlusMuSquared - sqrt2 * sqrtMinus1PlusMuSquared, sqrtMinus1PlusMuSquared / sqrt2),
            new Vector3(oneOverSqrt2 * sqrt1PlusMuSquared - sqrt2 * sqrtMinus1PlusMuSquared, -oneOverSqrt2 * sqrt1PlusMuSquared + sqrt2 * sqrtMinus1PlusMuSquared, -sqrtMinus1PlusMuSquared / sqrt2),
            new Vector3(-oneOverSqrt2 * sqrt1PlusMuSquared + sqrt2 * sqrtMinus1PlusMuSquared, oneOverSqrt2 * sqrt1PlusMuSquared + sqrt2 * sqrtMinus1PlusMuSquared, sqrtMinus1PlusMuSquared / sqrt2),
            new Vector3(oneOverSqrt2 * sqrt1PlusMuSquared + sqrt2 * sqrtMinus1PlusMuSquared, -oneOverSqrt2 * sqrt1PlusMuSquared - sqrt2 * sqrtMinus1PlusMuSquared, sqrtMinus1PlusMuSquared / sqrt2),
            new Vector3(-oneOverSqrt2 * sqrt1PlusMuSquared - sqrt2 * sqrtMinus1PlusMuSquared, oneOverSqrt2 * sqrt1PlusMuSquared - sqrt2 * sqrtMinus1PlusMuSquared, sqrtMinus1PlusMuSquared / sqrt2),
            new Vector3(0, -oneOverSqrt2 * (1 + muSquared), -oneOverSqrt2 * (1 - muSquared)),
            new Vector3(0, oneOverSqrt2 * (1 + muSquared), oneOverSqrt2 * (1 - muSquared))
        };

        return singularityPoints;
    }
}











