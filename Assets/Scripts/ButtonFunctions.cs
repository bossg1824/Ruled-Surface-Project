using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions
{
    public delegate void buttonFunction(Transform target);
    public enum buttonFuncName {ToIntro, ToDevelopable, ToQuadrics, ToCubics, ToQuartics, Check};
    public static buttonFunction[] buttonFunctions = {ToIntro, ToDevelopable, ToQuadrics, ToCubics, ToQuartics, Check};

    public static buttonFunction GetButtonFunction(int index)
    {
        return buttonFunctions[index];
    } 

    public static buttonFunction GetButtonFunction(buttonFuncName buttonFunctionName)
    {
        return buttonFunctions[(int)buttonFunctionName];
    }

    public static void ToIntro(Transform target)
    {
        SceneManager.LoadScene("Intro");
    }

    public static void ToDevelopable(Transform target)
    {
        SceneManager.LoadScene("Developable");
    }
    public static void ToQuadrics(Transform target)
    {
        SceneManager.LoadScene("Quadrics");
    }
    public static void ToCubics(Transform target)
    {
        SceneManager.LoadScene("Cubics");
    }
    public static void ToQuartics(Transform target)
    {
        SceneManager.LoadScene("Quartics");
    }

    public static void Check(Transform target)
    {
        bool success = true;
        TargetsToCheck targets = target.GetComponent<TargetsToCheck>();
        foreach(PedistalScript p in targets.targets)
        {
            if(p != null)
            {
                if (!p.OnCheck())
                {
                    success = false;
                    break;
                }
            }
        }

        foreach(Light l in targets.lightsToIndicate)
        {
            if(l != null)
            {
                l.color = success ? Color.red : Color.green;
            }
        }
    }
}
