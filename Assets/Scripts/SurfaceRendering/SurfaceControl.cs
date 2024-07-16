
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class SurfaceControl : MonoBehaviour
{

    public List<GameObject> surfaceObjects;
    public List<Surface> surfaces;
    public SlicePlane slicePlane;
    public int numberOfChildren;
    private bool buttonDown;
    private int selectedFunction;
    private List<int> functions = new List<int>{1,2,3,4,5,6,7,8};
    public List<NewMarker> newMarkers = new List<NewMarker>();


    public void setFunction(int x)
    {
        selectedFunction = x;
    }

    public int getFunction()
    {
        return selectedFunction;
    }

    public bool checkButton()
    {
        return buttonDown;
    }

    public void buttonStatus(bool button)
    {
        buttonDown = button;
    }

    public void SetFunctions(List<int> rf)
    {
        functions = rf;
    }

    public void InitializeSurfaces()
    {
        
         
        numberOfChildren = transform.childCount;

        surfaceObjects = new List<GameObject>(numberOfChildren);
        surfaces = new List<Surface>(numberOfChildren);
        for (int i = 0; i < numberOfChildren; i++)
        {
            surfaceObjects.Add(transform.GetChild(i).gameObject);
            Surface surface = surfaceObjects[i].GetComponent<Surface>();
            surfaces.Add(surface);
            surfaces[surfaces.Count-1].function = functions[i];            
        }
    }
}
