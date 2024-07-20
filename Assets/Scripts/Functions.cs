using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Mathf;

public class Functions
{
    public delegate Vector3[] Function(int numpoints, float scale, bool negatives, float timesThrough, Vector3 offset, Quaternion rotation);

    public enum FunctionName { Circle, Line, Helix, Parabola, CircleSine, SinLine, Oloid1, Oloid2 };
    static Function[] functions = { Circle, Line, Helix, Parabola, CircleSine, SinLine, Oloid1, Oloid2 };


    public static Function GetFunction(int index)
    {
        return functions[index];
    }

    public static Function GetFunction(FunctionName name)
    {
        return functions[(int)name];
    }
    public static Vector3[] Circle(int numpoints, float scale, bool negatives, float timesThrough, Vector3 offset, Quaternion rotation)
    {
        Vector3[] vals = new Vector3[numpoints];

        for (int i = 0, j = negatives ? -(numpoints / 2) : 0; i < numpoints; i++, j++)
        {
            float progress = 2 * PI * j * timesThrough/ (float)numpoints;
            Vector3 next = new Vector3();
            next.x = scale * Cos(progress);
            next.z = scale * Sin(progress);
            next.y = 0;
            vals[i] = next;
        }
        ApplyOfRot(vals, offset, rotation);
        return vals;
    }

    public static Vector3[] Line(int numpoints, float scale, bool negatives, float timesThrough, Vector3 offset, Quaternion rotation)
    {
        Vector3[] vals = new Vector3[numpoints];

        for (int i = 0, j = negatives ? -(numpoints / 2) : 0; i < numpoints; i++, j++)
        {
            Vector3 next = new Vector3();
            next.y = scale * j * timesThrough/ (float)numpoints;
            next.z = 0;
            next.x = 0;
            vals[i] = next;
        }
        ApplyOfRot(vals, offset, rotation);
        return vals;
    }

    public static Vector3[] Helix(int numpoints, float scale, bool negatives, float timesThrough, Vector3 offset, Quaternion rotation)
    {
        Vector3[] vals = new Vector3[numpoints];

        for (int i = 0, j = negatives ? -(numpoints / 2) : 0; i < numpoints; i++, j++)
        {
            float progress = 2 * PI * j * timesThrough / (float)numpoints;
            Vector3 next = new Vector3();
            next.x = scale * Cos(progress);
            next.z = scale * Sin(progress);
            next.y = scale * j * timesThrough/ (float)numpoints;
            vals[i] = next;
        }
        ApplyOfRot(vals, offset, rotation);
        return vals;
    }

    public static Vector3[] Parabola(int numpoints, float scale, bool negatives, float timesThrough, Vector3 offset, Quaternion rotation)
    {
        Vector3[] vals = new Vector3[numpoints];

        for (int i = 0, j = negatives ? -(numpoints / 2) : 0; i < numpoints; i++, j++)
        {
            float progress =  j * timesThrough / (float)numpoints;
            Vector3 next = new Vector3();
            next.x = scale * progress;
            next.z = 0;
            next.y = next.x * next.x;
            vals[i] = next;
        }
        ApplyOfRot(vals, offset, rotation);
        return vals;
    }

    public static Vector3[] CircleSine(int numpoints, float scale, bool negatives, float timesThrough, Vector3 offset, Quaternion rotation)
    {
        Vector3[] vals = new Vector3[numpoints];

        for (int i = 0, j = negatives ? -(numpoints / 2) : 0; i < numpoints; i++, j++)
        {
            float progress = 2 * PI * j * timesThrough / (float)numpoints;
            Vector3 next = new Vector3();
            next.x = scale * Cos(progress);
            next.z = scale * Sin(progress);
            next.y = scale * Sin(progress * 3);
            vals[i] = next;
        }
        ApplyOfRot(vals, offset, rotation);
        return vals;
    }

    public static Vector3[] SinLine(int numpoints, float scale, bool negatives, float timesThrough, Vector3 offset, Quaternion rotation)
    {
        Vector3[] vals = new Vector3[numpoints];

        for (int i = 0, j = negatives ? -(numpoints / 2) : 0; i < numpoints; i++, j++)
        {
            Vector3 next = new Vector3();
            float progress = 2 * PI * j * timesThrough / (float)numpoints;
            next.z = scale * Sin(progress);
            next.y = 0;
            next.x = 0;
            vals[i] = next;
        }
        ApplyOfRot(vals, offset, rotation);
        return vals;
    }

    public static Vector3[] Oloid1(int numpoints, float scale, bool negatives, float timesThrough, Vector3 offset, Quaternion rotation) 
    {
        Vector3[] vals = new Vector3[numpoints];

        for (int i = 0, j = negatives ? -(numpoints / 2) : 0; i < numpoints; i++, j++)
        {
            Vector3 next = new Vector3();
            float progress = 2 * PI * j * timesThrough / (float)numpoints;
            next.x = scale * Cos(progress);
            next.y = 0;
            next.z = scale * Sin(progress);
            vals[i] = next;
        }
        ApplyOfRot(vals, offset, rotation);
        return vals;
    }

    public static Vector3[] Oloid2(int numpoints, float scale, bool negatives, float timesThrough, Vector3 offset, Quaternion rotation)
    {
        Vector3[] vals = new Vector3[numpoints];

        for (int i = 0, j = negatives ? -(numpoints / 2) : 0; i < numpoints; i++, j++)
        {
            Vector3 next = new Vector3();
            float progress = 2 * PI * j * timesThrough / (float)numpoints;
            next.x = 0;
            next.y = scale * Sin(progress);
            next.z = scale * Cos(progress);
            vals[i] = next;
        }
        ApplyOfRot(vals, offset, rotation);
        return vals;
    }

    private static void ApplyOfRot(Vector3[] input, Vector3 offset, Quaternion rotation)
    {
        for (int i = 0; i < input.Length; i++)
        {
            input[i] =  rotation * input[i] + offset;
        }
    }
}    

