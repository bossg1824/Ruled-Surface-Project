using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeFunctions
{
    public delegate void TimeFunction(Transform target);

    public enum TimeFunctionName {ChangeRulingColorToGreen, ChangeRulingColorBack, ChangeOutsideColorToGreen, ChangeOutsideColorBack, Activate, Deactivate };
    public static TimeFunction[] functions = { ChangeRulingColorToGreen, ChangeRulingColorBack, ChangeOutsideColorToGreen, ChangeOutsideColorBack, Activate, Deactivate };
    
    public static TimeFunction GetTimeFunction(int function)
    {
        return functions[function];
    }

    public static TimeFunction GetTimeFunction(TimeFunctionName function)
    {
        return GetTimeFunction((int)function);
    }

    public static void ChangeRulingColorToGreen(Transform target)
    {
        GrababbleRuled changingColor = target.GetComponent<GrababbleRuled>();
        if (changingColor != null)
        {
            Debug.Log("Changing color"); 
            changingColor.changeRulingColor(Color.green);
        } else
        {
            Debug.Log("Failed");
        }
    }

    public static void ChangeRulingColorBack(Transform target)
    {
        GrababbleRuled changingColor = target.GetComponent<GrababbleRuled>();
        if (changingColor != null)
        {
            changingColor.changeRulingColor(changingColor.getRulingColor());
        }
    }
    public static void ChangeOutsideColorToGreen(Transform target)
    {
        GrababbleRuled changingColor = target.GetComponent<GrababbleRuled>();
        if (changingColor != null)
        {
            changingColor.changeOutsideColor(3, Color.green);
        }
    }

    public static void ChangeOutsideColorBack(Transform target)
    {
        GrababbleRuled changingColor = target.GetComponent<GrababbleRuled>();
        if(changingColor != null)
        {
            changingColor.changeOutsideColor(3, changingColor.getOutsideColor());
        }
    }
    public static void Activate(Transform target)
    {
        target.gameObject.SetActive(true);
    }

    public static void Deactivate(Transform target)
    {
        target.gameObject.SetActive(false);
    }
}
